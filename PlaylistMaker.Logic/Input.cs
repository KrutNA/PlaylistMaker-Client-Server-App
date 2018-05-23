using System;

namespace PlaylistMaker.Logic
{
    public class Input
    {
        public bool IsRunnig { get; set; } = true;
        public string Value { get; private set; }
        
        public void ConsoleRead()
        {
            do
            {
                Console.WriteLine("> ");
            } while (IsRunnig);
        }
    }
}
