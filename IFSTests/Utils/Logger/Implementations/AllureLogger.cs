using System.Globalization;
using Allure.Net.Commons;

namespace IFSTests.Utils.Logger.Implementations;

public sealed class AllureLogger : ILogger
{
    public void Error(string message) => Write("ERROR", message);

    public void Error(string message, params object[] args) => Write("ERROR", Format(message, args));

    public void Warning(string message) => Write("WARN", message);

    public void Warning(string message, params object[] args) => Write("WARN", Format(message, args));

    public void Info(string message) => Write("INFO", message);

    public void Info(string message, params object[] args) => Write("INFO", Format(message, args));

    public void Debug(string message) => Write("DEBUG", message);

    public void Debug(string message, params object[] args) => Write("DEBUG", Format(message, args));

    public void Step(string stepName, Action? action = null)
    {
        if (action is null)
        {
            AllureApi.Step(stepName);
            return;
        }
        AllureApi.Step(stepName, action);
    }

    public T Step<T>(string stepName, Func<T>? func = null)
    {
        if (func is null)
        {
            AllureApi.Step(stepName);
            return default!;
        }
        return AllureApi.Step(stepName, func);
    }

    public Task StepAsync(string stepName, Func<Task>? action = null)
    {
        if (action is null)
        {
            AllureApi.Step(stepName);
            return Task.CompletedTask;
        }
        return AllureApi.Step(stepName, action);
    }

    public Task<T> StepAsync<T>(string stepName, Func<Task<T>>? func = null)
    {
        if (func is null)
        {
            AllureApi.Step(stepName);
            return Task.FromResult(default(T)!);
        }
        return AllureApi.Step(stepName, func);
    }

    private static string Format(string message, object[] args)
    {
        return args.Length == 0
            ? message
            : string.Format(CultureInfo.InvariantCulture, message, args);
    }

    private static void Write(string level, string message) => AllureApi.Step($"[{level}] {message}");
}
