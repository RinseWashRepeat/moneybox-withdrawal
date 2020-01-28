using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moneybox.App;
using System;

namespace MoneyBoxTesting
{
    [TestClass]
    public class AccountTests
    {
        [TestMethod]
        public void Withdraw_WithValid()
        {
            //Setup test
            decimal startingAmount = 10;
            decimal startingWithdrawn = 2;
            decimal withdrawAmount = 5m;
            decimal expectedFinalAmount = 5m;
            decimal expectedWithdrawnAmount = 7m;
            Account account = new Account();
            account.setupForTesting(startingAmount, startingWithdrawn);

            //Run test
            account.WithdrawMoney(withdrawAmount);

            //Check results
            decimal finalAmount = account.Balance;
            Assert.AreEqual(expectedFinalAmount, finalAmount, "Withdrawn error - expected result not correct.");
            Assert.AreEqual(expectedWithdrawnAmount, account.Withdrawn, "Withdrawn error - Withdrawn expected result not correct.");

            //Ensure multiple calls handled
            account.WithdrawMoney(withdrawAmount);
            Assert.AreEqual((expectedWithdrawnAmount+withdrawAmount), account.Withdrawn, "Withdrawn error - Withdrawn expected second result not correct.");
        }

        [TestMethod]
        public void Withdraw_WithInvalid()
        {
            //Setup test
            decimal startingAmount = 10;
            decimal withdrawAmount = -5.50m;
            
            Account account = new Account();
            account.setupForTesting(startingAmount);

            //Run test & check
            try
            {
                account.WithdrawMoney(withdrawAmount);
                //Manually assert to fail, as above should throw an error...
                Assert.Fail("Withdrawn error - negative value did not throw exception.");
            }
            catch(Exception ex)
            {
                Assert.IsTrue(ex is InvalidOperationException, "Withdrawn error - negative value did not throw correct exception type.");
            }
        }

        [TestMethod]
        public void Withdraw_TooMuch()
        {
            //Setup test
            decimal startingAmount = 10;
            decimal withdrawAmount = 10.1m;
            Account account = new Account();
            account.setupForTesting(startingAmount);

            //Run test & check
            try
            {
                account.WithdrawMoney(withdrawAmount);
                //Manually assert to fail, as above should throw an error...
                Assert.Fail("Withdrawn error - going overdrawn did not throw exception.");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is InvalidOperationException, "Withdrawn error - going overdrawn did not throw correct exception type.");
            }

        }

        [TestMethod]
        public void Deposit_WithValid()
        {
            //Setup test
            decimal startingAmount = 10;
            decimal startingWithdrawn = 0;
            decimal startingPaidIn = 2; 
            decimal depositAmount = 5m;
            decimal expectedFinalAmount = 15m;
            decimal expectedPaidInAmount = 7m;
            Account account = new Account();
            account.setupForTesting(startingAmount, startingWithdrawn, startingPaidIn);

            //Run test
            account.DepositMoney(depositAmount);

            //Check results
            decimal finalAmount = account.Balance;
            Assert.AreEqual(expectedFinalAmount, finalAmount, "Deposit error - Deposit expected result not correct.");
            Assert.AreEqual(expectedPaidInAmount, account.PaidIn, "Deposit error - PaidIn expected result not correct.");

            //Ensure multiple calls handled
            account.DepositMoney(depositAmount);
            Assert.AreEqual((expectedPaidInAmount + depositAmount), account.PaidIn, "Deposit error - PaidIn expected second result not correct.");
        }

        [TestMethod]
        public void Deposit_WithInvalid()
        {
            //Setup test
            decimal startingAmount = 10;
            decimal depositAmount = -5.50m;

            Account account = new Account();
            account.setupForTesting(startingAmount);

            //Run test & check
            try
            {
                account.DepositMoney(depositAmount);
                //Manually assert to fail, as above should throw an error...
                Assert.Fail("Deposit error - negative value did not throw exception.");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is InvalidOperationException, "Deposit error - negative value did not throw correct exception type.");
            }
        }

        [TestMethod]
        public void Deposit_TooMuch()
        {
            //Setup test
            decimal startingAmount = 1;
            decimal startingPaidIn = 2m;
            decimal DepositAmount = Account.PayInLimit + 0.01m;
            Account account = new Account();
            account.setupForTesting(startingAmount, 0, startingPaidIn);

            //Run test & check
            try
            {
                account.DepositMoney(DepositAmount);
                //Manually assert to fail, as above should throw an error...
                Assert.Fail("Deposit error - paying in too much did not throw exception.");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is InvalidOperationException, "Deposit error - paying in too much did not throw correct exception type.");
            }

            Assert.AreEqual(startingPaidIn, account.PaidIn, "Deposit error - Paid in amount is wrong as paidIn should not have changed");

        }

    }
}
