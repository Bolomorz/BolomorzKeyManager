namespace BolomorzKeyManager.Controller.Data;

internal class Message(bool success, string error)
{
    internal bool Success { get; init; } = success;
    internal string Error { get; init; } = error;

    internal static Message FailedToCreateDatabase = new(false, "failed to create database.");
    internal static Message Successful = new(true, "performing operation successful.");
    internal static Message WrongCredentials = new(false, $"wrong credentials entered.");
    internal static Message UnauthorizedAccess = new(false, "unauthorized access prohibited.");

    internal static Message ErrorThrown(string error, string source)
        => new(false, $"following error has been thrown in [{source}]:\n{error}");

    internal static Message Duplicate(string key, string value)
        => new(false, $"duplicate {key}: {value} found, no duplicates allowed.");

    internal static Message NotFound(string type, string key, string value)
        => new(false, $"{type} with {key}={value} not found.");
}

internal class ReturnDialog(Message message)
{
    internal Message Message { get; init; } = message;
}

internal class ReturnDialog<T>(Message message, T? value)
{
    internal Message Message { get; init; } = message;
    internal T? ReturnValue { get; init; } = value;
}