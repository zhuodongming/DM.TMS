using DM.TMS.Domain.Interface.TMS;
using DM.TMS.Domain.TMS;
using System;
using System.Collections.Generic;
using System.Text;

namespace DM.TMS.Repository.TMS
{
    public class TaskRepository : TMSRepository<TaskModel>, ITaskRepository
    {
    }
}
