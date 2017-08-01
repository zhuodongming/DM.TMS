using DM.TMS.Domain.Interface.TMS;
using DM.TMS.Domain.Service;
using DM.TMS.Domain.TMS;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DM.TMS.App.TMS
{
    public class TaskApp
    {
        private ITaskRepository taskRepository;
        public TaskApp(ITaskRepository taskRepository)
        {
            this.taskRepository = taskRepository;
        }

        public async Task StartTaskHost()
        {
            string strWhere = " where IsEnabled=1 ";
            List<TaskModel> taskModelList = await taskRepository.FetchAsync(strWhere);
            await TaskPoolManager.InitScheduler();
            await TaskPoolManager.StartScheduler(taskModelList);
        }
    }
}
