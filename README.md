# ServiceStackGenerators
Playing around with the idea of using .NET Source Generators to provide minimal code play ground for trying out ServiceStack services.

## Idea
Source Generators could be used with frameworks to reduce the amount of known concepts needed to get started as well as allowing developers to provide custom overrides for framework components as they learn about them and explore.
Rather than Source Generators being used as something that is always used in a project, we can use them as a learning aid that is designed to be removed/shed from a project once developers are comfortable enough wtih concepts and want the ability to customize.

> This project is just exploring the about idea, it very well might be a very bad one and at this stage should only be used for fun/toy projects.

## What do these generators do?
The ServiceStackGenerators use .NET Source Generators to fill in the gaps from a minimal 3 class implementation (request DTO, response DTO and Service class) that is hosted with a series of defaults based on ServiceStack mix templates.

The developer can then override these generated classes by declaring them and the source generator will detect and avoid generating. Examples

- If there is no entrypoint (`static void Main(string[] args)`), one will be generated with a default `IHostBuilder` and using standard `Startup` class.
- If `[Authenticate]` is used on the service, a default AuthRepository + Registration service will be added.
- If no modular startup class is declared, one will be generated using the standard named AppHost class.
- If no class using `AppHostBase` is declared, a default one will be generated using name `AppHost` that also detects class name of declared services to use to automatically pick up services to host.

Another goal is that any can be replaced with custom naming, currently it only detects name of custom Service class but can add custom AppHost/Startup/ConfigureAuth etc.

This isn't designed to support all the features of ServiceStack or every variation of setup but rather as an introduction with the ability to emit the generated code to continue on with full customization like a normal ServiceStack project.

## Minimal code example

For the generator to work with minimal service code, 3 classes need to be declared.

- A `Service` class, that is a class inheriting from ServiceStack `Service` class, it can be named whatever you like.
- A Request DTO used in a Service method.
- A Response DTO used in a Service method.

For example.

```csharp
using ServiceStack;

namespace Console1
{
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

```

The above is enough to generate a working ServiceStack application utilizing your custom service. `Route` attributes can be used as normal but are not required if you are using clients like the ServiceStack `JsonServiceClient`.

### Possible future features
- Detect names of custom AppHost + Startup classes rather than rely on naming convensions
- Add friendly analyzer warnings about using default database implementations that use Sqlite in memory, warn about loss of data.
- Probably a bad feature, but some kind of text based minimal config like a `json` or `yml` (I know..) file to enable swapping out features like AuthProviders and providing config.
 - This one is likely a bad idea as it extends the scope but also encourages the idea of baking in possible sensitive details like database connection strings into compiled source.
- Package for NuGet once more polished.

## Development

Generator is not currently on NuGet, if you want to play with this code yourself, build the `ServiceStackGenerators` project and reference as normal. You will need VisualStudio 2019+ to extend/edit the generators. Debugging is sadly most easily done using:

```
#if DEBUG
            if (!Debugger.IsAttached)
            {
                Debugger.Launch();
            }
#endif 
```

### Background
I had this idea from working with Vitepress and thought it would be interesting to have a good server side technology to use with their own minimal example. This would allow rapid client + server side development with just the following files.
- index.md
- package.json
- .servicestack/MyServices.cs
- .servicestack/MyServices.csproj
- .servicestack/Properties/launchSettings.json

Adding a `Dockerfile` and `.github` Actions workflow for GitHub Actions would then be able to iterate and deploy a minimal .NET + Vite/Vue3 JAMStack project.
