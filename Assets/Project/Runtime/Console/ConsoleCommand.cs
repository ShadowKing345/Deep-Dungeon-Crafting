using System;

namespace Project.Runtime.Console
{
    public class ConsoleCommand : IConsoleCommand
    {
        public string Name { get; }
        public string Format { get; }
        public string Description { get; }

        private Func<string[], bool> action;

        public ConsoleCommand(string name, Func<string[], bool> action)
        {
            Name = name;
            this.action = action;
        }

        public ConsoleCommand(string name, string description,Func<string[], bool> action) : this(name, action)
        {
            Description = description;
        }

        public ConsoleCommand(string name, string description, string format,Func<string[], bool> action) : this(name, description, action)
        {
            Format = format;
        }

        public bool Invoke(params string[] args)
        {
            return action?.Invoke(args) ?? false;
        }
    }
}