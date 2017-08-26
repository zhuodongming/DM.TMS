using DM.Infrastructure.Helper;
using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DM.TMS.Job.OutputTest
{
    public class OutputTestJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            //string taskName = context.Trigger.JobKey.Name;
            // 3. 开始执行相关任务
            Log.Info("当前系统时间:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            await Task.CompletedTask;
        }
    }
}
