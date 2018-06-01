using System;

namespace PlaylistMaker.Logic.Commands
{
    internal class ClientCloser : ICommand
    {
        private const string Name = "exit";
        public bool Displayability = false;
        private const int ArgsCount = 0;

        public void Execute(string path, out string result)
        {
            result = null;
            Environment.Exit(0);
        }

        public void Display(string result) { }

        public string ReadArgs()
        {
            return null;
        }

        public string GetName()
        {
            return Name;
        }

        public int GetArgsCount()
        {
            return ArgsCount;
        }

        public bool IsDisplayable()
        {
            return Displayability;
        }
    }
}
