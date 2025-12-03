using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ApiCatalogo.Filters;

public class ApiExceptionFilter : IExceptionFilter
{
    
    private readonly ILogger<ApiExceptionFilter> _logger;

    public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger)
    {
        _logger = logger;
    }

    public void OnException(ExceptionContext context)// É chamada automaticamente, quando aparece uma exceção nao tratada
    {
        _logger.LogError(context.Exception, "Ocorreu uma exceção nao tratada: Status Code 500");//Estou logando a exceção, durante o processamento do requeset
        
        // O context, contem a informação da exceção
        //A exceção vai ter essa resposta
        context.Result = new ObjectResult("Ocorreu um problema ao tratar a sua solicittação: Status Code 500")
        {
            StatusCode = StatusCodes.Status500InternalServerError 
        };
    }
}