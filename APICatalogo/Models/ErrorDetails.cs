using System.Text.Json;

namespace APICatalogo.Models;

public class ErrorDetails
{
    public int StatusCode { get; set; }
    public string? Message { get; set; }
    public string? Trace { get; set; }

    override public string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}