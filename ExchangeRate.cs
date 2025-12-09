using System;

public class ExchangeRate
{
    private int _currencyID;
    private DateTime _date;
    private decimal _rate;

    public int CurrencyID
    {
        get { return _currencyID; }
        set { _currencyID = value; }
    }

    public DateTime Date
    {
        get { return _date; }
        set { _date = value; }
    }

    public decimal Rate
    {
        get { return _rate; }
        set { _rate = value; }
    }

    public ExchangeRate(int currencyId, DateTime date, decimal rate)
    {
        _currencyID = currencyId;
        _date = date;
        _rate = rate;
    }

    public override string ToString()
    {
        return $"Курс валюты {CurrencyID} на {Date:dd.MM.yyyy}: {Rate:F4} руб.";
    }
}