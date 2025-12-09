using System;
using System.Collections.Generic;


public class DatabaseManager
{
    private BankService _service;
    private string _loadedFilePath; 

    public BankService Service
    {
        get { return _service; }
        private set { _service = value; }
    }

    // загружает данные из excel и сохраняет путь к файлу
    public void LoadData(string filePath)
    {
        Service = ExcelHelper.LoadFromExcel(filePath);
        _loadedFilePath = filePath; 
    }

    // сохраняет service обратно в тот же excel-файл
    public void SaveData()
    {
        if (!string.IsNullOrEmpty(_loadedFilePath) && Service != null)
        {
            ExcelHelper.SaveToExcel(Service, _loadedFilePath);
        }
        else
        {
            throw new InvalidOperationException("Невозможно сохранить: данные не загружены или путь к файлу неизвестен.");
        }
    }

    // выводит все счета в консоль
    public void PrintAllAccounts()
    {
        if (Service != null)
        {
            foreach (var item in Service.Accounts)
            {
                Console.WriteLine(item);
            }
        }
        else
        {
            Console.WriteLine("Данные не загружены.");
        }
    }

    // выводит все валюты в консоль
    public void PrintAllCurrencies()
    {
        if (Service != null)
        {
            foreach (var item in Service.Currencies)
            {
                Console.WriteLine(item);
            }
        }
        else
        {
            Console.WriteLine("Данные не загружены.");
        }
    }

    // выводит все курсы валют в консоль
    public void PrintAllExchangeRates()
    {
        if (Service != null)
        {
            foreach (var item in Service.Rates)
            {
                Console.WriteLine(item);
            }
        }
        else
        {
            Console.WriteLine("Данные не загружены.");
        }
    }

    // выводит все начисления/транзакции в консоль
    public void PrintAllTransactions()
    {
        if (Service != null)
        {
            foreach (var item in Service.Transactions)
            {
                Console.WriteLine(item);
            }
        }
        else
        {
            Console.WriteLine("Данные не загружены.");
        }
    }

    // добавляет новый счёт в сервис если данные загружены
    public void AddAccount(Account acc)
    {
        if (Service != null)
        {
            Service.AddAccount(acc);
        }
        else
        {
            Console.WriteLine("Данные не загружены.");
        }
    }

    // удаляет счёт по id и возвращает true если удалено
    public bool RemoveAccount(int id)
    {
        if (Service != null)
        {
            return Service.RemoveAccount(id);
        }
        Console.WriteLine("Данные не загружены.");
        return false;
    }

    // возвращает счета, открытые в январе 2021
    public IEnumerable<Account> GetAccountsOpenedInJanuary2021()
    {
        if (Service != null)
        {
            return Service.GetAccountsOpenedInJanuary2021();
        }
        return new List<Account>();
    }

    // возвращает транзакции с суммой в рублях больше 4
    public IEnumerable<(Transaction, decimal)> GetLargeTransactionsInRub()
    {
        if (Service != null)
        {   
            return Service.GetLargeTransactionsInRub();
        }
        return new List<(Transaction, decimal)>();
    }

    // возвращает фио владельца с наибольшим суммарным доходом
    public string GetTopEarnerFIO()
    {
        if (Service != null)
        {
            return Service.GetTopEarnerFIO();
        }
        return "Не найдено";
    }

    // возвращает счета с отрицательным балансом на заданную дату
    public IEnumerable<(int, string, decimal)> GetNegativeBalancesOnDate(DateTime date)
    {
        if (Service != null)
        {
            return Service.GetNegativeBalancesOnDate(date);
        }
        return new List<(int, string, decimal)>();
    }
}