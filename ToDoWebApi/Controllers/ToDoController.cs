using Microsoft.AspNetCore.Mvc;
using ToDoLibrary.JsonRpc;
using ToDoWebApi.JsonRpc;

namespace ToDoWebApi.Controllers;

[ApiController]
[Route("/")]
public class ToDoController : ControllerBase
{
    private readonly IJsonRpcService _jsonRpcService;
    private readonly ILogger<ToDoController> _logger;

    public ToDoController(
        IJsonRpcService jsonRpcService,
        ILogger<ToDoController> logger
        )
    {
        _jsonRpcService = jsonRpcService;
        _logger = logger;
    }

    [HttpPost(Name = "PostToDo")]
    public async Task<JsonRpcResponse> Post()
    {
        Request.EnableBuffering();

        Request.Body.Position = 0;

        var json = await new StreamReader(Request.Body).ReadToEndAsync();

        return await _jsonRpcService.ProcessRequest(User, json, FunctionCalls.Dictionary);
    }
}
