using System;
using System.Linq;
using System.Collections.Generic;

namespace webapi
{
    public class  TasksListService
    {
        private TasksDBContext _context;

        public TasksListService(TasksDBContext context)
        {
            this._context = context; 
        }

        public List<TasksList> GetLists()
        {
            return _context.taskLists.ToList<TasksList>();
        }

        internal TasksList CreateList(string name)
        {
            TasksList list = new TasksList() {title = name};
            _context.taskLists.Add(list);
            _context.SaveChanges();
            return list;
        }

        internal List<Task> GetTaskList(int listId)
        {
            return _context.tasks
            .Where(t => t.tasksListId == listId)
            .ToList();
        }

        internal TasksList PatchList(int listId, string name)
        {
            _context.taskLists
            .Where(l => l.tasksListId == listId)
            .Single()
            .title = name;
            _context.SaveChanges();
            return new TasksList() {title = name};
        }

        internal Task CreateTask(int listId, Task task)
        {
            task.tasksListId = listId;
            _context.tasks.Add(task);
            _context.SaveChanges();
            return task;
        }

        internal Task PatchTask(int listId, int taskId, Task task)
        {
            task.tasksListId = listId;
            task.taskId = taskId;
            _context.tasks.Update(task);
            _context.SaveChanges();
            // .Where(t => (t.tasksListId == listId) && (t.taskId == taskId))
            return task;
        }

        internal void DeleteList(int listId)
        {
            TasksList list = new TasksList() {tasksListId = listId};
            _context.taskLists.Remove(list);
            _context.SaveChanges();
        }

        internal void DeleteTask(int listId, int _taskId)
        {
            Task task = new Task() {tasksListId = listId, taskId = _taskId};
            _context.tasks.Remove(task);
            _context.SaveChanges();
        }
    }
}