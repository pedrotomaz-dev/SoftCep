using SoftCep.Domain.Exceptions;

namespace SoftCep.Api.Extensions;

public static class ExceptionHandlingExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandling(this IApplicationBuilder app, ILogger logger)
    {
        app.Use(async (context, next) =>
        {
            try
            {
                await next();
            }
            catch (ExternalServiceTimeoutException ex)
            {
                logger.LogError(ex, "Timeout ao acessar serviço externo.");
                context.Response.StatusCode = StatusCodes.Status504GatewayTimeout;
                await context.Response.WriteAsJsonAsync(new { error = ex.Message });
            }
            catch (ExternalServiceUnavailableException ex)
            {
                logger.LogError(ex, "Serviço externo indisponível.");
                context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                await context.Response.WriteAsJsonAsync(new { error = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                logger.LogError(ex, "Erro genérico de integração externa.");
                context.Response.StatusCode = StatusCodes.Status502BadGateway;
                await context.Response.WriteAsJsonAsync(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro interno inesperado.");
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(new { error = ex.Message ?? "Erro interno do servidor." });
            }
        });

        return app;
    }
}
