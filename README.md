# Matt Parker - Moneybox Money Withdrawal

Here's a few things I'd like to highlight;

* As the task asked, I've devloped the model so that it's now functional and not just a container for variables.
* I decided to create both a Deposit and a Withdraw function and have the Transfer function call those two. My reasoning being that in future the way Depositing and Withdrawing may change, but you shouldn't need to alter the Transfer function if that happens (assuming no new args are required)
* I pushed notification calls to the end of functions - as I figured you'd only want to push notifications once we're sure transactions have taken place.
* Speaking of transactions - I assumed that the AccountRepository.update() function would throw errors if needed and decided not to place it inside a 'try and catch'. However, I've placed some comments to show I gave this consideration.
* If I had more time, I'd be sure to implement further unit tests. As it is, I decided to cover what I felt to be the main functions of the re-factored Account class.

Thanks for your time. :v: