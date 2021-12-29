using BlazorApp.Client;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using ServiceGrpc;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddSingleton(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var serverUrl = config["ServerUrl"];

    var channel = GrpcChannel.ForAddress(serverUrl, new GrpcChannelOptions
    {
        HttpHandler = new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler())
    });

    var client = new MessageContext.MessageContextClient(channel);

    return client;
});

await builder.Build().RunAsync();
