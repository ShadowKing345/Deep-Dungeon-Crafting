namespace Project.Runtime.Utils.Debug
{
    public abstract class DebugCommandBase
    {
        public string Id { get; }
        public string Format { get; }
        public string Description { get; }

        protected DebugCommandBase(string id, string description, string format)
        {
            Id = id;
            Description = description;
            Format = format;
        }
    }
}