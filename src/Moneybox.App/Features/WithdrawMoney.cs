using Moneybox.App.DataAccess;
using Moneybox.App.Domain.Services;
using System;

namespace Moneybox.App.Features
{
    public class WithdrawMoney
    {
        private IAccountRepository accountRepository;
        private INotificationService notificationService;

        public WithdrawMoney(IAccountRepository accountRepository, INotificationService notificationService)
        {
            this.accountRepository = accountRepository;
            this.notificationService = notificationService;
        }

        public void Execute(Guid fromAccountId, decimal amount)
        {
            var from = this.accountRepository.GetAccountById(fromAccountId);

            //Some validation to ensure account is found - ideally this would be ensured through the accountrepository?
            if (from == null)
            {
                throw new InvalidOperationException("Cannot find account with the id: " + fromAccountId.ToString());
            }

            from.WithdrawMoney(amount);

            //Would this ideally be in a 'try and catch'?
            accountRepository.Update(from);

            //Push notification happens after update occurs - ideal order in case update fails
            if (from.Balance < 500m)
            {
                this.notificationService.NotifyFundsLow(from.User.Email);
            }
        }
    }
}
