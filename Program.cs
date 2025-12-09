using System;

class Program
{
    static void Main()
    {
        DatabaseManager dbManager = new DatabaseManager();
        const string FILE_PATH = "LR5-var12.xls";

        try
        {
            dbManager.LoadData(FILE_PATH);
            Console.WriteLine("Данные успешно загружены из Excel.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка загрузки Excel: {ex.Message}");
            Console.WriteLine("Нажмите любую клавишу для выхода...");
            Console.ReadKey();
            return;
        }

        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Банковская система (Инвестиционные счета) ===");
            Console.WriteLine("1. Просмотр всех счетов");
            Console.WriteLine("2. Просмотр всех валют");
            Console.WriteLine("3. Просмотр всех курсов валют");
            Console.WriteLine("4. Просмотр всех начислений");
            Console.WriteLine("5. Добавить счёт");
            Console.WriteLine("6. Удалить счёт по ID");
            Console.WriteLine("7. Запрос 1: Счета, открытые в январе 2021");
            Console.WriteLine("8. Запрос 2: Транзакции >4 руб.");
            Console.WriteLine("9. Запрос 3: Владелец с max доходом");
            Console.WriteLine("10. Запрос 4: Счета с отриц. балансом на 26.12.2021");
            Console.WriteLine("11. Сохранить и выйти");
            Console.Write("Выберите действие: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    dbManager.PrintAllAccounts();
                    Console.ReadKey();
                    break;
                case "2":
                    dbManager.PrintAllCurrencies();
                    Console.ReadKey();
                    break;
                case "3":
                    dbManager.PrintAllExchangeRates();
                    Console.ReadKey();
                    break;
                case "4":
                    dbManager.PrintAllTransactions();
                    Console.ReadKey();
                    break;
                case "5":
                    try
                    {
                        Console.Write("ID: "); int id = int.Parse(Console.ReadLine());
                        Console.Write("ФИО: "); string fio = Console.ReadLine();
                        Console.Write("Дата открытия (дд.мм.гггг): ");
                        DateTime dt = DateTime.Parse(Console.ReadLine());
                        dbManager.AddAccount(new Account(id, fio, dt));
                        Console.WriteLine("Счёт добавлен.");
                    }
                    catch (Exception ex) { Console.WriteLine("Ошибка: " + ex.Message); }
                    Console.ReadKey();
                    break;
                case "6":
                    Console.Write("ID счёта для удаления: ");
                    if (int.TryParse(Console.ReadLine(), out int delId))
                    {
                        if (dbManager.RemoveAccount(delId))
                            Console.WriteLine("Счёт удалён.");
                        else
                            Console.WriteLine("Счёт не найден.");
                    }
                    Console.ReadKey();
                    break;
                case "7":
                    foreach (var acc in dbManager.GetAccountsOpenedInJanuary2021())
                        Console.WriteLine(acc);
                    Console.ReadKey();
                    break;
                case "8":
                    foreach (var (t, rub) in dbManager.GetLargeTransactionsInRub())
                        Console.WriteLine($"{t} → {rub:F2} руб.");
                    Console.ReadKey();
                    break;
                case "9":
                    Console.WriteLine("Лучший заработавший клиент: " + dbManager.GetTopEarnerFIO());
                    Console.ReadKey();
                    break;
                case "10":
                    foreach (var (id, fio, bal) in dbManager.GetNegativeBalancesOnDate(new DateTime(2021, 12, 26)))
                        Console.WriteLine($"Счёт {id} ({fio}): {bal:F2} руб.");
                    Console.ReadKey();
                    break;
                case "11":
                    try
                    {
                        dbManager.SaveData(); 
                        Console.WriteLine($"Изменения сохранены в {FILE_PATH}"); 
                    }
                    catch (Exception ex) { Console.WriteLine("Ошибка сохранения: " + ex.Message); }
                    Console.WriteLine("Нажмите любую клавишу для выхода...");
                    Console.ReadKey();
                    return;
                default:
                    Console.WriteLine("Неверный выбор.");
                    Console.ReadKey();
                    break;
            }
        }
    }
}