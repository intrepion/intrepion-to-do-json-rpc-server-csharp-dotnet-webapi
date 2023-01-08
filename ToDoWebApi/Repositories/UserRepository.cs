using Microsoft.AspNetCore.Identity;
using ToDoLibrary.JsonRpc;
using ToDoWebApi.Data;
using ToDoWebApi.Entities;
using ToDoWebApi.Params;
using ToDoWebApi.Results;
using System.Security.Claims;
using System.Text.Json;
using ToDoWebApi.Token;

namespace ToDoWebApi.Repositories;

public class UserRepository : IUserRepository, IDisposable
{
    private readonly IConfiguration _configuration;
    private readonly ApplicationDbContext _context;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserRepository(
        IConfiguration configuration,
        ApplicationDbContext context,
        SignInManager<ApplicationUser> signInManager,
        ITokenService tokenService,
        UserManager<ApplicationUser> userManager
        )
    {
        _context = context;
        _configuration = configuration;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _userManager = userManager;
    }
    
    public async Task<JsonRpcResponse> LoginAsync(JsonRpcRequest request)
    {
        if (request.Params == null)
        {
            return new JsonRpcResponse
            {
                JsonRpc = "2.0",
                Error = new JsonRpcError
                {
                    Code = -32602,
                    Message = "Invalid params - request.Params is not found",
                },
            };
        }

        JsonElement requestParams = (JsonElement)request.Params;

        var loginParams = JsonSerializer.Deserialize<LoginParams>(requestParams.GetRawText());
        if (loginParams == null)
        {
            return new JsonRpcResponse
            {
                JsonRpc = "2.0",
                Error = new JsonRpcError
                {
                    Code = -32602,
                    Message = "Invalid params - loginParams is not found",
                },
            };
        }

        if (loginParams.UserName == null)
        {
            return new JsonRpcResponse
            {
                JsonRpc = "2.0",
                Error = new JsonRpcError
                {
                    Code = -32602,
                    Message = "Invalid params - loginParams.UserName is not found",
                },
            };
        }

        if (loginParams.Password == null)
        {
            return new JsonRpcResponse
            {
                JsonRpc = "2.0",
                Error = new JsonRpcError
                {
                    Code = -32602,
                    Message = "Invalid params - loginParams.Password is not found",
                },
            };
        }

        var userName = loginParams.UserName.Trim();
        var password = loginParams.Password;

        var result = await _signInManager.PasswordSignInAsync(userName, password, false, false);

        if (result.Succeeded)
        {
            var clientUrl = _configuration["ClientUrl"] ??
                Environment.GetEnvironmentVariable("CLIENT_URL");

            if (clientUrl == null)
            {
                return new JsonRpcResponse
                {
                    JsonRpc = "2.0",
                    Error = new JsonRpcError
                    {
                        Code = -32600,
                        Message = "internal error - clientUrl is not found",
                    },
                };
            }

            var jwtIssuer = _configuration["JwtIssuer"] ??
                Environment.GetEnvironmentVariable("JWT_ISSUER");

            if (jwtIssuer == null)
            {
                return new JsonRpcResponse
                {
                    JsonRpc = "2.0",
                    Error = new JsonRpcError
                    {
                        Code = -32600,
                        Message = "internal error - jwtIssuer is not found",
                    },
                };
            }

            var jwtSecretKey = _configuration["JwtSecretKey"] ??
                Environment.GetEnvironmentVariable("JWT_SECRET_KEY");

            if (jwtSecretKey == null)
            {
                return new JsonRpcResponse
                {
                    JsonRpc = "2.0",
                    Error = new JsonRpcError
                    {
                        Code = -32600,
                        Message = "internal error - jwtSecretKey is not found",
                    },
                };
            }

            var user = _context.Users.FirstOrDefault(u => u.UserName == userName);

            if (user == null)
            {
                return new JsonRpcResponse
                {
                    JsonRpc = "2.0",
                    Error = new JsonRpcError
                    {
                        Code = -32600,
                        Message = "internal error - user is not found",
                    },
                };
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userName),
            };
            var accessToken = _tokenService.GenerateAccessToken(claims, clientUrl, jwtIssuer, jwtSecretKey);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

            return new JsonRpcResponse {
                Id = request.Id,
                JsonRpc = request.JsonRpc,
                Result = new LoginResult {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    User = new LoginResultUser {
                        Guid = user.Guid,
                        UserName = user.UserName,
                    },
                },
            };
        }

        return new JsonRpcResponse {
            Id = request.Id,
            JsonRpc = request.JsonRpc,
            Error = new JsonRpcError {
                Code = 1,
                Message = "Invalid login attempt.",
                Data = result,
            },
        };
    }

    public JsonRpcResponse Logout(JsonRpcRequest request)
    {
        return new JsonRpcResponse {
            Id = request.Id,
            JsonRpc = request.JsonRpc,
        };
    }

    public JsonRpcResponse Refresh(JsonRpcRequest request)
    {
        var clientUrl = _configuration["ClientUrl"] ??
            Environment.GetEnvironmentVariable("CLIENT_URL");

        if (clientUrl == null)
        {
            return new JsonRpcResponse
            {
                JsonRpc = "2.0",
                Error = new JsonRpcError
                {
                    Code = -32600,
                    Message = "internal error - clientUrl is not found",
                },
            };
        }

        var jwtIssuer = _configuration["JwtIssuer"] ??
            Environment.GetEnvironmentVariable("JWT_ISSUER");

        if (jwtIssuer == null)
        {
            return new JsonRpcResponse
            {
                JsonRpc = "2.0",
                Error = new JsonRpcError
                {
                    Code = -32600,
                    Message = "internal error - jwtIssuer is not found",
                },
            };
        }

        var jwtSecretKey = _configuration["JwtSecretKey"] ??
            Environment.GetEnvironmentVariable("JWT_SECRET_KEY");

        if (jwtSecretKey == null)
        {
            return new JsonRpcResponse
            {
                JsonRpc = "2.0",
                Error = new JsonRpcError
                {
                    Code = -32600,
                    Message = "internal error - jwtSecretKey is not found",
                },
            };
        }

        if (request.Params == null)
        {
            return new JsonRpcResponse
            {
                JsonRpc = "2.0",
                Error = new JsonRpcError
                {
                    Code = -32602,
                    Message = "Invalid params - request.Params is not found",
                },
            };
        }

        JsonElement requestParams = (JsonElement)request.Params;

        var refreshParams = JsonSerializer.Deserialize<RefreshParams>(requestParams.GetRawText());

        if (refreshParams == null)
        {
            return new JsonRpcResponse {
                Id = request.Id,
                JsonRpc = request.JsonRpc,
                Error = new JsonRpcError {
                    Code = 2,
                    Message = "Invalid client request.",
                },
            };
        }
        
        if (refreshParams.AccessToken == null || refreshParams.RefreshToken == null)
        {
            return new JsonRpcResponse {
                Id = request.Id,
                JsonRpc = request.JsonRpc,
                Error = new JsonRpcError {
                    Code = 2,
                    Message = "Invalid client request.",
                },
            };
        }

        var accessToken = refreshParams.AccessToken;
        var refreshToken = refreshParams.RefreshToken;
        var principal = _tokenService.GetPrincipalFromExpiredToken(jwtSecretKey, accessToken);

        if (principal == null)
        {
            return new JsonRpcResponse {
                Id = request.Id,
                JsonRpc = request.JsonRpc,
                Error = new JsonRpcError {
                    Code = 2,
                    Message = "Invalid client request.",
                },
            };
        }

        if (principal.Identity == null)
        {
            return new JsonRpcResponse {
                Id = request.Id,
                JsonRpc = request.JsonRpc,
                Error = new JsonRpcError {
                    Code = 2,
                    Message = "Invalid client request.",
                },
            };
        }

        if (principal.Identity.Name == null)
        {
            return new JsonRpcResponse {
                Id = request.Id,
                JsonRpc = request.JsonRpc,
                Error = new JsonRpcError {
                    Code = 2,
                    Message = "Invalid client request.",
                },
            };
        }

        var userName = principal.Identity.Name;
        var user = _context.Users.SingleOrDefault(u => u.UserName == userName);
        if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
        {
            return new JsonRpcResponse {
                Id = request.Id,
                JsonRpc = request.JsonRpc,
                Error = new JsonRpcError {
                    Code = 2,
                    Message = "Invalid client request.",
                },
            };
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, userName),
        };
        var newAccessToken = _tokenService.GenerateAccessToken(claims, clientUrl, jwtIssuer, jwtSecretKey);
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        _context.SaveChanges();

        return new JsonRpcResponse {
            Id = request.Id,
            JsonRpc = request.JsonRpc,
            Result = new LoginResult {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            },
        };
    }

    public async Task<JsonRpcResponse> RegisterAsync(JsonRpcRequest request)
    {
        if (request.Params == null)
        {
            return new JsonRpcResponse
            {
                JsonRpc = "2.0",
                Error = new JsonRpcError
                {
                    Code = -32602,
                    Message = "Invalid params - request.Params is not found",
                },
            };
        }

        JsonElement requestParams = (JsonElement)request.Params;

        var registerParams = JsonSerializer.Deserialize<RegisterParams>(requestParams.GetRawText());

        if (registerParams == null)
        {
            return new JsonRpcResponse {
                Id = request.Id,
                JsonRpc = request.JsonRpc,
                Error = new JsonRpcError {
                    Code = 2,
                    Message = "Invalid client request.",
                },
            };
        }

        if (registerParams.Email == null || registerParams.Username == null || registerParams.Password == null || registerParams.Confirm == null)
        {
            return new JsonRpcResponse {
                Id = request.Id,
                JsonRpc = request.JsonRpc,
                Error = new JsonRpcError {
                    Code = 2,
                    Message = "Invalid client request.",
                },
            };
        }

        var confirm = registerParams.Confirm;
        var password = registerParams.Password;

        if (confirm != password) {
            return new JsonRpcResponse {
                Id = request.Id,
                JsonRpc = request.JsonRpc,
                Error = new JsonRpcError {
                    Code = 3,
                    Message = "Passwords do not match.",
                },
            };
        }

        var email = registerParams.Email.Trim();
        var userName = registerParams.Username.Trim();

        var user = new ApplicationUser {
            Email = email,
            Guid = Guid.NewGuid(),
            UserName = userName,
        };

        var result = await _userManager.CreateAsync(user, password);

        if (result.Succeeded)
        {
            return new JsonRpcResponse {
                Id = request.Id,
                JsonRpc = request.JsonRpc,
            };
        }

        return new JsonRpcResponse {
            Id = request.Id,
            JsonRpc = request.JsonRpc,
            Error = new JsonRpcError {
                Code = 4,
                Message = "User could not be created.",
                Data = result,
            },
        };
    }

    public JsonRpcResponse Revoke(ClaimsPrincipal claimsPrincipal, JsonRpcRequest request)
    {
        if (claimsPrincipal == null || claimsPrincipal.Identity == null || claimsPrincipal.Identity.Name == null) {
            return new JsonRpcResponse {
                Id = request.Id,
                JsonRpc = request.JsonRpc,
                Error = new JsonRpcError {
                    Code = 5,
                    Message = "Bad request.",
                },
            };
        }

        var userName = claimsPrincipal.Identity.Name;
        var user = _context.Users.SingleOrDefault(u => u.UserName == userName);
        if (user == null) {
            return new JsonRpcResponse {
                Id = request.Id,
                JsonRpc = request.JsonRpc,
                Error = new JsonRpcError {
                    Code = 5,
                    Message = "Bad request.",
                },
            };
        }
        user.RefreshToken = null;
        _context.SaveChanges();
        return new JsonRpcResponse {
            Id = request.Id,
            JsonRpc = request.JsonRpc,
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
