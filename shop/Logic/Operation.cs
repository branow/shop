using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop.Logic.Data.Tables;

namespace Shop.Logic
{
    class Operation
    {

        public void Buy(Account buyer, Product product)
        {
            AccountTable at = new AccountTable();
            Account seller = at.Get(product.SellerId);
            Transaction transaction = Transaction.newTransaction(product.Price, seller, buyer, $"For {product.Id} {product.Name}");
            transaction.Execute();
            at.Update(buyer);
            at.Update(seller);
            new ProductTable().Remove(product.Id);
        }

        public void Replenishment(Account receiver, decimal sum)
        {
            AccountTable at = new AccountTable();
            Account bank = new Account(-666, "bank", "****", "****", sum, new List<string>(), true);
            Transaction transaction = Transaction.newTransaction(sum, receiver, bank, "Account replenishment");
            transaction.Execute();
            at.Update(receiver);
        }


    }
}
