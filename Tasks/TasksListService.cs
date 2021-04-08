using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

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
            return _context.taskLists.ToList();
        }

        internal TasksList CreateList(string name)
        {
            TasksList list = new TasksList() {title = name};
            _context.taskLists.Add(list);
            _context.SaveChanges();
            return list;
        }

        internal List<Task> GetTaskList(int listId, bool all)
        {
            if(all)
                return _context.tasks
                .Where(t => (t.tasksListId == listId))
                .ToList();
            else
                return _context.tasks
                .Where(t => (t.tasksListId == listId) && (t.done == false))
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

        internal List<TodayTaskDTO> GetTodayTasks()
        {
            List<TodayTaskDTO> list = new List<TodayTaskDTO>();
            var tasks = _context.tasks.Where(b => b.dueDate.Value.Day == DateTime.Today.Day).Include(t => t.tasksList);
            foreach (var item in tasks.ToList())
            {
                TodayTaskDTO taskDTO = new TodayTaskDTO();
                taskDTO.listName = item.tasksList.title;
                item.tasksList = null;
                taskDTO.task = item;
                list.Add(taskDTO);
            }
            return list;
        }

        internal DashboardDTO GetDashboard()
        {
            DashboardDTO dashboardDTO = new DashboardDTO();
            dashboardDTO.todayTasks = _context.tasks.Where(b => b.dueDate.Value.Day == DateTime.Today.Day).Count();
            dashboardDTO.taskLists = new List<TasksListDTO>();
            foreach(var item in _context.tasks.Where(t => t.done == false).AsEnumerable().GroupBy(t => t.tasksListId))
            {
                TasksListDTO tasksListDTO = new TasksListDTO()
                {
                    notDoneTasks = item.Count(), 
                    tasksList = _context.taskLists
                    .Where(l => l.tasksListId == item.Key)
                    .Single() 
                };
                tasksListDTO.tasksList.tasks = null;
                dashboardDTO.taskLists.Add(tasksListDTO);            
            }
            return dashboardDTO;
        }
    }
}