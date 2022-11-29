using System;

namespace Project.Runtime.Utils.Debug
{
    public class DebugCommand : DebugCommandBase
    {
        private readonly Func<bool> action;

        public DebugCommand(string id, string description, string format, Func<bool> action) : base(id, description, format)
        {
            this.action = action;
        }

        public bool Invoke()
        {
            return action?.Invoke() ?? false;
        }
    }
}