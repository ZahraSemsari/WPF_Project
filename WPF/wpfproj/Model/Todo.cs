using System;
using System.Collections.ObjectModel;

namespace TaskManager.Models
{
    public class TODOTask
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Starred { get; set; }
        public bool Checked { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int ListId { get; set; }
    }

    public class List
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ObservableCollection<TODOTask> Tasks { get; set; } = new ObservableCollection<TODOTask>();
    }

    public class ListTemp
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
