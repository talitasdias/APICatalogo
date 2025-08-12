
namespace APICatalogo.Logging;

public class CustomerLogger(string name, CustomLoggerProviderConfiguration config) : ILogger
{
    readonly string loggerName = name;
    readonly CustomLoggerProviderConfiguration loggerConfig = config;
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

        EscreverTextoNoArquivo(mensagem);
    }

    private void EscreverTextoNoArquivo(string mensagem)
    {
        string caminhoArquivoLog = @"C:\Users\TALITA DIAS\Documents\Cursos\Web API ASP .NET Core\log.txt";

        using (StreamWriter sw = new StreamWriter(caminhoArquivoLog, true))
        {
            try
            {
                sw.WriteLine(mensagem);
                sw.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}