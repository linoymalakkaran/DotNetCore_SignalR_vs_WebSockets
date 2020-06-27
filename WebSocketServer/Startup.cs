using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.WebSockets;
using System.Threading;
using WebSocketServer.Middleware;

namespace WebSocketServer
{
    public class Startup
    {

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddWebSocketServerConnectionManager();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseWebSockets();

            app.UseWebSocketServer();

            // app.Use(async (context, next) =>
            // {
            //     //WriteRequestParameters(context);
            //     if (context.WebSockets.IsWebSocketRequest)
            //     {
            //         WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
            //         Console.WriteLine("WebSocket connected");

            //         await ReceiveMessage(webSocket, async (result, buffer) =>
            //         {
            //             if (result.MessageType == WebSocketMessageType.Text)
            //             {
            //                 Console.WriteLine("Message received");
            //                 return;
            //             }
            //             else if (result.MessageType == WebSocketMessageType.Close)
            //             {
            //                 Console.WriteLine("Received closed message");
            //                 return;
            //             }
            //         });
            //     }
            //     else
            //     {
            //         Console.WriteLine("Hello from second request delegate");
            //         await next();
            //     }
            // });

            app.Run(async context =>
            {
                Console.WriteLine("Hello from 3rd (terminal) Request Delegate");
                await context.Response.WriteAsync("Hello from 3rd (terminal) Request Delegate");
            });
        }

        private async Task ReceiveMessage(WebSocket socket, Action<WebSocketReceiveResult, byte[]> handleMessage)
        {
            var buffer = new byte[1034 * 4];

            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(buffer: new ArraySegment<byte>(buffer),
                     cancellationToken: CancellationToken.None);

                handleMessage(result, buffer);
            }
        }

        // public void WriteRequestParameters(HttpContext context)
        // {
        //     Console.WriteLine($"Request methos: { context.Request.Method }\n Request protocol: {context.Request.Protocol}");

        //     if (context.Request.Headers != null)
        //     {
        //         foreach (var h in context.Request.Headers)
        //         {
        //             Console.WriteLine($"--> {h.Key} : {h.Value}");
        //         }
        //     }
        // }
    }
}
