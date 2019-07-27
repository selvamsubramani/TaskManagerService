using System;

namespace TaskManager.Entities
{
    public class Task
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Task Parent { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Priority { get; set; }
        public bool Status { get; set; }
    }
}
