using IFSTests.TestObjectModels.API.Posts;

namespace IFSTests.TestObjectModels.API;

public class ApiClient(PostsController postsController)
{
    public PostsController PostsController { get; } = postsController;
}