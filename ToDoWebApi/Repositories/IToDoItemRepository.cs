using System.Security.Claims;
using ToDoLibrary.JsonRpc;

namespace ToDoWebApi.Repositories
{
    public interface IToDoItemRepository : IDisposable
    {
        Task<JsonRpcResponse> AllToDoItemsAsync(ClaimsPrincipal claimsPrincipal, JsonRpcRequest request);
        Task<JsonRpcResponse> EditToDoItemAsync(ClaimsPrincipal claimsPrincipal, JsonRpcRequest request);
        Task<JsonRpcResponse> NewToDoItemAsync(ClaimsPrincipal claimsPrincipal, JsonRpcRequest request);
    }
}
