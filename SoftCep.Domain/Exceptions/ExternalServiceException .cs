namespace SoftCep.Domain.Exceptions;

public class ExternalServiceException : Exception
{
    public ExternalServiceException(string message, Exception? inner = null)
        : base(message, inner) { }
}

public class ExternalServiceTimeoutException : ExternalServiceException
{
    public ExternalServiceTimeoutException(string message, Exception? inner = null)
        : base(message, inner) { }
}

public class ExternalServiceUnavailableException : ExternalServiceException
{
    public ExternalServiceUnavailableException(string message, Exception? inner = null)
        : base(message, inner) { }
}
