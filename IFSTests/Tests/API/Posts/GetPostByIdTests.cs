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
[AllureSubSuite("GET /posts/{id}")]
public class GetPostByIdTests
{
    private PostsController _postsController = null!;
    private ILogger _log;

    [SetUp]
    public void Setup()
    {
        var app = AppFactory.Create();
        _postsController = app.ApiClient.PostsController;
        _log = app.Logger;
    }

    [CancelAfter(Timeouts.DefaultTimeoutMs)]
    [AllureName("GET /posts/{id} returns expected post with HTTP 200")]
    [TestCase(1, Description = "Validates that GET /posts/{id} returns the expected post with HTTP 200.")]
    public async Task GetPostById_ReturnsExpectedPost(int postId,CancellationToken cancellationToken)
    {
        _log.Step($"Post id to lookup is {postId}");
        var response = await _postsController.GetPostById(postId, cancellationToken);
        _log.Step("Check if response status code is 200", () => {
            Assert.That(
                response.StatusCode,
                Is.EqualTo(HttpStatusCode.OK),
                "Expected GET /posts/{id} to return 200."
            );
        });
        _log.Step("Check if response contains a body", () => {
            Assert.That(
                response.Body,
                Is.Not.Null,
                "Expected response body to contain a post."
            );
        });
        _log.Step("Check if response body contains the requested post id, userId, title, and body", () => {
            Assert.Multiple(() =>
            {
                Assert.That(
                    response.Body!.Id,
                    Is.EqualTo(postId),
                    "Expected response to contain the requested post id."
                );
                Assert.That(
                    response.Body.UserId,
                    Is.GreaterThan(0),
                    "Expected userId to be positive."
                );
                Assert.That(
                    response.Body.Title,
                    Is.Not.Empty,
                    "Expected title to be non-empty."
                );
                Assert.That(
                    response.Body.Body,
                    Is.Not.Empty,
                    "Expected body to be non-empty."
                );
            });
        });
    }

    [CancelAfter(Timeouts.DefaultTimeoutMs)]
    [AllureName("GET /posts/{id} returns HTTP 404 for non-existent post")]
    [TestCase(10101, Description = "Validates that GET /posts/{id} returns HTTP 404 for a non-existent post.")]
    public async Task GetPostById_ForNonExistentPost_Returns404(int nonExistentPostId, CancellationToken cancellationToken)
    {
        _log.Step($"Post id to lookup is {nonExistentPostId}");
        var response = await _postsController.GetPostById(nonExistentPostId, cancellationToken);
        _log.Step("Check if response is unsuccessful", () => {
            Assert.That(
                response.IsSuccessStatusCode,
                Is.False,
                "Expected non-existent post lookup to be unsuccessful."
            );
        });
        _log.Step("Check if response status code is 404", () =>
        {
            Assert.That(
                response.StatusCode,
                Is.EqualTo(HttpStatusCode.NotFound),
                "Expected GET /posts/{id} to return 404 for non-existent post.");
        });
    }
}
