using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace webapi
{
    public class TasksDBContext : DbContext
    {
        public DbSet<Task> tasks { get; set; } 
        public DbSet<TasksList> taskLists { get; set; } 
        
        public TasksDBContext(DbContextOptions<TasksDBContext> options) : base (options) {}

    }
    
    
    public class TasksList
    { 
        public int tasksListId { get; set; }
        public string title { get; set; }
        public List<Task> tasks { get; set; }
    }

    public class Task
    {
        public int taskId { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public DateTime? dueDate { get; set; }
        public bool done { get; set; }

        public int tasksListId { get; set; }
        public TasksList tasksList { get; set; }
    }
}