using System.Security.Claims;
using ToDoLibrary.JsonRpc;

namespace ToDoWebApi.JsonRpc
{
    public interface IJsonRpcService : IDisposable
    {
        Task<JsonRpcResponse> ProcessRequest(ClaimsPrincipal claimsPrincipal, string json, Dictionary<string, FunctionCall> functionCalls);
    }
}
