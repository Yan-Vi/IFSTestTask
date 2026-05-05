namespace IFSTests.Utils.Logger.Implementations;

public sealed class NullLogger : ILogger
{
    public void Error(string message)
    {
    }

    public void Error(string message, params object[] args)
    {
    }

    public void Warning(string message)
    {
    }

    public void Warning(string message, params object[] args)
    {
    }

    public void Info(string message)
    {
    }

    public void Info(string message, params object[] args)
    {
    }

    public void Debug(string message)
    {
    }

    public void Debug(string message, params object[] args)
    {
    }

    public void Step(string stepName, Action? action = null) => action?.Invoke();

    public T Step<T>(string stepName, Func<T>? func = null) => func is null ? default! : func();

    public Task StepAsync(string stepName, Func<Task>? action = null) => action is null ? Task.CompletedTask : action.Invoke();

    public Task<T> StepAsync<T>(string stepName, Func<Task<T>>? func = null) => func is null ? Task.FromResult(default(T)!) : func.Invoke();
}
