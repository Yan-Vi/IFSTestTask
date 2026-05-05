namespace IFSTests.Utils.Logger.Implementations;

public sealed class CompositeLogger(IReadOnlyCollection<ILogger> loggers) : ILogger
{
    public void Error(string message)
    {
        foreach (var logger in loggers)
        {
            logger.Error(message);
        }
    }

    public void Error(string message, params object[] args)
    {
        foreach (var logger in loggers)
        {
            logger.Error(message, args);
        }
    }

    public void Warning(string message)
    {
        foreach (var logger in loggers)
        {
            logger.Warning(message);
        }
    }

    public void Warning(string message, params object[] args)
    {
        foreach (var logger in loggers)
        {
            logger.Warning(message, args);
        }
    }

    public void Info(string message)
    {
        foreach (var logger in loggers)
        {
            logger.Info(message);
        }
    }

    public void Info(string message, params object[] args)
    {
        foreach (var logger in loggers)
        {
            logger.Info(message, args);
        }
    }

    public void Debug(string message)
    {
        foreach (var logger in loggers)
        {
            logger.Debug(message);
        }
    }

    public void Debug(string message, params object[] args)
    {
        foreach (var logger in loggers)
        {
            logger.Debug(message, args);
        }
    }

    public void Step(string stepName, Action? action = null)
    {
        var chain = action;
        var list = loggers as IReadOnlyList<ILogger> ?? loggers.ToArray();
        for (var i = list.Count - 1; i >= 0; i--)
        {
            var logger = list[i];
            var inner = chain;
            chain = () => logger.Step(stepName, inner);
        }
        chain?.Invoke();
    }

    public T Step<T>(string stepName, Func<T>? func = null)
    {
        var chain = func;
        var list = loggers as IReadOnlyList<ILogger> ?? loggers.ToArray();
        for (var i = list.Count - 1; i >= 0; i--)
        {
            var logger = list[i];
            var inner = chain;
            chain = () => logger.Step(stepName, inner);
        }
        return chain is null ? default! : chain();
    }

    public Task StepAsync(string stepName, Func<Task>? action = null)
    {
        var chain = action;
        var list = loggers as IReadOnlyList<ILogger> ?? loggers.ToArray();
        for (var i = list.Count - 1; i >= 0; i--)
        {
            var logger = list[i];
            var inner = chain;
            chain = () => logger.StepAsync(stepName, inner);
        }
        return chain?.Invoke() ?? Task.CompletedTask;
    }

    public Task<T> StepAsync<T>(string stepName, Func<Task<T>>? func = null)
    {
        var chain = func;
        var list = loggers as IReadOnlyList<ILogger> ?? loggers.ToArray();
        for (var i = list.Count - 1; i >= 0; i--)
        {
            var logger = list[i];
            var inner = chain;
            chain = () => logger.StepAsync(stepName, inner);
        }
        return chain is null ? Task.FromResult(default(T)!) : chain();
    }
}
