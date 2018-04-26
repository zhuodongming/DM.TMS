using DM.Infrastructure.DI;
using DM.Infrastructure.Helper;
using DM.TMS.Domain.Service;
using DM.TMS.Domain.TMS;
using DM.TMS.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DM.TMS.App.TMS
{
    [SingletonDependency]
    public class TaskApp
    {
        public TMSRepository<TaskModel> TaskRep { get; set; }

        public async void StartTaskHost()
        {
            try
            {
                string strWhere = " where IsEnabled=1 ";
                List<TaskModel> taskModelList = await TaskRep.FetchAsync(strWhere);
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
