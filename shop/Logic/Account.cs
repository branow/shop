

namespace Shop.Logic
{
    class Account
    {
        public int Id { get; }
        public string Name
        {
            get { return _name; }
            set
            {
                if (_access) _name = value;
                else throw AccessError();
            }
        }
        public string Email
        {
            get { return _email; }
            set
            {
                if (_access) _email = value;
                else throw AccessError();
            }
        }
        public string Password
        {
            get { return _password; }
            set
            {
                if (_access) _password = value;
                else throw AccessError();
            }
        }
        public decimal Balance { get { return _balance; } }
        public List<string> History { get { return _history; } }
        public bool IsAccess { get { return _access; } }
        public List<Product> Basket { get; set; }

        private bool _access;
        private decimal _balance;
        private string _name;
        private string _email;
        private string _password;
        private readonly List<string> _history;

        public Account(int id, string name, string email, string password, bool access)
        {
            Id = id;
            _name = name;
            _email = email;
            _password = password;
            _balance = 0;
            _history = new List<string>();
            Basket = new List<Product>();
            _access = access;
        }

        public Account(int id, string name, string email, string password, decimal balance, List<string> history)
        {
            Id = id;
            Basket = new List<Product>();
            _name = name;
            _email = email;
            _password = password;
            _balance = balance;
            _history = history;
        }

        public Account(int id, string name, string email, string password, decimal balance, List<string> history, bool access)
        {
            Id = id;
            Basket = new List<Product>();
            _name = name;
            _email = email;
            _password = password;
            _balance = balance;
            _history = history;
            _access = access;
        }

        public void ExecuteTransaction(Transaction transaction)
        {
            if (transaction.IsReceiver(this))
            {
                _balance += transaction.Sum;
                History.Add(transaction.ToString());
            }
            else if (transaction.IsSender(this))
            {
                if (!_access)
                    throw AccessError();
                if (_balance < transaction.Sum)
                    throw new ArgumentOutOfRangeException("The balance is not enough money for this operation");
                _balance -= transaction.Sum;
                History.Add(transaction.ToString());
            }
        }

        public bool Open(string password)
        {
            if (IsAccess) return true;
            if (!Hash(password).Equals(Password)) return false;
            _access = true;
            Password = password;
            Email = Decode(Email, password);
            return _access;
        }

        public bool Close()
        {
            if (!IsAccess) return true;
            Email = Code(Email, Password);
            Password = Hash(Password);
            _access = false;
            return true;
        }

        public override string ToString()
        {
            if (_access)
            {
                return $"{Id}, {Name}, {Email}, {Balance}, {string.Join("; ", History)}";
            }
            else
            {
                return $"{Id}, {Name}";
            }
        }

        public override bool Equals(object? obj)
        {
            var o = obj as Account;
            if (o == null)
                return false;
            return Id == o.Id;
        }

        private string Hash(string key)
        {
            string hash = "";
            foreach (char c in key.ToCharArray())
            {
                hash += c * 11;
            }
            return hash;
        }

        private string Code(string value, string key)
        {
            string code = "";
            foreach (char val in value.ToCharArray())
            {
                int valI = val;
                foreach (char k in key.ToCharArray())
                {
                    int kI = k;
                    if (kI % 2 == 0)
                    {
                        valI += kI / 10;
                    }
                    else
                    {
                        valI -= kI / 10;
                    }
                }
                code += valI + " ";
            }
            return code;
        }

        private string Decode(string code, string key)
        {
            string value = "";
            foreach (string cod in code.Split(" "))
            {
                if (cod.Equals(""))
                    continue;
                int codI = int.Parse(cod);
                foreach (char k in key.ToCharArray())
                {
                    int kI = k;
                    if (kI % 2 == 0)
                    {
                        codI -= kI / 10;
                    }
                    else
                    {
                        codI += kI / 10;
                    }
                }
                value += (char)codI;
            }
            return value;
        }

        private AccessViolationException AccessError()
        {
            return new AccessViolationException("Firstly you have to get access for this account");
        }

    }
}
