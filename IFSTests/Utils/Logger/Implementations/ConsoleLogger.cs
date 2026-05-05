using System.Globalization;

namespace IFSTests.Utils.Logger.Implementations;

public sealed class ConsoleLogger : ILogger
{
    public void Error(string message) => Write("ERROR", message);

    public void Error(string message, params object[] args) => Write("ERROR", Format(message, args));

    public void Warning(string message) => Write("WARN", message);

    public void Warning(string message, params object[] args) => Write("WARN", Format(message, args));

    public void Info(string message) => Write("INFO", message);

    public void Info(string message, params object[] args) => Write("INFO", Format(message, args));

    public void Debug(string message) => Write("DEBUG", message);

    public void Debug(string message, params object[] args) => Write("DEBUG", Format(message, args));

    private static string Format(string message, object[] args) =>
        args.Length == 0
            ? message
            : string.Format(CultureInfo.InvariantCulture, message, args);

    private static void Write(string level, string message) => TestContext.Out.WriteLine("[{0:O}] [{1}] {2}", DateTime.UtcNow, level, message);

    public void Step(string stepName, Action? action = null)
    {
        Write("STEP START", stepName);
        try
        {
            action?.Invoke();
        }
        finally
        {
            Write("STEP END", stepName);
        }
    }

    public T Step<T>(string stepName, Func<T>? func = null)
    {
        Write("STEP START", stepName);
        try
        {
            return func is null ? default! : func();
        }
        finally
        {
            Write("STEP END", stepName);
        }
    }

    public async Task StepAsync(string stepName, Func<Task>? func = null)
    {
        Write("STEP START", stepName);
        try
        {
            if (func is null)
                return;
            await func.Invoke();
        }
        finally
        {
            Write("STEP END", stepName);
        }
    }

    public async Task<T> StepAsync<T>(string stepName, Func<Task<T>>? func = null)
    {
        Write("STEP START", stepName);
        try
        {
            return func is null ? default! : await func();
        }
        finally
        {
            Write("STEP END", stepName);
        }
    }
}
