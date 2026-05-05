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
[AllureSubSuite("GET /posts")]
public class GetPostsTests
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

    [Test(Description = "Validates that GET /posts responds with HTTP 200.")]
    [CancelAfter(Timeouts.DefaultTimeoutMs)]
    [AllureName("GET /posts responds with HTTP 200")]
    public async Task GetAllPosts_Returns200Ok(CancellationToken cancellationToken)
    {
        var response = await _postsController.GetAllPosts(cancellationToken);
        _log.Step("Check if response status code is 200", () =>
        {
            response.StatusCode
                .Should()
                .Be(HttpStatusCode.OK, "expected GET /posts to return 200.");
        });
    }

    [Test(Description = "Validates that GET /posts returns exactly 100 posts.")]
    [CancelAfter(Timeouts.DefaultTimeoutMs)]
    [AllureName("GET /posts returns exactly 100 posts")]
    public async Task GetAllPosts_ReturnsExpectedNumberOfPosts(CancellationToken cancellationToken)
    {
        var response = await _postsController.GetAllPosts(cancellationToken);
        _log.Step("Check if response status code is 200", () =>
        {
            response.StatusCode
                .Should()
                .Be(HttpStatusCode.OK, "expected GET /posts to return 200.");
        });
        _log.Step("Check if response body contains exactly 100 posts", () =>
        {
            response.Body
                .Should()
                .HaveCount(100, "expected JSONPlaceholder to return 100 posts.");
        });
    }

    [Test(Description = "Validates that GET /posts returns required post structure.")]
    [CancelAfter(Timeouts.DefaultTimeoutMs)]
    [AllureName("GET /posts returns required post structure")]
    public async Task GetAllPosts_ReturnsPostWithRequiredStructure(CancellationToken cancellationToken)
    {
        var response = await _postsController.GetAllPosts(cancellationToken);

        _log.Step("Check if response status code is 200", () =>
        {
            response.StatusCode
                .Should()
                .Be(HttpStatusCode.OK, "expected GET /posts to return 200.");
        });
        _log.Step("Check if response body contains at least one post", () =>
        {
            response.Body?
                .Should()
                .NotBeNullOrEmpty("expected response body to include at least one post.");
        });
        _log.Step("Check if response body contains the required post structure", () =>
        {
            response.Body!.Should().AllSatisfy(post =>
            {
                post.Id
                    .Should()
                    .BeGreaterThan(0, "Expected id to be positive.");
                post.UserId.Should()
                    .BeGreaterThan(0, "Expected userId to be positive.");
                post.Title
                    .Should()
                    .NotBeNullOrEmpty("Expected title to be non-empty.");
                post.Body
                    .Should()
                    .NotBeNullOrEmpty("Expected body to be non-empty.");
            });
        });
    }
}
