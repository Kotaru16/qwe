using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

public class Database
{
    private readonly string connectionString;

    public Database()
    {
        // Создаем базу данных в папке с программой
        string dbPath = "gpus.db";
        connectionString = $"Data Source={dbPath};Version=3;";
        
        // Создаем базу данных и таблицу, если они не существуют
        if (!File.Exists(dbPath))
        {
            SQLiteConnection.CreateFile(dbPath);
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string createTable = @"
                    CREATE TABLE GPUs (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL,
                        Price REAL NOT NULL,
                        Memory INTEGER NOT NULL
                    )";
                using (var command = new SQLiteCommand(createTable, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }

    public void AddGPU(GPU gpu)
    {
        try
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string insert = "INSERT INTO GPUs (Name, Price, Memory) VALUES (@Name, @Price, @Memory)";
                using (var command = new SQLiteCommand(insert, connection))
                {
                    command.Parameters.AddWithValue("@Name", gpu.Name);
                    command.Parameters.AddWithValue("@Price", gpu.Price);
                    command.Parameters.AddWithValue("@Memory", gpu.Memory);
                    command.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Ошибка при добавлении видеокарты: {ex.Message}");
        }
    }

    public List<GPU> GetAllGPUs()
    {
        var gpus = new List<GPU>();
        try
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string select = "SELECT Name, Price, Memory FROM GPUs";
                using (var command = new SQLiteCommand(select, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        gpus.Add(new GPU
                        {
                            Name = reader.GetString(0),
                            Price = reader.GetDecimal(1),
                            Memory = reader.GetInt32(2)
                        });
                    }
                }
            }
            return gpus;
        }
        catch (Exception ex)
        {
            throw new Exception($"Ошибка при получении списка видеокарт: {ex.Message}");
        }
    }

    public void DeleteGPU(int id)
    {
        try
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string delete = "DELETE FROM GPUs WHERE Id = @Id";
                using (var command = new SQLiteCommand(delete, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Ошибка при удалении видеокарты: {ex.Message}");
        }
    }

    public void UpdateGPU(int id, GPU gpu)
    {
        try
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string update = "UPDATE GPUs SET Name = @Name, Price = @Price, Memory = @Memory WHERE Id = @Id";
                using (var command = new SQLiteCommand(update, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@Name", gpu.Name);
                    command.Parameters.AddWithValue("@Price", gpu.Price);
                    command.Parameters.AddWithValue("@Memory", gpu.Memory);
                    command.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Ошибка при обновлении видеокарты: {ex.Message}");
        }
    }
} 