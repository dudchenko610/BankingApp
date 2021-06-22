using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PresentationLayer.Middlewares
{
    public class ConsoleLogMiddleware
    {
        private RequestDelegate _next;

        public ConsoleLogMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            Console.WriteLine("                                         _");
            Console.WriteLine(context.Request.Path);
            Console.WriteLine("                                         _");


            Console.WriteLine("__________________ Request Headers __________________");
            foreach (var e in context.Request.Headers)
            {
                Console.WriteLine("[] " + e.Key + "   :   " + e.Value);
            }


            Console.WriteLine("__________________ Response Headers __________________");
            foreach (var e in context.Response.Headers)
            {
                Console.WriteLine("[] " + e.Key + "   :   " + e.Value);
            }

            await _next.Invoke(context);
        }

    }
}
