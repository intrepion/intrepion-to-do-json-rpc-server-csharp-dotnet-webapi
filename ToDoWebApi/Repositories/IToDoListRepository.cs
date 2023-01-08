using System.Security.Claims;
using ToDoLibrary.JsonRpc;

namespace ToDoWebApi.Repositories
{
    public interface IToDoListRepository : IDisposable
    {
        Task<JsonRpcResponse> AllToDoListsAsync(ClaimsPrincipal claimsPrincipal, JsonRpcRequest request);
        Task<JsonRpcResponse> EditToDoListAsync(ClaimsPrincipal claimsPrincipal, JsonRpcRequest request);
        Task<JsonRpcResponse> NewToDoListAsync(ClaimsPrincipal claimsPrincipal, JsonRpcRequest request);
    }
}
