using System;

public class Account
{
    private int _id;
    private string _fio;
    private DateTime _openDate;

    public int ID
    {
        get { return _id; }
        set { _id = value; }
    }

    public string FIO
    {
        get { return _fio; }
        set { _fio = value; }
    }

    public DateTime OpenDate
    {
        get { return _openDate; }
        set { _openDate = value; }
    }

    public Account(int id, string fio, DateTime openDate)
    {
        _id = id;
        _fio = fio;
        _openDate = openDate;
    }

    public override string ToString()
    {
        return $"Счёт ID: {ID}, Владелец: {FIO}, Открыт: {OpenDate:dd.MM.yyyy}";
    }
}