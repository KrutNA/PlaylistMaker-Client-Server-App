namespace PlaylistMaker.Logic.Commands
{
    interface ICommand
    {
        string GetName();
        
        void Execute(string path);
    }
}
