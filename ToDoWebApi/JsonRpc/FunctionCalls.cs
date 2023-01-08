namespace ToDoWebApi.JsonRpc;

public static class FunctionCalls
{
    public static Dictionary<string, FunctionCall> Dictionary = new Dictionary<string, FunctionCall>
    {
        {
            "all_to_do_items", new FunctionCall
            {
                Parameters = new List<Parameter> {
                    new Parameter { Name = "guid", Kind = "string" },
                },
            }
        },
        {
            "all_to_do_lists", new FunctionCall
            {
                Parameters = new List<Parameter> {},
            }
        },
        {
            "edit_to_do_item", new FunctionCall
            {
                Parameters = new List<Parameter>
                {
                    new Parameter { Name = "complete", Kind = "boolean" },
                    new Parameter { Name = "guid", Kind = "string" },
                    new Parameter { Name = "text", Kind = "string" },
                    new Parameter { Name = "visible", Kind = "boolean" },
                },
            }
        },
        {
            "edit_to_do_list", new FunctionCall
            {
                Parameters = new List<Parameter>
                {
                    new Parameter { Name = "complete", Kind = "boolean" },
                    new Parameter { Name = "guid", Kind = "string" },
                    new Parameter { Name = "title", Kind = "string" },
                    new Parameter { Name = "visible", Kind = "boolean" },
                },
            }
        },
        {
            "login", new FunctionCall
            {
                Parameters = new List<Parameter>
                {
                    new Parameter { Name = "username", Kind = "string" },
                    new Parameter { Name = "password", Kind = "string" },
                },
            }
        },
        {
            "logout", new FunctionCall
            {
                Parameters = new List<Parameter> {},
            }
        },
        {
            "new_to_do_item", new FunctionCall
            {
                Parameters = new List<Parameter>
                {
                    new Parameter { Name = "text", Kind = "string" },
                }
            }
        },
        {
            "new_to_do_list", new FunctionCall
            {
                Parameters = new List<Parameter>
                {
                    new Parameter { Name = "title", Kind = "string" },
                }
            }
        },
        {
            "refresh", new FunctionCall
            {
                Parameters = new List<Parameter>
                {
                    new Parameter { Name = "access_token", Kind = "string" },
                    new Parameter { Name = "refresh_token", Kind = "string" },
                },
            }
        },
        {
            "register", new FunctionCall
            {
                Parameters = new List<Parameter>
                {
                    new Parameter { Name = "confirm", Kind = "string" },
                    new Parameter { Name = "email", Kind = "string" },
                    new Parameter { Name = "password", Kind = "string" },
                    new Parameter { Name = "username", Kind = "string" },
                },
            }
        },
        {
            "revoke", new FunctionCall
            {
                Parameters = new List<Parameter> {},
            }
        }
    };
}
