using BFloEcommerce.Protos;
using Grpc.Core;

namespace BFloEcommerce.Services
{
    public class TodoAllService : TodoService.TodoServiceBase
    {
        private readonly ILogger<TodoAllService> _logger;

        public TodoAllService(ILogger<TodoAllService> logger)
        {
            _logger = logger;
        }

        public override Task<TodoResponse> TodoSend(TodoRequest request, ServerCallContext context)
        {
            string resultMsg = $"Hello {request.Name}";

            var response = new TodoResponse
            {
                Message = resultMsg
            };

            return Task.FromResult(response);
        }
    }
}
