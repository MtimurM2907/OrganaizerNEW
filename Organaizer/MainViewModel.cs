using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Organaizer;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

namespace Organaizer
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly DatabaseContext _dbContext = new DatabaseContext();

        [ObservableProperty]
        private ObservableCollection<TaskItem> _tasks = new ObservableCollection<TaskItem>();

        [ObservableProperty]
        private string[] _sortOptions = { "По приоритету", "По дате", "Без сортировки" };

        [ObservableProperty]
        private string _selectedSortOption = "По приоритету";

        public MainViewModel()
        {
            LoadTasks();
        }

        private void LoadTasks()
        {
            try
            {
                List<TaskItem> tasks = SelectedSortOption switch
                {
                    "По приоритету" => _dbContext.GetAllTasksSortedByPriority(),
                    "По дате" => _dbContext.GetAllTasksSortedByDate(),
                    _ => _dbContext.GetAllTasks()
                };

                Tasks.Clear();
                foreach (var task in tasks)
                {
                    Tasks.Add(task);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки задач: {ex.Message}");
            }
        }

        [RelayCommand]
        private void AddTask()
        {
            var addWindow = new AddTaskWindow();
            if (addWindow.ShowDialog() == true)
            {
                LoadTasks();
            }
        }

        [RelayCommand]
        private void EditTask(TaskItem task)
        {
            if (task == null) return;

            var editWindow = new AddTaskWindow(task);
            if (editWindow.ShowDialog() == true)
            {
                _dbContext.UpdateTask(editWindow.NewTask);
                LoadTasks();
            }
        }

        [RelayCommand]
        private void UpdateTaskStatus(TaskItem task)
        {
            try
            {
                _dbContext.UpdateTaskCompletion(task.Id, task.IsCompleted);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения статуса: {ex.Message}");
                task.IsCompleted = !task.IsCompleted; // Откат изменения
            }
        }

        [RelayCommand]
        private void DeleteTask(int taskId)
        {
            var result = MessageBox.Show(
                "Удалить задачу?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _dbContext.DeleteTask(taskId);
                    LoadTasks();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка удаления: {ex.Message}");
                }
            }
        }

        [RelayCommand]
        private void ChangeSorting()
        {
            LoadTasks();
        }
    }
}