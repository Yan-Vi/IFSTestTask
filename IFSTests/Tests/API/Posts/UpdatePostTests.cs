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
[AllureSubSuite("PUT /posts/{id}")]
public class UpdatePostTests
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

    [Test(Description = "Validates that PUT /posts/{id} updates a post and responds with HTTP 200.")]
    [CancelAfter(Timeouts.DefaultTimeoutMs)]
    [AllureName("PUT /posts/{id} updates post and responds with HTTP 200")]
    public async Task UpdatePost_UpdatesExistingPost(CancellationToken cancellationToken)
    {
        PostInfoDTO updatedPost = null!;
        _log.Step("Create sample post for update", () =>
        {
            updatedPost = new PostInfoDTO
            {
                Id = 1,
                UserId = 7,
                Title = "updated title",
                Body = "updated body"
            };
        });
        var response = await _postsController.UpdatePost(updatedPost.Id, updatedPost, cancellationToken);
        _log.Step("Check if response status code is 200", () =>
        {
            response.StatusCode
                .Should()
                .Be(HttpStatusCode.OK, "expected PUT /posts/{id} to return 200.");
        });
        _log.Step("Check if response body matches updated post", () =>
        {
            response.Body?
                .Should()
                .NotBeNull("expected response body to be present.")
                .And
                .BeEquivalentTo(updatedPost, "Expected response body to match updated post.");
        });
    }
}
