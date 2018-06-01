namespace PlaylistMaker.Logic.Commands
{
    internal interface ICommand
    {
        string ReadArgs();

        void Execute(string request, out string result);

        void Display(string result);

        string GetName();

        int GetArgsCount();

        bool IsDisplayable();
    }
}
