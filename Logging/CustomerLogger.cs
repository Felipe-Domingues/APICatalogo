
namespace APICatalogo.Logging;
public class CustomerLogger : ILogger
{
    readonly string loggerName;
    readonly CustomLoggerProviderConfiguration loggerConfig;

    public CustomerLogger(string name, CustomLoggerProviderConfiguration config)
    {
        loggerName = name;
        loggerConfig = config;
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel == loggerConfig.LogLevel;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        string mensagem = $"{logLevel.ToString()}: {eventId.Id} - {formatter(state, exception)}";

        WriteTextIntoFile(mensagem);
    }

    private void WriteTextIntoFile(string message)
    {
        if (!Path.Exists("./Log"))
            Directory.CreateDirectory("./Log");

        string caminhoArquivoLog = $"./Log/{DateTime.Now.ToString("dd_MM_yyyy")}.log";
        using (StreamWriter streamWriter = new StreamWriter(caminhoArquivoLog, true))
        {
            try
            {
                streamWriter.WriteLine(message);
                streamWriter.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
