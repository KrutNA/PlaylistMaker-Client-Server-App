using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;

namespace PlaylistMaker.Logic.Request
{
    public class Executer
    {
        private string _request;
        private string _result;
        private bool _isNotExecuted = true;

        public Executer(System.Net.HttpListenerRequest request)
        {
            var reader = new System.IO.StreamReader(request.InputStream);
            _request = reader.ReadToEnd();
        }
        
        public string GetResult()
        {
            return _isNotExecuted ? null : _result;
        }

        /// <summary>
        /// Server always gets correct request and it's always executes
        /// </summary>
        public void Execute()
        {
            
        }
    }
}
