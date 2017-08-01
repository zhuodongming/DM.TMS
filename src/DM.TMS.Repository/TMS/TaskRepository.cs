using DM.TMS.Domain.Interface.TMS;
using DM.TMS.Domain.TMS;
using System;
using System.Collections.Generic;
using System.Text;
using DM.TMS.Domain;
using Microsoft.Extensions.Options;

namespace DM.TMS.Repository.TMS
{
    public class TaskRepository : TMSRepository<TaskModel>, ITaskRepository
    {
        public TaskRepository(IOptions<DBSettings> dbSettings) : base(dbSettings)
        {
        }
    }
}
