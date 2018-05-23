using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaylistMaker.Logic.Commands
{
    public class Addition : ICommand
    {
        private const string Name = "add";

        public void Execute(string path)
        {

        }

        public string GetName()
        {
            return Name;
        }
    }
}
