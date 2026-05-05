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
    public async Task GetPostById_ReturnsExpectedPost(int postId, CancellationToken cancellationToken)
    {
        _log.Step($"Post id to lookup is {postId}");
        var response = await _postsController.GetPostById(postId, cancellationToken);
        _log.Step("Check if response status code is 200", () =>
        {
            response.StatusCode
                .Should()
                .Be(HttpStatusCode.OK, "expected GET /posts/{id} to return 200.");
        });
        _log.Step("Check if response body contains the requested post id, userId, title, and body", () =>
        {
            response.Body.Should().NotBeNull("expected response body to contain a post.");
            var post = response.Body!;
            using (new AssertionScope())
            {
                post.Id
                    .Should()
                    .Be(postId, "expected response to contain the requested post id.");
                post.UserId
                    .Should()
                    .BeGreaterThan(0, "expected userId to be positive.");
                post.Title
                    .Should()
                    .NotBeNullOrEmpty("expected title to be non-empty.");
                post.Body
                    .Should()
                    .NotBeNullOrEmpty("expected body to be non-empty.");
            }
        });
    }

    [CancelAfter(Timeouts.DefaultTimeoutMs)]
    [AllureName("GET /posts/{id} returns HTTP 404 for non-existent post")]
    [TestCase(10101, Description = "Validates that GET /posts/{id} returns HTTP 404 for a non-existent post.")]
    public async Task GetPostById_ForNonExistentPost_Returns404(int nonExistentPostId, CancellationToken cancellationToken)
    {
        _log.Step($"Post id to lookup is {nonExistentPostId}");
        var response = await _postsController.GetPostById(nonExistentPostId, cancellationToken);
        _log.Step("Check if response is unsuccessful", () =>
        {
            response.IsSuccessStatusCode
                .Should()
                .BeFalse("expected non-existent post lookup to be unsuccessful.");
        });
        _log.Step("Check if response status code is 404", () =>
        {
            response.StatusCode
                .Should()
                .Be(HttpStatusCode.NotFound, "expected GET /posts/{id} to return 404 for non-existent post.");
        });
    }
}
