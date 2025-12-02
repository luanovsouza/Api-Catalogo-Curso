namespace ApiCatalogo.Logging;

public class CustomerLogger : ILogger
{
    readonly string _loggerName;
    readonly CustomLoggerProviderConfiguration _loggerConfig;
    
    
    public CustomerLogger(string name,CustomLoggerProviderConfiguration loggerConfig)
    {
        _loggerConfig = loggerConfig;
        _loggerName = name;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        string mensagem = $"{logLevel.ToString()}: {eventId.Id} - {formatter(state, exception)}";
        
        
    }

    private void WriteArchiveText(string texto)
    {
        var path = @"C:\Users\gamer\Documents\Api Catalogo\TextoError.txt";

        using (StreamWriter sw = new StreamWriter(path, true))
        {
            try
            {
                sw.WriteLine(texto);
                sw.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
    
    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel == _loggerConfig.LogLevel;
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return null;
    }
}