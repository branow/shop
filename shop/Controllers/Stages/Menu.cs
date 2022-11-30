using Shop.Logic;
using Shop.Logic.Data.Tables;
using System.Text.RegularExpressions;

namespace Shop.Controllers.Stages
{
    class Menu : Controller
    {
        private static readonly string s_name = "menu";
        private readonly string _help = "help", _goods = "goods", _entrance = "ent", _registration = "reg", _cabinate = "cab",
            _buy = "buy", _toBasket = "to bsk", _seeProduct = "see";
        
        public Menu() : base(s_name) { }

        protected override Dictionary<string, Action<Session>> GetMethodsDictionary()
        {
            List<int> l = new List<int>();
            Dictionary<string, Action<Session>> dic = new Dictionary<string, Action<Session>>();
            dic.Add(_goods, Goods);
            dic.Add(_help, Help);
            dic.Add(_cabinate, Cabinet);
            dic.Add(_entrance, Entrance);
            dic.Add(_registration, Registrate);
            dic.Add(_buy, Buy);
            dic.Add(_toBasket, ToBusket);
            dic.Add(_seeProduct, SeeProduct);
            return dic;
        }

        private void Goods(Session session) 
        {
            List<Product> goods = new ProductTable().Get();
            string respond = "";
            goods.ForEach(e => respond += e.GetShortInfo() + "\n");
            session.Respond = respond;
        }

        private void Help(Session session)
        {
            session.Respond = $"help - shows all possible commands\n" +
                            $"goods - shows list of all goods\n" +
                            $"seee - shows all information about product\n" +
                            $"buy - buys product\n" +
                            $"to bsk - picks up product to basket\n" +
                            $"ent - entrance to your account\n" +
                            $"reg - registrate new account\n" +
                            $"cab - goes to the your cabinet\n" +
                            $"exit - exit\n";
        }

        private void Cabinet(Session session)
        {
            if (session.Account == null || !session.Account.IsAccess) 
                session.Respond = "Firstly log in";

            session.Controller = new Cabinet();
            session.Respond = "You are in cabinet";
        }

        private void Entrance(Session session)
        {   
            string name, password;
            Account ac;
            while (true)
            {
                Console.Write("Type your username -> ");
                name = Console.ReadLine();
                ac = new AccountTable().Get(name);
                if (ac != null) break;
                Console.Write("Not found user with this name, try again");
            }
            while (true)
            {
                Console.Write("Type your password -> ");
                password = Console.ReadLine();
                if (ac.Open(password)) break;
                Console.Write("Not correct password, try again");
            }
            session.Controller = new Cabinet();
            session.Account = ac;
            session.Respond = "Entrance was successful";
        }

        private void Registrate(Session session)
        {
            string name = RegistrateName(), email = RegistrateEmail(), password = RegistratePassword();
            session.Account = new AccountTable().Create(name, email, password);
            session.Controller = new Cabinet();
            session.Respond = "Registration was successful";
        }

        private void Buy(Session session)
        {
            if (session.Account == null || !session.Account.IsAccess)
                session.Respond = "Firstly log in";
            Console.Write("Type id of product ->");
            int id = int.Parse(Console.ReadLine());
            ProductTable pt = new ProductTable();
            Product product = pt.Get(id);
            new Operation().Buy(session.Account, product);
            pt.Remove(id);
            session.Respond = $"You buy {product.GetShortInfo}";
        }

        private void SeeProduct(Session session)
        {
            Console.Write("Type id of product ->");
            int id = int.Parse(Console.ReadLine());
            ProductTable pt = new ProductTable();
            Product product = pt.Get(id);
            session.Respond = $"{product.GetAllInfo()}";
        }

        private void ToBusket(Session session)
        {
            if (session.Account == null || !session.Account.IsAccess)
                session.Respond = "Firstly log in";
            Console.Write("Type id of product ->");
            int id = int.Parse(Console.ReadLine());
            Product product = new ProductTable().Get(id);
            session.Account.Basket.Add(product);
            session.Respond = $"You get into busket {product.GetShortInfo()}";
        }

        private string RegistrateName()
        {
            string name;
            while (true)
            {
                Console.Write("Type your user name -> ");
                name = Console.ReadLine();
                if (name != null && !name.Equals(""))
                {
                    if (!new AccountTable().ExistName(name))
                    {
                        return name;
                    }
                    Console.Write("User with this name already exists ((");
                }
                Console.Write("Not correct name");
            }
        }

        private string RegistrateEmail()
        {
            string email;
            string regular = "@{1}";
            Regex regex = new Regex(regular);
            while (true)
            {
                Console.Write("Type your email -> ");
                email = Console.ReadLine();
                if (email != null && !email.Equals("") && regex.IsMatch(email))
                {
                    return email;
                }
                Console.Write("Not correct email");
            }
        }

        private string RegistratePassword()
        {
            string password;
            while (true)
            {
                Console.Write("Type your password -> ");
                password = Console.ReadLine();
                if (password != null && !password.Equals(""))
                {
                    Console.Write("Type your password again -> ");
                    string again = Console.ReadLine();
                    if (password.Equals(again))
                        return password;
                    Console.Write("You type two different passwords");
                }
                Console.Write("Not correct password");
            }
        }

        

        
    }
}
