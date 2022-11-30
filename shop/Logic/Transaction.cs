using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Logic
{
    class Transaction
    {
        public static Transaction newTransaction(decimal sum, Account receiver, Account sender, string message = "")
        {
            return new Transaction(DateTime.Now.Ticks, sum, receiver, sender, message);
        }

        public long Id { get; }
        public decimal Sum { get; }
        public string Message { get; }
        public Account Receiver { get; }
        public Account Sender { get; }
        private bool _executed;

        public Transaction(long id, decimal sum, Account receiver, Account sender, string message = "")
        {
            Id = id;
            Sum = sum;
            Receiver = receiver;
            Sender = sender;
            Message = message;
        }

        public void Execute()
        {
            if (_executed)
                throw new AccessViolationException("This transaction has alredy been executed");
            Sender.ExecuteTransaction(this);
            Receiver.ExecuteTransaction(this);
            _executed = true;
        }

        public bool IsReceiver(Account account) 
        {
            return Receiver.Equals(account);
        }

        public bool IsSender(Account account)
        {
            return Sender.Equals(account);
        }

        public bool IsExecuted() 
        {
            return _executed;
        } 

        public override string ToString()
        {
            return $"{Id} sum: {Sum/100} reciever: {Receiver.Name} sender: {Sender.Name} mes: {Message}";
        }

    }
}
