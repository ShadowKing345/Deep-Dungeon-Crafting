using System;
using System.Collections.Generic;
using System.Linq;

namespace Project.Runtime.Console
{
    public class ConsoleController
    {
        private readonly Dictionary<string, IConsoleCommand> commands;

        public ConsoleController()
        {
            commands = new Dictionary<string, IConsoleCommand>();
        }

        public void AddCommand(IConsoleCommand command)
        {
            if (commands.ContainsKey(command.Name))
            {
                throw new Exception("Key already contained!");
            }

            commands.Add(command.Name, command);
        }

        public bool ProcessCommand(string inputText)
        {
            if (string.IsNullOrEmpty(inputText.Trim()))
            {
                return false;
            }

            var split = inputText.Split(" ").ToList();
            var name = split.First();
            split.RemoveAt(0);

            foreach (var command in commands.Values)
            {
                if (!command.Name.Equals(name, StringComparison.InvariantCulture))
                {
                    continue;
                }

                if (command.Invoke(split.ToArray()))
                {
                    return true;
                }
            }

            return false;
        }
    }
}