using System;
using Moneybox.App.Domain.Services;

namespace Moneybox.App
{
    public class Account
    {
        public const decimal PayInLimit = 4000m;

        /*I did wonder if you would have wanted a withdraw limit too?
        I decided against this as I couldn't edit the notification service, so assumed this wasn't needed. */
        
        //Whilst these fields need to be accessed outside of the class, I assumed it'd be safer to have their setter method private.
        public Guid Id { get; private set; }

        public User User { get; private set; }

        public decimal Balance { get; private set; }

        public decimal Withdrawn { get; private set; }

        public decimal PaidIn { get; private set; }

        public static void TransferMoney(Account fromAccount, Account toAccount, Decimal amount)
        {
            if(amount <= 0m)
            {
                throw new InvalidOperationException("Transfer amount needs to be more than 0.00");
            }
            fromAccount.WithdrawMoney(amount);
            toAccount.DepositMoney(amount);

            //Transfer can happen, so update balances
            toAccount.PaidIn = toAccount.PaidIn + amount;

        }

        //New to unit testing - decided to create a function to ensure I could set values
        public void setupForTesting(decimal initBalance, decimal initWithdrawn = 0, decimal initPaidIn = 0)
        {
            this.Balance = initBalance;
            this.Withdrawn = initWithdrawn;
            this.PaidIn = initPaidIn;
        }

        public void WithdrawMoney(Decimal amount)
        {
            if (amount <= 0m)
            {
                throw new InvalidOperationException("Withdraw amount needs to be more than 0.00");
            }
            //Check this account has sufficient funds
            if ((Balance - amount) < 0)
            {
                throw new InvalidOperationException("Insufficient funds to make transfer");
            }
            
            //withdraw can happen, so update balance
            Balance = Balance - amount;
            Withdrawn = Withdrawn + amount; //Even though withdrawn isn't used, I figured it'd be a good idea to keep it up to date
        }

        public void DepositMoney(Decimal amount)
        {
            if (amount <= 0m)
            {
                throw new InvalidOperationException("Deposit amount needs to be more than 0.00");
            }
            //Check to account won't break pay in limit
            if ((PaidIn + amount) > Account.PayInLimit)
            {
                throw new InvalidOperationException("Account pay in limit reached");
            }

            //deposit can happen, so update balance and paid in
            Balance = Balance + amount;
            PaidIn = PaidIn + amount;
            
        }

    }
}
