using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceStackGenerators
{
    public class CodeTemplates
    {
        public const string AppHostCodeTemplate = @"
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ServiceStack;
using Funq;

namespace {0}
{{
    public class AppHost : AppHostBase
    {{
        public AppHost() : base(""{1}"", typeof({2}).Assembly) {{ }}

        // Configure your AppHost with the necessary configuration and dependencies your App needs
        public override void Configure(Container container)
        {{
            SetConfig(new HostConfig
            {{
                DebugMode = AppSettings.Get(nameof(HostConfig.DebugMode), false),
            }});
        }}
    }}
}}";

        public const string EntryPointTemplate = @"
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ServiceStack;
using Funq;

namespace {0}
{{
    public class Program
    {{
        public static void Main(string[] args)
        {{
            CreateHostBuilder(args).Build().Run();
        }}

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(builder =>
                {{
                    builder.UseModularStartup<Startup>();
                }});
    }}
}}";

        public const string StartupTemplate = @"
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ServiceStack;
using Funq;

namespace {0}
{{
    public class Startup : ModularStartup
    {{
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public new void ConfigureServices(IServiceCollection services)
        {{
        }}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IWebHostEnvironment env)
        {{
            if (env.IsDevelopment())
            {{
                app.UseDeveloperExceptionPage();
            }}

            app.UseServiceStack(new AppHost
            {{
                AppSettings = new NetCoreAppSettings(Configuration)
            }});
        }}
    }}
}}";

        public const string AuthTemplate = @"
using Microsoft.Extensions.DependencyInjection;
using ServiceStack;
using ServiceStack.Auth;

namespace {0}
{{
    public class ConfigureAuth : IConfigureAppHost
    {{
        public void Configure(IAppHost appHost)
        {{
            var appSettings = appHost.AppSettings;
            appHost.Plugins.Add(new AuthFeature(() => new AuthUserSession(),
                new IAuthProvider[] {{
                    new CredentialsAuthProvider(appSettings),     /* Sign In with Username / Password credentials */
                    new FacebookAuthProvider(appSettings),        /* Create App https://developers.facebook.com/apps */
                    new GoogleAuthProvider(appSettings),          /* Create App https://console.developers.google.com/apis/credentials */
                    new MicrosoftGraphAuthProvider(appSettings),  /* Create App https://apps.dev.microsoft.com */
                }}));

            appHost.Plugins.Add(new RegistrationFeature()); //Enable /register Service
        }}
    }}
}}
";
    }
}
