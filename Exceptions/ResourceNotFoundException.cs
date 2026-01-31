namespace PlataformaMarcenaria.API.Exceptions;

public class ResourceNotFoundException : Exception
{
    public ResourceNotFoundException(string message) : base(message)
    {
    }

    public ResourceNotFoundException(string resource, string field, object value) 
        : base($"{resource} não encontrado com {field}: '{value}'")
    {
    }
}

