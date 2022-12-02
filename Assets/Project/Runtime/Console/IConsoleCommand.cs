namespace Project.Runtime.Console
{
    public interface IConsoleCommand
    {
        public string Name { get; }
        public string Format { get; }
        public string Description { get; }
        public bool Invoke(string[] args);
    }
}