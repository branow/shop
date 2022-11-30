using Shop.Logic.Data.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Logic
{
    
    class Product
    {
        public static readonly List<string> Categories = new CategoryTable().Get();
        public int Id { get; }
        private decimal _price;
        public decimal Price
        {
            get { return _price; }
            set
            {
                if (value < 0) throw new InvalidDataException("price have to be > 0");
                else _price = value;
            }
        }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public string Description { get; set; }
        public int SellerId { get; }

        public Product(int id, string name, decimal price, int categoryId, string description, int sellerId)
        {
            Id = id;
            Price = price;
            Name = name;
            Description = description;
            SellerId = sellerId;
            CategoryId = categoryId;
        }

        public Product(int id, string name, decimal price, int categoryId, int sellerId)
        {
            Id = id;
            Price = price;
            Name = name;
            Description = "";
            SellerId = sellerId;
            CategoryId = categoryId;
        }


        public string GetShortInfo() 
        {
            return $"{Id} -- {Name} -- {Price/100} -- {Categories[CategoryId-1]}";
        }

        public string GetAllInfo()
        {
            return $"{Id} -- {Name} -- {Price / 100} -- {Categories[CategoryId - 1]}\n{Description}" +
                $"\n seller - {new AccountTable().Get(SellerId).Name}";
        }
        public override string ToString()
        {
            return $"{Id} {Categories[CategoryId - 1]} {Name} {Price/100} ({Description}) {SellerId}";
        }

        public override bool Equals(object? obj)
        {
            var o = obj as Product;
            if (o == null)
                return false;
            return Id == o.Id;
        }


    }



}
