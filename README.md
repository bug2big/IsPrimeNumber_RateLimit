IsPrimeNumber_RateLimit

Test task.
You should implement policy to limit request to call IsPrimeNumber API.
Restriction: 10 call per user for 1 hour.
If someone (e.g. js client) calls API then for identify user you should pass IP and token (e.g. UUID). 