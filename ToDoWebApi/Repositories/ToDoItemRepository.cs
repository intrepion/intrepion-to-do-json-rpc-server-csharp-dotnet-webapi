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

public class ToDoItemRepository : IToDoItemRepository, IDisposable
{
    private readonly ApplicationDbContext _context;

    public ToDoItemRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<JsonRpcResponse> AllToDoItemsAsync(ClaimsPrincipal claimsPrincipal, JsonRpcRequest request)
    {
        if (_context.ToDoItems == null)
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


        var toDoItems = await _context.ToDoItems.ToListAsync();

        return new JsonRpcResponse
        {
            Id = request.Id,
            JsonRpc = request.JsonRpc,
            Result = new AllToDoItemsResult
            {
                ToDoItems = toDoItems.Select(toDoItem => new AllToDoItemsResultToDoItem
                {
                    Complete = toDoItem.Complete,
                    Guid = toDoItem.Guid,
                    Text = toDoItem.Text,
                    Visible = toDoItem.Visible,
                }).ToList()
            },
        };
    }

    public async Task<JsonRpcResponse> EditToDoItemAsync(ClaimsPrincipal claimsPrincipal, JsonRpcRequest request)
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

        var editToDoItemParams = JsonSerializer.Deserialize<EditToDoItemParams>(requestParams.GetRawText());

        if (editToDoItemParams == null || string.IsNullOrEmpty(editToDoItemParams.Text))
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

        var text = editToDoItemParams.Text.Trim();

        if (_context.ToDoItems == null)
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

        var toDoItem = await _context.ToDoItems.Where(toDoItem => toDoItem.Text == text).FirstOrDefaultAsync();
        if (toDoItem == null) {
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

        var message = ToDoItem.CreateToDoItem(text);

        toDoItem = new ToDoItemEntity
        {
            Text = text,
        };

        await _context.AddAsync(toDoItem);
        _context.SaveChanges();

        return new JsonRpcResponse
        {
            Id = request.Id,
            JsonRpc = request.JsonRpc,
            Result = new NewToDoItemResult
            {
                Text = text
            },
        };
    }

    public async Task<JsonRpcResponse> NewToDoItemAsync(ClaimsPrincipal claimsPrincipal, JsonRpcRequest request)
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

        var newToDoItemParams = JsonSerializer.Deserialize<NewToDoItemParams>(requestParams.GetRawText());

        if (newToDoItemParams == null || string.IsNullOrEmpty(newToDoItemParams.Text))
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

        var text = newToDoItemParams.Text.Trim();

        if (_context.ToDoItems == null)
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

        var toDoItem = await _context.ToDoItems.Where(toDoItem => toDoItem.Text == text).FirstOrDefaultAsync();
        if (toDoItem != null) {
            return new JsonRpcResponse
            {
                Id = request.Id,
                JsonRpc = request.JsonRpc,
                Error = new JsonRpcError
                {
                    Code = -32602,
                    Message = "internal error - to do item is already exists",
                },
            };
        }

        var message = ToDoItem.CreateToDoItem(text);

        toDoItem = new ToDoItemEntity
        {
            Text = text,
        };

        await _context.AddAsync(toDoItem);
        _context.SaveChanges();

        return new JsonRpcResponse
        {
            Id = request.Id,
            JsonRpc = request.JsonRpc,
            Result = new NewToDoItemResult
            {
                Text = text
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
