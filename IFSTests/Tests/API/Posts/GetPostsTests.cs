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
        _log.Step("Check if response status code is 200", () => {
            Assert.That(
                response.StatusCode,
                Is.EqualTo(HttpStatusCode.OK),
                "Expected GET /posts to return 200."
            );
        });
    }

    [Test(Description = "Validates that GET /posts returns exactly 100 posts.")]
    [CancelAfter(Timeouts.DefaultTimeoutMs)]
    [AllureName("GET /posts returns exactly 100 posts")]
    public async Task GetAllPosts_ReturnsExpectedNumberOfPosts(CancellationToken cancellationToken)
    {
        var response = await _postsController.GetAllPosts(cancellationToken);
        _log.Step("Check if response status code is 200", () => {
            Assert.That(
                response.StatusCode,
                Is.EqualTo(HttpStatusCode.OK),
                "Expected GET /posts to return 200."
            );
        });
        _log.Step("Check if response body contains exactly 100 posts", () => {
            Assert.That(
                response.Body,
                Has.Length.EqualTo(100),
                "Expected JSONPlaceholder to return 100 posts."
            );
        });
    }

    [Test(Description = "Validates that GET /posts returns required post structure.")]
    [CancelAfter(Timeouts.DefaultTimeoutMs)]
    [AllureName("GET /posts returns required post structure")]
    public async Task GetAllPosts_ReturnsPostWithRequiredStructure(CancellationToken cancellationToken)
    {
        var response = await _postsController.GetAllPosts(cancellationToken);

        _log.Step("Check if response status code is 200", () => {
            Assert.That(
                response.StatusCode,
                Is.EqualTo(HttpStatusCode.OK),
                "Expected GET /posts to return 200."
            );
        });
        _log.Step("Check if response body contains at least one post", () => {
            Assert.That(
                response.Body,
                Is.Not.Null.And.Not.Empty,
                "Expected response body to include at least one post."
            );
        });
        _log.Step("Check if response body contains the required post structure", () =>
        {
            Assert.Multiple(() =>
            {
                for (var i = 0; i < response.Body!.Length; i++)
                {
                    var post = response.Body[i];
                    Assert.That(
                        post.Id,
                        Is.GreaterThan(0),
                        $"Expected posts[{i}].id to be positive."
                    );
                    Assert.That(
                        post.UserId,
                        Is.GreaterThan(0),
                        $"Expected posts[{i}].userId to be positive."
                    );
                    Assert.That(
                        post.Title,
                        Is.Not.Empty,
                        $"Expected posts[{i}].title to be non-empty."
                    );
                    Assert.That(
                        post.Body,
                        Is.Not.Empty,
                        $"Expected posts[{i}].body to be non-empty."
                    );
                }
            });
        });
    }
}
