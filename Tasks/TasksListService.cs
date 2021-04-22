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
                .OrderBy(t => t.taskId)
                .ToList();
            else
                return _context.tasks
                .Where(t => (t.tasksListId == listId) && (t.done == false))
                .OrderBy(t => t.taskId)
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

        internal Task PutTask(int listId, int taskId, Task task)
        {
            task.tasksListId = listId;
            task.taskId = taskId;
            _context.tasks.Update(task);
            _context.SaveChanges();
            // .Where(t => (t.tasksListId == listId) && (t.taskId == taskId))
            return task;
        }

        internal string PatchTaskStatus(int listId, int taskId, TaskStatusDTO taskStatusDTO)
        {
            _context.tasks.Where(t => t.taskId == taskId).First().done = taskStatusDTO.done;
            _context.SaveChanges();
            return $"Status of task {taskId} set to {taskStatusDTO.done}";
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
            return _context.tasks
                .Where(b => b.dueDate.Value.Date == DateTime.Today.Date)
                .Include(t => t.tasksList)
                .Select(ToTodayTaskDTO)
                .ToList();
        }

        

        private TodayTaskDTO ToTodayTaskDTO(Task task)
        {   
            TodayTaskDTO taskDTO = new TodayTaskDTO();
            taskDTO.listName = task.tasksList.title;
            task.tasksList = null;
            taskDTO.task = task;
            return taskDTO;
        }

        internal DashboardDTO GetDashboard()
        {
            List<TasksListDTO> list = new List<TasksListDTO>();
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "select l.tasks_list_id, l.title, Count(t.done) from tasks t right join task_lists l on l.tasks_list_id=t.tasks_list_id  where t.done=false group by l.tasks_list_id, l.title";
                _context.Database.OpenConnection();
                using (var result = command.ExecuteReader())
                {
                    while (result.Read())
                    {
                        list.Add(new TasksListDTO()
                        {
                            id = result.GetInt32(0),
                            title = result.IsDBNull(1) ? null : result.GetString(1),
                            notDoneTasks = result.GetInt32(2)
                        });
                    }
                }
            }        
            DashboardDTO outp = new DashboardDTO()
            {
                todayTasks = _context.tasks.Where(b => b.dueDate == DateTime.Today).Count(),
                taskLists = list
            };
            return outp;
        }

    }
}