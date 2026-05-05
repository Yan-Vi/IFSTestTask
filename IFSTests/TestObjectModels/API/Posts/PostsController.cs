using IFSTests.TestObjectModels.App;
using IFSTests.Utils.RestClient;
using IFSTests.Utils.RestClient.ResponseModel;

namespace IFSTests.TestObjectModels.API.Posts;

public class PostsController(
    IRestClient restClient,
    AppTestingConfig appTestingConfig,
    PostsControllerConfig postsControllerConfig
)
{
    private Uri BaseUri => new($"{appTestingConfig.BaseUrl}/{postsControllerConfig.RelativePath}");

    private Uri GetUriForPostId(int id)
    {
        return new($"{BaseUri.AbsoluteUri}/{id}");
    }

    public Task<Response<PostInfoDTO[]>> GetAllPosts(CancellationToken cancellationToken = default)
    {
        return restClient.GetAsync<PostInfoDTO[]>(BaseUri, cancellationToken);
    }

    public Task<Response<PostInfoDTO>> GetPostById(int id, CancellationToken cancellationToken = default)
    {
        return restClient.GetAsync<PostInfoDTO>(GetUriForPostId(id), cancellationToken);
    }

    public Task<Response<PostInfoDTO>> CreatePost(PostInfoDTO postInfo, CancellationToken cancellationToken = default)
    {
        return restClient.PostAsync<PostInfoDTO, PostInfoDTO>(BaseUri, postInfo, cancellationToken);
    }

    public Task<Response<PostInfoDTO>> UpdatePost(int id, PostInfoDTO postInfo, CancellationToken cancellationToken = default)
    {
        return restClient.PutAsync<PostInfoDTO, PostInfoDTO>(GetUriForPostId(id), postInfo, cancellationToken);
    }

    public Task<Response<string>> DeletePost(int id, CancellationToken cancellationToken = default)
    {
        return restClient.DeleteAsync(GetUriForPostId(id), cancellationToken);
    }
}