using Shop.Logic.Data.Tables;
using Shop.Logic;


namespace Shop.Controllers.Stages
{
    class Cabinet : Controller
    {

        private static readonly string s_controller = "account";
        private readonly string _info = "info", _help = "help", _buy = "buyb", _transaction = "tran", _balance = "balance",
            _basket = "basket", _rename = "rename", _delete = "delete", _menu = "menu", _replenishment = "repl", 
            _create = "sell", _exitFromAccount = "ext";

        public Cabinet() : base(s_controller) { }

        protected override Dictionary<string, Action<Session>> GetMethodsDictionary()
        {
            Dictionary<string, Action<Session>> dic = new Dictionary<string, Action<Session>>();
            dic.Add(_info, Info);
            dic.Add(_help, Help);
            dic.Add(_buy, Buy);
            dic.Add(_transaction, Transaction);
            dic.Add(_balance, Balance);
            dic.Add(_basket, Basket);
            dic.Add(_rename, Rename);
            dic.Add(_delete, Delete);
            dic.Add(_menu, Menu);
            dic.Add(_replenishment, Replanishment);
            dic.Add(_create, CreateProduct);
            dic.Add(_exitFromAccount, Ext);
            return dic;
        }

        private void Info(Session session)
        {
            session.Respond = session.Account.ToString();
        }

        private void Help(Session session)
        {
            session.Respond = $"help - shows all possible commands\n" +
                            $"info - shows information about account\n" +
                            $"tran - shows list of all your transactions\n" +
                            $"buyb - buys all product from your basket\n" +
                            $"repl - makes account replenishment\n" +
                            $"balance - shows your balance\n" +
                            $"basket - shows list of choosen goods\n" +
                            $"rename - renames your account\n" +
                            $"delete - deletes your account\n" +
                            $"menu - goes to menu\n" +
                            $"ext - exit from account\n";

        }

        private void CreateProduct(Session session)
        {
            string name, description;
            decimal price;
            int categoryId;
            Console.Write("Type name of product -> ");
            name = Console.ReadLine();
            Console.Write("Type category number " + GetCategories() + " -> ");
            categoryId = int.Parse(Console.ReadLine());
            Console.Write("Type price of product int grn -> ");
            price = decimal.Parse(Console.ReadLine()) * 100;
            Console.Write("Type desctiption of product -> ");
            description = Console.ReadLine();
            if (name == null || categoryId <= 0 || categoryId > Product.Categories.Count)
            {
                session.Respond = "Incorect data";
                return;
            }
            Product product = new ProductTable().Create(name, price, categoryId, session.Account.Id, description != null ? description : "");
            session.Respond = $"You product {product.GetShortInfo()} was created succesful";
        }

        private void Transaction(Session session)
        {
            session.Respond = string.Join("\n", session.Account.History);
        }

        private void Balance(Session session)
        {
             session.Respond = $"balance = {session.Account.Balance / 100}";
        }

        private void Basket(Session session)
        {
            session.Respond = string.Join("\n", session.Account.Basket);
        }

        private void Rename(Session session)
        {
            string name;
            while (true)
            {
                Console.Write("Type your new user name -> ");
                name = Console.ReadLine();
                if (name != null && !name.Equals(""))
                {
                    if (!new AccountTable().ExistName(name))
                        break;
                    Console.Write("User with this name already exists ((");
                }
                Console.Write("Not correct name");
            }
            session.Account.Name = name;
            new AccountTable().Update(session.Account);
            session.Respond = "renaming was successful";
        }

        private void Delete(Session session)
        {
            while (true)
            {
                Console.Write("Type your password -> ");
                string password = Console.ReadLine();
                if (password != null && !password.Equals("") && session.Account.Password.Equals(password))
                {
                    break;
                }
                Console.Write("Not correct password, try again");
            }
            new AccountTable().Remove(session.Account.Id);
            session.Account = null;
            session.Controller = new Menu();
            session.Respond = "removing was successful";
        }

        private void Buy(Session session)
        {
            List<Product> basket = session.Account.Basket;
            if (basket.Count == 0)
            {
                session.Respond = "your basket is empty";
                return;
            }
            decimal sum = 0;
            basket.ForEach(e => sum += e.Price);

            if (session.Account.Balance < sum)
            {
                session.Respond = "there are not enough funds in your account";
                return;
            }
            Operation o = new Operation();
            basket.ForEach(e => o.Buy(session.Account, e));
            session.Respond = "The purchase was successful";
        }

        private void Replanishment(Session session)
        {
            Console.Write("Type number of bank card -> ");
            Console.ReadLine();
            Console.Write("Type amount of money -> ");
            decimal sum = decimal.Parse(Console.ReadLine()) * 100;
            new Operation().Replenishment(session.Account, sum);
            session.Respond = "replanishment was successful";
        }

        private void Menu(Session session)
        {
            session.Controller = new Menu();
            session.Respond = "You in menu";
        }

        private void Ext(Session session)
        {
            session.Account = null;
            session.Controller = new Menu();
            session.Respond = "You exit from your account";
        }

        private string GetCategories()
        {
            string res = "";
            List<string> list = Product.Categories;
            for (int i = 0; i < list.Count; i++)
            {
                res += $" {i + 1} - {list[i]}, ";
            }
            return res;
        }

        
    }
}
