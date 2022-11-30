using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Controllers
{
    abstract class Controller
    {

        public readonly string Name;
        protected readonly Dictionary<string, Action<Session>> methods;

        protected Controller(string name) 
        {
            methods = GetMethodsDictionary();
            Name = name;
        }

        public void Execute(string command, Session session) 
        {
            methods.TryGetValue(command, out var action);
            if (action != null)
                action(session);
            else
                Default(session);
        }

        protected virtual void Default(Session session) 
        {
            session.Respond = "Not correct command";
        }

        protected abstract Dictionary<string, Action<Session>> GetMethodsDictionary();

    }
}
