namespace SharpOMatic.Server;

public class TriviaResponse
{
    public required string Answer { get; set; }
    public required string Reason { get; set; }
}

public class StringList
{
    public required string[] List { get; set; }
}

