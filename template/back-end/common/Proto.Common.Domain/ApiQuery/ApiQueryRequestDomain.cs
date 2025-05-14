namespace Proto.Common.Domain;

public class ApiQueryRequestDomain
{
    public int Page { get; set; } = 1;
    public int Size { get; set; } = 10;
    public string Order { get; set; } = string.Empty;
    public Dictionary<string, string> Filters { get; set; } = new();
}
