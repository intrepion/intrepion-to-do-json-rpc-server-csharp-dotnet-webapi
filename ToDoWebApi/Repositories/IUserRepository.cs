using System.Security.Claims;
using ToDoLibrary.JsonRpc;

namespace ToDoWebApi.Repositories
{
    public interface IUserRepository : IDisposable
    {
        Task<JsonRpcResponse> LoginAsync(JsonRpcRequest request);
        JsonRpcResponse Logout(JsonRpcRequest request);
        JsonRpcResponse Refresh(JsonRpcRequest request);
        Task<JsonRpcResponse> RegisterAsync(JsonRpcRequest request);
        JsonRpcResponse Revoke(ClaimsPrincipal claimsPrincipal, JsonRpcRequest request);
    }
}
