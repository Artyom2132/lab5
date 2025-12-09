using System;

public class Transaction
{
    private int _id;
    private int _accountId;
    private int _currencyId;
    private DateTime _date;
    private decimal _amount;

    public int ID
    {
        get { return _id; }
        set { _id = value; }
    }

    public int AccountID
    {
        get { return _accountId; }
        set { _accountId = value; }
    }

    public int CurrencyID
    {
        get { return _currencyId; }
        set { _currencyId = value; }
    }

    public DateTime Date
    {
        get { return _date; }
        set { _date = value; }
    }

    public decimal Amount
    {
        get { return _amount; }
        set { _amount = value; }
    }

    public Transaction(int id, int accountId, int currencyId, DateTime date, decimal amount)
    {
        _id = id;
        _accountId = accountId;
        _currencyId = currencyId;
        _date = date;
        _amount = amount;
    }

    public override string ToString()
    {
        return $"Операция ID: {ID}, Счёт: {AccountID}, Валюта: {CurrencyID}, Дата: {Date:dd.MM.yyyy}, Сумма: {Amount}";
    }
}