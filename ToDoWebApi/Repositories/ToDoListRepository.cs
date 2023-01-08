using Microsoft.EntityFrameworkCore;
using ToDoLibrary.Domain;
using ToDoLibrary.JsonRpc;
using ToDoWebApi.Data;
using ToDoWebApi.Entities;
using ToDoWebApi.Params;
using ToDoWebApi.Results;
using System.Text.Json;
using System.Security.Claims;

namespace ToDoWebApi.Repositories;

public class ToDoListRepository : IToDoListRepository, IDisposable
{
    private readonly ApplicationDbContext _context;

    public ToDoListRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<JsonRpcResponse> AllToDoListsAsync(ClaimsPrincipal claimsPrincipal, JsonRpcRequest request)
    {
        if (_context.ToDoLists == null)
        {
            return new JsonRpcResponse
            {
                Id = request.Id,
                JsonRpc = request.JsonRpc,
                Error = new JsonRpcError
                {
                    Code = -32600,
                    Message = "internal error - to do items is not found",
                },
            };
        }

        if (claimsPrincipal == null || claimsPrincipal.Identity == null || claimsPrincipal.Identity.Name == null) {
            return new JsonRpcResponse {
                Id = request.Id,
                JsonRpc = request.JsonRpc,
                Error = new JsonRpcError {
                    Code = 5,
                    Message = "Bad request - claim does not exist.",
                },
            };
        }

        var userName = claimsPrincipal.Identity.Name;
        var applicationUser = await _context.Users.Where(au => au.UserName == userName).FirstOrDefaultAsync();

        if (applicationUser == null) {
            return new JsonRpcResponse {
                Id = request.Id,
                JsonRpc = request.JsonRpc,
                Error = new JsonRpcError {
                    Code = 6,
                    Message = "Bad request - user does not exist.",
                },
            };
        }

        var toDoLists = await (from ToDoLists in _context.ToDoLists
                               join Users in _context.Users on ToDoLists.ApplicationUser!.Id equals Users.Id
                               where Users.UserName == userName
                               select ToDoLists).ToListAsync();

        return new JsonRpcResponse
        {
            Id = request.Id,
            JsonRpc = request.JsonRpc,
            Result = new AllToDoListsResult
            {
                ToDoLists = toDoLists.Select(toDoList => new AllToDoListsResultToDoList
                {
                    Complete = toDoList.Complete,
                    Guid = toDoList.Guid,
                    Title = toDoList.Title,
                    Visible = toDoList.Visible,
                }).ToList()
            },
        };
    }

    public async Task<JsonRpcResponse> EditToDoListAsync(ClaimsPrincipal claimsPrincipal, JsonRpcRequest request)
    {
        if (request.Params == null)
        {
            return new JsonRpcResponse
            {
                Id = request.Id,
                JsonRpc = request.JsonRpc,
                Error = new JsonRpcError
                {
                    Code = -32602,
                    Message = "Invalid params - params is not found",
                },
            };
        }

        JsonElement requestParams = (JsonElement)request.Params;

        var editToDoListParams = JsonSerializer.Deserialize<EditToDoListParams>(requestParams.GetRawText());

        if (editToDoListParams == null || string.IsNullOrEmpty(editToDoListParams.Title))
        {
            return new JsonRpcResponse
            {
                Id = request.Id,
                JsonRpc = request.JsonRpc,
                Error = new JsonRpcError
                {
                    Code = -32602,
                    Message = "Invalid params - text is not found",
                },
            };
        }

        var title = editToDoListParams.Title.Trim();

        if (_context.ToDoLists == null)
        {
            return new JsonRpcResponse
            {
                Id = request.Id,
                JsonRpc = request.JsonRpc,
                Error = new JsonRpcError
                {
                    Code = -32602,
                    Message = "internal error - to do items is not found",
                },
            };
        }

        var toDoList = await _context.ToDoLists.Where(toDoList => toDoList.Title == title).FirstOrDefaultAsync();
        if (toDoList == null) {
            return new JsonRpcResponse
            {
                Id = request.Id,
                JsonRpc = request.JsonRpc,
                Error = new JsonRpcError
                {
                    Code = -32602,
                    Message = "internal error - to do item does not exist",
                },
            };
        }

        var message = ToDoList.CreateToDoList(title);

        toDoList = new ToDoListEntity
        {
            Title = title,
        };

        await _context.AddAsync(toDoList);
        _context.SaveChanges();

        return new JsonRpcResponse
        {
            Id = request.Id,
            JsonRpc = request.JsonRpc,
            Result = new NewToDoListResult
            {
                Title = title
            },
        };
    }

    public async Task<JsonRpcResponse> NewToDoListAsync(ClaimsPrincipal claimsPrincipal, JsonRpcRequest request)
    {
        if (request.Params == null)
        {
            return new JsonRpcResponse
            {
                Id = request.Id,
                JsonRpc = request.JsonRpc,
                Error = new JsonRpcError
                {
                    Code = -32602,
                    Message = "Invalid params - params is not found",
                },
            };
        }

        JsonElement requestParams = (JsonElement)request.Params;

        var newToDoListParams = JsonSerializer.Deserialize<NewToDoListParams>(requestParams.GetRawText());

        if (newToDoListParams == null || string.IsNullOrEmpty(newToDoListParams.Title))
        {
            return new JsonRpcResponse
            {
                Id = request.Id,
                JsonRpc = request.JsonRpc,
                Error = new JsonRpcError
                {
                    Code = -32602,
                    Message = "Invalid params - text is not found",
                },
            };
        }

        var title = newToDoListParams.Title.Trim();

        if (_context.ToDoLists == null)
        {
            return new JsonRpcResponse
            {
                Id = request.Id,
                JsonRpc = request.JsonRpc,
                Error = new JsonRpcError
                {
                    Code = -32602,
                    Message = "internal error - to do items is not found",
                },
            };
        }

        if (claimsPrincipal == null || claimsPrincipal.Identity == null || claimsPrincipal.Identity.Name == null) {
            return new JsonRpcResponse {
                Id = request.Id,
                JsonRpc = request.JsonRpc,
                Error = new JsonRpcError {
                    Code = 5,
                    Message = "Bad request - claim does not exist.",
                },
            };
        }

        var userName = claimsPrincipal.Identity.Name;
        var applicationUser = await _context.Users.Where(au => au.UserName == userName).FirstOrDefaultAsync();

        if (applicationUser == null) {
            return new JsonRpcResponse {
                Id = request.Id,
                JsonRpc = request.JsonRpc,
                Error = new JsonRpcError {
                    Code = 6,
                    Message = "Bad request - user does not exist.",
                },
            };
        }

        var toDoList = await _context.ToDoLists.Where(toDoList => toDoList.Title == title).FirstOrDefaultAsync();
        if (toDoList != null) {
            return new JsonRpcResponse
            {
                Id = request.Id,
                JsonRpc = request.JsonRpc,
                Error = new JsonRpcError
                {
                    Code = -32602,
                    Message = "internal error - to do item already exists",
                },
            };
        }

        toDoList = new ToDoListEntity
        {
            ApplicationUser = applicationUser,
            Complete = false,
            Guid = Guid.NewGuid(),
            Title = title,
            Visible = true,
        };

        await _context.AddAsync(toDoList);
        _context.SaveChanges();

        return new JsonRpcResponse
        {
            Id = request.Id,
            JsonRpc = request.JsonRpc,
            Result = new NewToDoListResult
            {
                Title = title
            },
        };
    }

    private bool disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
        this.disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
