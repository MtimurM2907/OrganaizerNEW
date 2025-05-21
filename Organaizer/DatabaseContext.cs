using Microsoft.Data.Sqlite;
using Organaizer;
using System;
using System.Collections.Generic;
using System.IO;

public class DatabaseContext
{
    private readonly string _connectionString;

    public DatabaseContext()
    {
        var databasePath = Path.Combine(Environment.CurrentDirectory, "tasks.db");
        _connectionString = $"Data Source={databasePath};";
        InitializeDatabase();
    }

    private void InitializeDatabase()
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Tasks (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Title TEXT NOT NULL,
                    Description TEXT,
                    DueDate TEXT NOT NULL,
                    Priority TEXT,
                    Category TEXT,
                    IsCompleted INTEGER DEFAULT 0
                    
                )";
            command.ExecuteNonQuery();
        }
    }

    public void AddTask(TaskItem task)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO Tasks 
                (Title, Description, DueDate, Priority, Category, IsCompleted)
                VALUES 
                (@title, @description, @dueDate, @priority, @category, @isCompleted)";

            command.Parameters.AddWithValue("@title", task.Title);
            command.Parameters.AddWithValue("@description", task.Description ?? "");
            command.Parameters.AddWithValue("@dueDate", task.DueDate.ToString("yyyy-MM-dd HH:mm:ss"));
            command.Parameters.AddWithValue("@priority", task.Priority);
            command.Parameters.AddWithValue("@category", task.Category ?? "");
            command.Parameters.AddWithValue("@isCompleted", task.IsCompleted ? 1 : 0);
            

            command.ExecuteNonQuery();
        }
    }

    public List<TaskItem> GetAllTasks()
    {
        var tasks = new List<TaskItem>();
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Tasks";

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    tasks.Add(new TaskItem
                    {
                        Id = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        Description = reader.IsDBNull(2) ? "" : reader.GetString(2),
                        DueDate = DateTime.Parse(reader.GetString(3)),
                        Priority = reader.IsDBNull(4) ? "Средний" : reader.GetString(4),
                        Category = reader.IsDBNull(5) ? "" : reader.GetString(5),
                        IsCompleted = reader.GetInt32(6) == 1
                    });
                }
            }
        }
        return tasks;
    }

    public List<TaskItem> GetAllTasksSortedByPriority()
    {
        var tasks = new List<TaskItem>();
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT * FROM Tasks
                ORDER BY CASE Priority
                    WHEN 'Высокий' THEN 1
                    WHEN 'Средний' THEN 2
                    WHEN 'Низкий' THEN 3
                    ELSE 4
                END, DueDate";

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    tasks.Add(new TaskItem
                    {
                        Id = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        Description = reader.IsDBNull(2) ? "" : reader.GetString(2),
                        DueDate = DateTime.Parse(reader.GetString(3)),
                        Priority = reader.IsDBNull(4) ? "Средний" : reader.GetString(4),
                        Category = reader.IsDBNull(5) ? "" : reader.GetString(5),
                        IsCompleted = reader.GetInt32(6) == 1
                    });
                }
            }
        }
        return tasks;
    }

    public List<TaskItem> GetAllTasksSortedByDate()
    {
        var tasks = new List<TaskItem>();
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Tasks ORDER BY DueDate";

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    tasks.Add(new TaskItem
                    {
                        Id = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        Description = reader.IsDBNull(2) ? "" : reader.GetString(2),
                        DueDate = DateTime.Parse(reader.GetString(3)),
                        Priority = reader.IsDBNull(4) ? "Средний" : reader.GetString(4),
                        Category = reader.IsDBNull(5) ? "" : reader.GetString(5),
                        IsCompleted = reader.GetInt32(6) == 1
                    });
                }
            }
        }
        return tasks;
    }

    public void UpdateTask(TaskItem task)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"
                UPDATE Tasks SET
                    Title = @title,
                    Description = @description,
                    DueDate = @dueDate,
                    Priority = @priority,
                    Category = @category,
                    IsCompleted = @isCompleted
                WHERE Id = @id";
            

            command.Parameters.AddWithValue("@id", task.Id);
            command.Parameters.AddWithValue("@title", task.Title);
            command.Parameters.AddWithValue("@description", task.Description ?? "");
            command.Parameters.AddWithValue("@dueDate", task.DueDate.ToString("yyyy-MM-dd HH:mm:ss"));
            command.Parameters.AddWithValue("@priority", task.Priority);
            command.Parameters.AddWithValue("@category", task.Category ?? "");
            command.Parameters.AddWithValue("@isCompleted", task.IsCompleted ? 1 : 0);

            command.ExecuteNonQuery();
        }
    }

    public void UpdateTaskCompletion(int taskId, bool isCompleted)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "UPDATE Tasks SET IsCompleted = @isCompleted WHERE Id = @id";
            command.Parameters.AddWithValue("@id", taskId);
            command.Parameters.AddWithValue("@isCompleted", isCompleted ? 1 : 0);
            command.ExecuteNonQuery();
        }
    }

    public void DeleteTask(int taskId)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Tasks WHERE Id = @id";
            command.Parameters.AddWithValue("@id", taskId);
            command.ExecuteNonQuery();
        }
    }
}