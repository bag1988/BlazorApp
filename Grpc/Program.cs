using BlazorApp.Grpc.Data;
using BlazorApp.Grpc.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.

builder.Services.AddDbContext<MessagesDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MessageConnection")));



builder.Services.AddScoped<IMessageInterface, MessageRelease>();
builder.Services.AddGrpc();

builder.Services.AddCors(o => o.AddPolicy("AllowAll", builder =>
{
    builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
        .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding");
}));

var app = builder.Build();


app.UseStaticFiles();
app.UseCors();
app.UseGrpcWeb();
// Configure the HTTP request pipeline.
app.MapGrpcService<MessageService>().RequireCors("AllowAll").EnableGrpcWeb(); ;
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
