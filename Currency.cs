public class Currency
{
    private int _currencyID;
    private string _name;

    public int CurrencyID
    {
        get { return _currencyID; }
        set { _currencyID = value; }
    }

    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }

    public Currency(int id, string name)
    {
        _currencyID = id;
        _name = name;
    }

    public override string ToString()
    {
        return $"Валюта ID: {CurrencyID}, Название: {Name}";
    }
}