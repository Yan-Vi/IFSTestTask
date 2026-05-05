using System.Runtime.CompilerServices;
using DotNetEnv;

namespace IFSTests;

internal static class EnvFileBootstrap
{
    [ModuleInitializer]
    internal static void Initialize()
    {
        for (var directory = new DirectoryInfo(AppContext.BaseDirectory); directory != null; directory = directory.Parent)
        {
            var candidate = Path.Combine(directory.FullName, ".env");
            if (!File.Exists(candidate))
            {
                continue;
            }

            Env.Load(candidate);
            return;
        }
    }
}
