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
            // 3. 开始执行相关任务
            Log.Info("当前系统时间:" + Time.GetTimestamp());
            await Task.Delay(1000);
            Log.Info("当前系统时间:" + Time.GetTimestampByMS());
        }
    }
}
