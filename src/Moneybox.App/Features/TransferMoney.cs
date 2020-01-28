using Moneybox.App.DataAccess;
using Moneybox.App.Domain.Services;
using System;

namespace Moneybox.App.Features
{
    public class TransferMoney
    {
        private IAccountRepository accountRepository;
        private INotificationService notificationService;

        public TransferMoney(IAccountRepository accountRepository, INotificationService notificationService)
        {
            this.accountRepository = accountRepository;
            this.notificationService = notificationService;
        }

        public void Execute(Guid fromAccountId, Guid toAccountId, decimal amount)
        {
            var from = this.accountRepository.GetAccountById(fromAccountId);
            var to = this.accountRepository.GetAccountById(toAccountId);

            //Some validation to ensure both accounts are found - ideally this would be ensured through the accountrepository?
            if (from == null)
            {
                throw new InvalidOperationException("Cannot find account with the id: " + fromAccountId.ToString());
            }
            if (to == null)
            {
                throw new InvalidOperationException("Cannot find account with the id: " + toAccountId.ToString());
            }

            Account.TransferMoney(from, to, amount);

            //Would this ideally be in a 'try and catch'?
            this.accountRepository.Update(from);
            this.accountRepository.Update(to);

            //Push notification happens after update occurs - ideal order in case update fails
            if (Account.PayInLimit - to.PaidIn < 500m)
            {
                this.notificationService.NotifyApproachingPayInLimit(to.User.Email);
            }
            if (from.Balance < 500m)
            {
                this.notificationService.NotifyFundsLow(from.User.Email);
            }
        }
    }
}
