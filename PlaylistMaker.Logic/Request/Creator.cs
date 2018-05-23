using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlaylistMaker.Logic.Commands;


namespace PlaylistMaker.Logic.Request
{
    public class Creator
    {
        private string _result;
        private ICommand[] commands;

        public Creator()
        {
            commands = new ICommand[]
            {
                new Addition(),
                //new 
            };
        }

        public string GetResult()
        {
            return _result;
        }

        public bool Execute()
        {
            var command = Console.ReadLine();
            return commands.Any(x => x.GetName() == command);
        }
    }
}
