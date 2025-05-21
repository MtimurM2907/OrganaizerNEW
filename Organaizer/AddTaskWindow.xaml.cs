using Organaizer;
using System.Windows;

namespace Organaizer
{
    public partial class AddTaskWindow : Window
    {
        private readonly bool _isEditMode;
        private readonly TaskItem? _originalTask;

        public TaskItem NewTask { get; }

        public AddTaskWindow(TaskItem? taskToEdit = null)
        {
            InitializeComponent();

            if (taskToEdit != null)
            {
                _isEditMode = true;
                _originalTask = taskToEdit;
                NewTask = new TaskItem
                {
                    Id = taskToEdit.Id,
                    Title = taskToEdit.Title,
                    Description = taskToEdit.Description,
                    DueDate = taskToEdit.DueDate,
                    Priority = taskToEdit.Priority,
                    Category = taskToEdit.Category,
                    IsCompleted = taskToEdit.IsCompleted
                };
                Title = "Редактировать задачу";
            }
            else
            {
                NewTask = new TaskItem();
                Title = "Добавить задачу";
            }

            DataContext = NewTask;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(NewTask.Title))
                {
                    MessageBox.Show("Введите название задачи!");
                    return;
                }

                if (NewTask.DueDate == default)
                {
                    MessageBox.Show("Выберите дату выполнения!");
                    return;
                }

                var db = new DatabaseContext();

                if (_isEditMode)
                {
                    db.UpdateTask(NewTask);
                }
                else
                {
                    db.AddTask(NewTask);
                }

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}");
            }
        }
    }
}