using Microsoft.Extensions.Hosting;
using ServiceStack;
using System;

namespace Console1
{
    [Authenticate]
    public class FooService : Service
    {
        public object Any(Hello request)
        {
            return new HelloResponse { Result = $"Hello, {request.Name}!" };
        }
    }

    public class Hello
    {
        public string Name { get; set; }
    }

    public class HelloResponse
    {
        public string Result { get; set; }
    }
}