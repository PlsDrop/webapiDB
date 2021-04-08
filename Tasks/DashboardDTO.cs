using System.Collections.Generic;

namespace webapi
{
    public class DashboardDTO
    {
        public int todayTasks { get; set; }
        public List<TasksListDTO> taskLists { get; set; }
    }

    public class TasksListDTO
    {
        public int notDoneTasks { get; set; }
        public TasksList tasksList { get; set; }
    }
}