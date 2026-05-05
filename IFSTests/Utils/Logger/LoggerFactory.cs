using IFSTests.Utils.Logger.Implementations;

namespace IFSTests.Utils.Logger;

public static class LoggerFactory
{
    public static ILogger Create(string loggerType)
    {
        var loggerTypes = loggerType.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        if (loggerTypes.Length == 0)
            return new NullLogger();
        if (loggerTypes.Length == 1)
            return CreateLogger(loggerTypes[0]);
        var loggers = new List<ILogger>(loggerTypes.Length);
        foreach (var currentLoggerType in loggerTypes)
        {
            loggers.Add(CreateLogger(currentLoggerType));
        }
        return new CompositeLogger(loggers);
    }

    private static ILogger CreateLogger(string loggerType)
    {
        switch (loggerType)
        {
            case nameof(ConsoleLogger):
                return new ConsoleLogger();
            case nameof(AllureLogger):
                return new AllureLogger();
            case nameof(NullLogger):
                return new NullLogger();
            default:
                new ConsoleLogger().Error("Unsupported logger type '{0}'. Falling back to '{1}'.", loggerType, nameof(NullLogger));
                return new NullLogger();
        }
    }
}
