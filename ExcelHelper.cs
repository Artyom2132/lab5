using Aspose.Cells;
using System;
using System.Globalization;
using System.Collections.Generic;

public static class ExcelHelper
{
    public static BankService LoadFromExcel(string filePath)
    {
        var service = new BankService();
        var workbook = new Workbook(filePath);
        Worksheet ws;

        // --- 1. Лист "Счета" ---
        ws = workbook.Worksheets["Счета"];
        if (ws == null) throw new InvalidOperationException("Лист 'Счета' не найден");
        var accounts = ws.Cells;
        for (int row = 1; row <= accounts.MaxDataRow; row++)
        {
            if (accounts[row, 0].Value == null) continue; // Пропустить пустые строки
            int id = Convert.ToInt32(accounts[row, 0].Value);
            string fio = accounts[row, 1].StringValue;
            DateTime date = DateTime.ParseExact(accounts[row, 2].StringValue, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            service.Accounts.Add(new Account(id, fio, date));
        }

        // --- 2. Лист "Валюты" ---
        ws = workbook.Worksheets["Валюты"];
        if (ws == null) throw new InvalidOperationException("Лист 'Валюты' не найден");
        var currencies = ws.Cells;
        for (int row = 1; row <= currencies.MaxDataRow; row++)
        {
            if (currencies[row, 0].Value == null) continue; // Пропустить пустые строки
            int id = Convert.ToInt32(currencies[row, 0].Value);
            string name = currencies[row, 1].StringValue;
            service.Currencies.Add(new Currency(id, name));
        }

        // --- 3. Лист "Курс валют" ---
        ws = workbook.Worksheets["Курсы валют"];
        if (ws == null) throw new InvalidOperationException("Лист 'Курсы валют' не найден ");
        var rates = ws.Cells;
        for (int row = 1; row <= rates.MaxDataRow; row++)
        {
            if (rates[row, 0].Value == null) continue; // Пропустить пустые строки
            int currId = Convert.ToInt32(rates[row, 1].Value); 
            DateTime date = DateTime.ParseExact(rates[row, 2].StringValue, "dd.MM.yyyy", CultureInfo.InvariantCulture); 
            decimal rate = Convert.ToDecimal(rates[row, 3].Value); 
            service.Rates.Add(new ExchangeRate(currId, date, rate));
        }

        // --- 4. Лист "Начисления" ---
        ws = workbook.Worksheets["Поступления"];
        if (ws == null) throw new InvalidOperationException("Лист 'Начисления' не найден");
        var trans = ws.Cells;
        for (int row = 1; row <= trans.MaxDataRow; row++)
        {
            if (trans[row, 0].Value == null) continue; // Пропустить пустые строки
            int id = Convert.ToInt32(trans[row, 0].Value); 
            int accId = Convert.ToInt32(trans[row, 1].Value); 
            int currId = Convert.ToInt32(trans[row, 2].Value); 
            DateTime date = DateTime.ParseExact(trans[row, 3].StringValue, "dd.MM.yyyy", CultureInfo.InvariantCulture); 
            decimal amount = Convert.ToDecimal(trans[row, 4].Value); 
            service.Transactions.Add(new Transaction(id, accId, currId, date, amount));
        }

        return service;
    }

    public static void SaveToExcel(BankService service, string filePath)
    {
        var workbook = new Workbook();
        workbook.Worksheets.Clear();

        // Счета
        var ws = workbook.Worksheets.Add("Счета");
        ws.Cells[0, 0].PutValue("ID");
        ws.Cells[0, 1].PutValue("ФИО");
        ws.Cells[0, 2].PutValue("Дата открытия");
        for (int i = 0; i < service.Accounts.Count; i++)
        {
            var acc = service.Accounts[i];
            ws.Cells[i + 1, 0].PutValue(acc.ID);
            ws.Cells[i + 1, 1].PutValue(acc.FIO);
            ws.Cells[i + 1, 2].PutValue(acc.OpenDate.ToString("dd.MM.yyyy"));
        }

        // Валюты
        ws = workbook.Worksheets.Add("Валюты");
        ws.Cells[0, 0].PutValue("ID");
        ws.Cells[0, 1].PutValue("Наименование");
        for (int i = 0; i < service.Currencies.Count; i++)
        {
            var c = service.Currencies[i];
            ws.Cells[i + 1, 0].PutValue(c.CurrencyID);
            ws.Cells[i + 1, 1].PutValue(c.Name);
        }

        // Курсы
        ws = workbook.Worksheets.Add("Курсы валют");
        ws.Cells[0, 0].PutValue("ID");
        ws.Cells[0, 1].PutValue("ID валюты");
        ws.Cells[0, 2].PutValue("Дата");
        ws.Cells[0, 3].PutValue("Курс");
        for (int i = 0; i < service.Rates.Count; i++)
        {
            var r = service.Rates[i];
            ws.Cells[i + 1, 0].PutValue(i + 1);
            ws.Cells[i + 1, 1].PutValue(r.CurrencyID);
            ws.Cells[i + 1, 2].PutValue(r.Date.ToString("dd.MM.yyyy"));
            ws.Cells[i + 1, 3].PutValue(r.Rate);
        }

        // Начисления
        ws = workbook.Worksheets.Add("Поступления");
        ws.Cells[0, 0].PutValue("ID");
        ws.Cells[0, 1].PutValue("ID счёта");
        ws.Cells[0, 2].PutValue("ID валюты");
        ws.Cells[0, 3].PutValue("Дата");
        ws.Cells[0, 4].PutValue("Поступление");
        for (int i = 0; i < service.Transactions.Count; i++)
        {
            var t = service.Transactions[i];
            ws.Cells[i + 1, 0].PutValue(t.ID);
            ws.Cells[i + 1, 1].PutValue(t.AccountID);
            ws.Cells[i + 1, 2].PutValue(t.CurrencyID);
            ws.Cells[i + 1, 3].PutValue(t.Date.ToString("dd.MM.yyyy"));
            ws.Cells[i + 1, 4].PutValue(t.Amount);
        }

        workbook.Save(filePath);
    }
}