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
[AllureSubSuite("DELETE /posts/{id}")]
public class DeletePostTests
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
    [AllureName("DELETE /posts/{id} responds with HTTP 200")]
    [TestCase(1, Description = "Validates that DELETE /posts/{id} responds with HTTP 200.")]
    public async Task DeletePost_Returns200(int postId, CancellationToken cancellationToken)
    {
        _log.Step($"Post id to delete is {postId}");
        var response = await _postsController.DeletePost(postId, cancellationToken);
        _log.Step("Check if response status code is 200", () =>
        {
            response.StatusCode
                .Should()
                .Be(HttpStatusCode.OK, "expected DELETE /posts/{id} to return 200.");
        });
    }
}
