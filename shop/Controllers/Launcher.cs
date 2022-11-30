
using Shop.Controllers.Stages;
using Shop.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Controllers
{
    class Launcher
    {
        public Session Session { get; }

        public Launcher() 
        { 
            Session = new Session();
            Session.Controller = new Menu();
        }

        public void Start() 
        {
            Console.WriteLine("Glad to see you in our shop)");
            while (true)
            {   
                Console.Write($"shop.{Session.Controller.Name} -> ");
                string command = Console.ReadLine();

                if (command.Equals("exit"))
                    break;
                Session.Controller.Execute(command, Session);

                Console.WriteLine(Session.Respond);
            }  
        }       
    }

}
