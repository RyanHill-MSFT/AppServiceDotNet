using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using common.models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;
using Newtonsoft.Json;

namespace funcapp
{
    public class TodoFunction
    {
        private readonly ILogger<TodoFunction> log;
        private const string GetTodos_Query = @"select * from dbo.Todo";

        public TodoFunction(ILogger<TodoFunction> log)
        {
            this.log = log;
        }

        [FunctionName(nameof(GetTodos))]
        public async Task<IActionResult> GetTodos(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "todos")] HttpRequest req,
            [Sql(GetTodos_Query, CommandType = CommandType.Text, ConnectionStringSetting = "SqlConnectionString")] IEnumerable<Todo> todos)
        {
            return new OkObjectResult(todos);
        }

        [FunctionName(nameof(PostTodo))]
        [RequiredScope("access_as_user")]
        public async Task<IActionResult> PostTodo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "todos")] HttpRequest req,
            [Sql("dbo.Todo", ConnectionStringSetting = "SqlConnectionString")] IAsyncCollector<Todo> todos)
        {
            var (authenticationStatus, authenticationResponse) = await req.HttpContext.AuthenticateAzureFunctionAsync();
            if (!authenticationStatus)
            {
                return authenticationResponse;
            }

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            log.LogInformation("Request: {0}", req.Path);
            log.LogInformation("Body: {0}", requestBody);

            var item = JsonConvert.DeserializeObject<Todo>(requestBody);
            item.Id = Guid.NewGuid();

            await todos.AddAsync(item);
            await todos.FlushAsync();
            List<Todo> todoList = new() { item };

            return new OkObjectResult(todoList);
        }
    }
}
