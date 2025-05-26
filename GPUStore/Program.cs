using System;
using System.Collections.Generic;

class Program
{
    static Database db = new Database();

    static void Main()
    {
        while (true)
        {
            try
            {
                ShowMenu();
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowGPUs();
                        break;
                    case "2":
                        AddGPU();
                        break;
                    case "3":
                        DeleteGPU();
                        break;
                    case "4":
                        UpdateGPU();
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Ошибка: выберите пункт меню от 1 до 5");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }

            Console.WriteLine("\nНажмите Enter для продолжения...");
            Console.ReadLine();
            Console.Clear();
        }
    }

    static void ShowMenu()
    {
        Console.WriteLine("Магазин видеокарт");
        Console.WriteLine("================");
        Console.WriteLine("1. Показать все видеокарты");
        Console.WriteLine("2. Добавить видеокарту");
        Console.WriteLine("3. Удалить видеокарту");
        Console.WriteLine("4. Изменить видеокарту");
        Console.WriteLine("5. Выход");
        Console.Write("\nВыберите действие (1-5): ");
    }

    static void ShowGPUs()
    {
        var gpus = db.GetAllGPUs();
        if (gpus.Count == 0)
        {
            Console.WriteLine("Список видеокарт пуст");
            return;
        }

        Console.WriteLine("\nСписок видеокарт:");
        for (int i = 0; i < gpus.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {gpus[i]}");
        }
    }

    static void AddGPU()
    {
        Console.Write("\nВведите название видеокарты: ");
        string name = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new Exception("Название не может быть пустым");
        }

        Console.Write("Введите цену: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal price) || price <= 0)
        {
            throw new Exception("Введите корректную цену (больше 0)");
        }

        Console.Write("Введите размер памяти (GB): ");
        if (!int.TryParse(Console.ReadLine(), out int memory) || memory <= 0)
        {
            throw new Exception("Введите корректный размер памяти (больше 0)");
        }

        var gpu = new GPU { Name = name, Price = price, Memory = memory };
        db.AddGPU(gpu);
        Console.WriteLine("Видеокарта успешно добавлена!");
    }

    static void DeleteGPU()
    {
        ShowGPUs();
        Console.Write("\nВведите номер видеокарты для удаления: ");
        if (!int.TryParse(Console.ReadLine(), out int index) || index < 1)
        {
            throw new Exception("Введите корректный номер видеокарты");
        }

        db.DeleteGPU(index);
        Console.WriteLine("Видеокарта успешно удалена!");
    }

    static void UpdateGPU()
    {
        ShowGPUs();
        Console.Write("\nВведите номер видеокарты для изменения: ");
        if (!int.TryParse(Console.ReadLine(), out int index) || index < 1)
        {
            throw new Exception("Введите корректный номер видеокарты");
        }

        Console.Write("Введите новое название видеокарты: ");
        string name = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new Exception("Название не может быть пустым");
        }

        Console.Write("Введите новую цену: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal price) || price <= 0)
        {
            throw new Exception("Введите корректную цену (больше 0)");
        }

        Console.Write("Введите новый размер памяти (GB): ");
        if (!int.TryParse(Console.ReadLine(), out int memory) || memory <= 0)
        {
            throw new Exception("Введите корректный размер памяти (больше 0)");
        }

        var gpu = new GPU { Name = name, Price = price, Memory = memory };
        db.UpdateGPU(index, gpu);
        Console.WriteLine("Видеокарта успешно изменена!");
    }
}
