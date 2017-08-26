using DM.Infrastructure.Helper;
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

        public async void StartTaskHost()
        {
            try
            {
                string strWhere = " where IsEnabled=1 ";
                List<TaskModel> taskModelList = await taskRepository.FetchAsync(strWhere);
                await TaskPoolManager.Start();
                await TaskPoolManager.ScheduleJobs(taskModelList);
            }
            catch (Exception ex)
            {
                Log.Error("启动任务寄宿程序失败", ex);
            }
        }
    }
}
