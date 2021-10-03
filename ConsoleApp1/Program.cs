using Microsoft.Extensions.Hosting;
using ServiceStack;
using System;
using ServiceStack.Auth;
using Funq;
using Microsoft.AspNetCore.Hosting;

namespace Console1
{
    //[Authenticate]
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


    //public class AppHost : AppHostBase
    //{
    //    public AppHost() : base("Test", typeof(FooService).Assembly) { }

    //    // Configure your AppHost with the necessary configuration and dependencies your App needs
    //    public override void Configure(Container container)
    //    {
    //        // enable server-side rendering, see: https://sharpscript.net/docs/sharp-pages
    //        Plugins.Add(new SharpPagesFeature
    //        {
    //            EnableSpaFallback = true
    //        });

    //        SetConfig(new HostConfig
    //        {
    //            AddRedirectParamsToQueryString = true,
    //            DebugMode = AppSettings.Get(nameof(HostConfig.DebugMode), HostingEnvironment.IsDevelopment())
    //        });
    //    }
    //}


    //public class Program
    //{
    //    public static void Main(string[] args)
    //    {
    //        CreateHostBuilder(args).Build().Run();
    //    }

    //    public static IHostBuilder CreateHostBuilder(string[] args) =>
    //        Host.CreateDefaultBuilder(args)
    //            .ConfigureWebHostDefaults(builder =>
    //            {
    //                builder.UseModularStartup<Startup>();
    //            });
    //}

    //public class ConfigureAuth : IConfigureAppHost
    //{
    //    public void Configure(IAppHost appHost)
    //    {
    //        {
    //            var appSettings = appHost.AppSettings;
    //            appHost.Plugins.Add(new AuthFeature(() => new AuthUserSession(),
    //                new IAuthProvider[] {
    //                new CredentialsAuthProvider(appSettings),     /* Sign In with Username / Password credentials */
    //                new FacebookAuthProvider(appSettings),        /* Create App https://developers.facebook.com/apps */
    //                new GoogleAuthProvider(appSettings),          /* Create App https://console.developers.google.com/apis/credentials */
    //                new MicrosoftGraphAuthProvider(appSettings),  /* Create App https://apps.dev.microsoft.com */
    //            }));

    //            appHost.Plugins.Add(new RegistrationFeature {
    //            AtRestPath = "/my-rego"}) ; //Enable /register Service
    //        }
    //    }
    //}
}