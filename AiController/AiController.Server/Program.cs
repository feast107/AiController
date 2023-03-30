using AiController.Abstraction.Communication;
using AiController.Communication.GPT35;
using AiController.Operation.Operators.Direct;
using AiController.Server.Hubs;
using AiController.Server.Service;
using OpenAI.Chat;

namespace AiController.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSingleton(typeof(IAsyncCommunicator<ChatPrompt[]>), typeof(Gpt35AsyncCommunicator));
            builder.Services.AddSingleton<Gpt35DistributeEventOperator>();
            builder.Services.AddSingleton(typeof(IHubDispatchService<>), typeof(HubMessageDispatcher<>));
            builder.Services.AddSignalR();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();
            app.MapHub<MessageHub>("/server");
            app.Run();
        }
    }
}