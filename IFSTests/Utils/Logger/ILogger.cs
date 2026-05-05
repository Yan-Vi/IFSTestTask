namespace IFSTests.Utils.Logger;

public interface ILogger
{
    void Error(string message);
    void Error(string message, params object[] args);
    void Warning(string message);
    void Warning(string message, params object[] args);
    void Info(string message);
    void Info(string message, params object[] args);
    void Debug(string message);
    void Debug(string message, params object[] args);
    void Step(string stepName, Action? action = null);
    T Step<T>(string stepName, Func<T>? func = null);
    Task StepAsync(string stepName, Func<Task>? action = null);
    Task<T> StepAsync<T>(string stepName, Func<Task<T>>? func = null);
}
