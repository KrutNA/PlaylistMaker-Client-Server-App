using System;
using System.Linq;
using PlaylistMaker.Logic.Commands;
using PlaylistMaker.Logic.Model;
using PlaylistMaker.Logic.Stream;


namespace PlaylistMaker.Logic.Request
{
    public class Creator
    {
        private string _result;
        private ICommand[] commands;
        public bool NotCreated { get; private set; } = true;
        private ICommand command;

        public Creator()
        {
            commands = new ICommand[]
            {
                new ClientCloser(),
                new CompositionAdder(),
                new CompositionRemover(), 
                new CompositionSearchEngine(), 
                new CompositionsListDisplayer(), 
                //new CompositionsListDisplayerFull(),
                new PlaylistsListDisplayer(),
                new PMHelpDisplayer(), 
            };
        }

        public string GetResult()
        {
            return _result;
        }

        public bool Execute()
        {
            var input = new Input();
            var output = new Output();
            output.Execute("Input command: ");
            var commandName = input.Execute();
            var retrn = commands.Any(x => x.GetName() == commandName);
            if (retrn)
            {
                command = commands.FirstOrDefault(x => x.GetName() == commandName);
                if (commandName == "exit")
                    command.Execute(Environment.CurrentDirectory, out _result);
                else
                {
                    var requestCommand = new RequestCommand(command.GetName());
                    _result = requestCommand.Serialize();
                    var result = command.ReadArgs();
                    if (command.GetArgsCount() == 0) { }
                    else if (result == null)
                        retrn = false;
                    else
                        _result += "\n" + result;
                }
            }
            return retrn;
        }

        public void DisplayResponse(string response)
        {
            if (command.IsDisplayable())
                command.Display(response);
        }
    }
}
