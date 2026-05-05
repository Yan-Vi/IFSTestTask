using Allure.NUnit;
using Allure.NUnit.Attributes;
using IFSTests.TestObjectModels.App;
using IFSTests.TestObjectModels.API.Posts;
using System.Net;
using IFSTests.Utils.Logger;

namespace IFSTests.Tests.API.Posts;

[AllureNUnit]
[AllureParentSuite("API")]
[AllureSuite("Posts")]
[AllureSubSuite("POST /posts")]
public class CreatePostTests
{
    private PostsController _postsController;
    private ILogger _log;

    [SetUp]
    public void Setup()
    {
        var app = AppFactory.Create();
        _postsController = app.ApiClient.PostsController;
        _log = app.Logger;
    }

    [Test(Description = "Validates that POST /posts responds with HTTP 201.")]
    [CancelAfter(Timeouts.DefaultTimeoutMs)]
    [AllureName("POST /posts responds with HTTP 201")]
    public async Task CreatePost_Returns201Created(CancellationToken cancellationToken)
    {
        PostInfoDTO newPost = null!;
        _log.Step("Create sample post for response validation", () =>
        {
            newPost = new PostInfoDTO
            {
                UserId = 1,
                Title = "new title",
                Body = "new body"
            };
        });
        var response = await _postsController.CreatePost(newPost, cancellationToken);
        _log.Step("Check if response status code is 201", () =>
        {
            Assert.That(
                response.StatusCode,
                Is.EqualTo(HttpStatusCode.Created),
                "Expected POST /posts to return 201."
            );
        });
    }

    [Test(Description = "Validates that POST /posts response contains submitted data.")]
    [CancelAfter(Timeouts.DefaultTimeoutMs)]
    [AllureName("POST /posts response contains submitted data")]
    public async Task CreatePost_ResponseContainsSubmittedData(CancellationToken cancellationToken)
    {
        PostInfoDTO newPost = null!;
        _log.Step("Create sample post for response validation", () =>
        {
            newPost = new PostInfoDTO
            {
                UserId = 3,
                Title = "submitted title",
                Body = "submitted body"
            };
        });
        var response = await _postsController.CreatePost(newPost, cancellationToken);
        _log.Step("Check if response body has same data as submitted", () =>
        {
            Assert.Multiple(() =>
            {
                Assert.That(
                    response.StatusCode,
                    Is.EqualTo(HttpStatusCode.Created),
                    "Expected POST /posts to return 201."
                );
                Assert.That(
                    response.Body,
                    Is.Not.Null,
                    "Expected response body to be present."
                );
                Assert.That(
                    response.Body!.UserId,
                    Is.EqualTo(newPost.UserId),
                    "Expected response userId to match submitted value."
                );
                Assert.That(
                    response.Body!.Title,
                    Is.EqualTo(newPost.Title),
                    "Expected response title to match submitted value."
                );
                Assert.That(
                    response.Body!.Body,
                    Is.EqualTo(newPost.Body),
                    "Expected response body to match submitted value."
                );
            });
        });
    }
}
