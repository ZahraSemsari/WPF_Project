using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TaskManager.Models;

namespace TaskManager.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly HttpClient _httpClient;
        private ObservableCollection<List> _lists = new();
        private List _selectedList;
        private TODOTask _selectedTask;
        private string _listName;
        private string _taskName;
        private DateTime _taskStartTime = DateTime.Now;
        private DateTime _taskEndTime = DateTime.Now;
        private bool _taskStarred;
        private bool _taskChecked;

        public MainViewModel()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7048/api/") };
            LoadLists();
        }

        public ObservableCollection<List> Lists
        {
            get => _lists;
            set
            {
                _lists = value;
                OnPropertyChanged();
            }
        }

        public List SelectedList
        {
            get => _selectedList;
            set
            {
                _selectedList = value;
                OnPropertyChanged();
                if (_selectedList != null)
                {
                    LoadTasks();
                }
            }
        }

        public TODOTask SelectedTask
        {
            get => _selectedTask;
            set
            {
                _selectedTask = value;
                OnPropertyChanged();
                if (_selectedTask != null)
                {
                    TaskName = _selectedTask.Name;
                    TaskStartTime = _selectedTask.StartTime;
                    TaskEndTime = _selectedTask.EndTime;
                    TaskStarred = _selectedTask.Starred;
                    TaskChecked = _selectedTask.Checked;
                }
            }
        }

        public string ListName
        {
            get => _listName;
            set
            {
                _listName = value;
                OnPropertyChanged();
            }
        }

        public string TaskName
        {
            get => _taskName;
            set
            {
                _taskName = value;
                OnPropertyChanged();
            }
        }

        public DateTime TaskStartTime
        {
            get => _taskStartTime;
            set
            {
                _taskStartTime = value;
                OnPropertyChanged();
            }
        }

        public DateTime TaskEndTime
        {
            get => _taskEndTime;
            set
            {
                _taskEndTime = value;
                OnPropertyChanged();
            }
        }

        public bool TaskStarred
        {
            get => _taskStarred;
            set
            {
                if (_taskStarred != value)
                {
                    _taskStarred = value;
                    OnPropertyChanged();
                    UpdateTaskProperties().ConfigureAwait(false);
                }
            }
        }

        public bool TaskChecked
        {
            get => _taskChecked;
            set
            {
                if (_taskChecked != value)
                {
                    _taskChecked = value;
                    OnPropertyChanged();
                    UpdateTaskProperties().ConfigureAwait(false);
                }
            }
        }


        public async Task UpdateTaskProperties()
        {
            if (SelectedTask != null)
            {
                var content = new StringContent(JsonSerializer.Serialize(SelectedTask), Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"Task/{SelectedTask.Id}", content);
                if (response.IsSuccessStatusCode)
                {
                    OnPropertyChanged(nameof(SelectedTask));
                    OnPropertyChanged(nameof(SelectedList.Tasks));
                }
                else
                {
                    MessageBox.Show("Failed to update the task.");
                }
            }
        }


        public async void LoadLists()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<ListTemp>>("list");
                if (response != null)
                {
                    Lists.Clear();
                    foreach (var item in response)
                    {
                        Lists.Add(new List
                        {
                            Name = item.Name,
                            Id = item.Id,
                            Tasks = new ObservableCollection<TODOTask>()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load lists: {ex.Message}");
            }
        }

        public async void LoadTasks()
        {
            if (SelectedList != null)
            {
                try
                {
                    var response = await _httpClient.GetFromJsonAsync<List<TODOTask>>($"Task/Filter_listId?ListId={SelectedList.Id}");
                    if (response != null)
                    {
                        SelectedList.Tasks.Clear();
                        foreach (var item in response)
                        {
                            SelectedList.Tasks.Add(item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to load tasks: {ex.Message}");
                }
            }
        }

        public void ResetForm()
        {
            TaskChecked = false;
            TaskStarred = false;
            TaskStartTime = DateTime.Now;
            TaskEndTime = DateTime.Now;
            TaskName = "";
        }

        public async Task AddList()
        {
            var list = new ListTemp { Name = ListName };
            var response = await _httpClient.PostAsJsonAsync("list", list);
            if (response.IsSuccessStatusCode)
            {
                LoadLists();
                ResetForm();
            }
            else
            {
                MessageBox.Show("Failed to add list.");
            }
        }

        public async Task AddTask(object sender, RoutedEventArgs e)
        {
            if (SelectedList != null)
            {
                var task = new TODOTask
                {
                    Name = TaskName,
                    StartTime = TaskStartTime,
                    EndTime = TaskEndTime,
                    Starred = TaskStarred,
                    Checked = TaskChecked,
                    ListId = SelectedList.Id,
                };

                var content = new StringContent(JsonSerializer.Serialize(task), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("Task", content);//here
                if (response.IsSuccessStatusCode)
                {
                    SelectedList.Tasks.Add(task);
                    OnPropertyChanged(nameof(SelectedList.Tasks));
                }
                else
                {
                    MessageBox.Show("Failed to add task.");
                }
            }
        }

        public async Task DeleteTask()
        {
            if (SelectedTask != null)
            {
                var response = await _httpClient.DeleteAsync($"Task/{SelectedTask.Id}");//here
                if (response.IsSuccessStatusCode)
                {
                    SelectedList.Tasks.Remove(SelectedTask);
                    SelectedTask = null;
                    OnPropertyChanged(nameof(SelectedList.Tasks));
                }
                else
                {
                    MessageBox.Show("Failed to delete task.");
                }
            }
        }

        public async Task RenameTask(string newName)
        {
            if (SelectedTask != null && !string.IsNullOrWhiteSpace(newName))
            {
                SelectedTask.Name = newName;
                var content = new StringContent(JsonSerializer.Serialize(SelectedTask), Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"Task/{SelectedTask.Id}", content);
                if (response.IsSuccessStatusCode)
                {
                    OnPropertyChanged(nameof(SelectedTask));
                    OnPropertyChanged(nameof(SelectedList.Tasks));
                }
                else
                {
                    MessageBox.Show("Failed to rename the task.");
                }
            }
        }

        public async Task DeleteList()
        {
            if (SelectedList != null)
            {
                var response = await _httpClient.DeleteAsync($"list/{SelectedList.Id}");
                if (response.IsSuccessStatusCode)
                {
                    Lists.Remove(SelectedList);
                    SelectedList = null;
                    OnPropertyChanged(nameof(Lists));
                    OnPropertyChanged(nameof(SelectedList));
                }
                else
                {
                    MessageBox.Show("Failed to delete the list.");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
