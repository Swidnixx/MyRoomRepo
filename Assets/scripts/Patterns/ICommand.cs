using System;

public interface ICommand
{
    public string title { get; }
    public string msg { get; }
    public Func<string> command { get; }
}