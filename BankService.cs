using System;
using System.Collections.Generic;
using System.Linq;

public class BankService
{
    private List<Account> _accounts;
    private List<Currency> _currencies;
    private List<ExchangeRate> _rates;
    private List<Transaction> _transactions;

    public List<Account> Accounts
    {
        get { return _accounts; }
        set { _accounts = value; }
    }

    public List<Currency> Currencies
    {
        get { return _currencies; }
        set { _currencies = value; }
    }

    public List<ExchangeRate> Rates
    {
        get { return _rates; }
        set { _rates = value; }
    }

    public List<Transaction> Transactions
    {
        get { return _transactions; }
        set { _transactions = value; }
    }

    public BankService()
    {
        _accounts = new List<Account>();
        _currencies = new List<Currency>();
        _rates = new List<ExchangeRate>();
        _transactions = new List<Transaction>();
    }

    // 1. Запрос к одной таблице список всех счетов, открытых в январе 2021
    public IEnumerable<Account> GetAccountsOpenedInJanuary2021()
    {
        foreach (Account account in Accounts)
        {
            if (account.OpenDate.Month == 1 && account.OpenDate.Year == 2021)
            {
                yield return account;
            }
        }
    }

    // 2. Запрос к двум таблицам — все транзакции с суммой > 4 в рублёвом эквиваленте (на основе курса)
    public IEnumerable<(Transaction, decimal RubAmount)> GetLargeTransactionsInRub()
    {
        var query = from t in Transactions
                    join r in Rates on new { t.CurrencyID, t.Date } equals new { r.CurrencyID, r.Date }
                    let rub = t.Amount * r.Rate
                    where rub > 4m
                    select (t, rub);
        return query.ToList();
    }

    // 3. Запрос к трём таблицам владелец счёта с максимальной суммой начислений в рублях
    public string GetTopEarnerFIO()
    {
        var result = (from t in Transactions
                      join r in Rates on new { t.CurrencyID, t.Date } equals new { r.CurrencyID, r.Date }
                      join a in Accounts on t.AccountID equals a.ID
                      group t.Amount * r.Rate by a into g
                      orderby g.Sum() descending
                      select g.Key.FIO).FirstOrDefault();

        return result ?? "Не найдено";
    }

    // 4. Запрос к трём таблицам список счетов с отрицательным балансом на 26.12.2021
    public IEnumerable<(int AccountID, string FIO, decimal Balance)> GetNegativeBalancesOnDate(DateTime date)
    {
        var balances = from t in Transactions
                       where t.Date == date
                       join r in Rates on new { t.CurrencyID, t.Date } equals new { r.CurrencyID, r.Date }
                       group t.Amount * r.Rate by t.AccountID into g
                       where g.Sum() < 0
                       select new { AccountID = g.Key, Balance = g.Sum() };

        var full = from b in balances
                   join a in Accounts on b.AccountID equals a.ID
                   select (b.AccountID, a.FIO, b.Balance);

        return full.ToList();
    }

    public void AddAccount(Account acc)
    {
        foreach (Account existingAccount in Accounts)
        {
            if (existingAccount.ID == acc.ID)
            {
                throw new InvalidOperationException("Счёт с таким ID уже существует.");
            }
        }
        Accounts.Add(acc);
    }

    public bool RemoveAccount(int id)
    {
        int initialCount = Accounts.Count;
        for (int i = Accounts.Count - 1; i >= 0; i--)
        {
            if (Accounts[i].ID == id)
            {
                Accounts.RemoveAt(i);
            }
        }
        return Accounts.Count < initialCount;
    }

}