using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace DM.TMS.Domain.Service
{
    /// <summary>
    /// 自定义触发器监听
    /// </summary>
    public class CustomTriggerListener : ITriggerListener
    {
        public string Name
        {
            get
            {
                return "All_TriggerListener";
            }
        }

        /// <summary>
        /// Job执行时调用
        /// </summary>
        /// <param name="trigger">触发器</param>
        /// <param name="context">上下文</param>
        public async Task TriggerFired(ITrigger trigger, IJobExecutionContext context, CancellationToken cancellationToken = default(CancellationToken))
        {

        }


        /// <summary>
        ///  //Trigger触发后，job执行时调用本方法。true即否决，job后面不执行。
        /// </summary>
        /// <param name="trigger"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<bool> VetoJobExecution(ITrigger trigger, IJobExecutionContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            //TaskHelper.UpdateRecentRunTime(trigger.JobKey.Name, TimeZoneInfo.ConvertTimeFromUtc(context.NextFireTimeUtc.Value.DateTime, TimeZoneInfo.Local));
            return false;
        }

        /// <summary>
        /// Job完成时调用
        /// </summary>
        /// <param name="trigger">触发器</param>
        /// <param name="context">上下文</param>
        /// <param name="triggerInstructionCode"></param>
        public async Task TriggerComplete(ITrigger trigger, IJobExecutionContext context, SchedulerInstruction triggerInstructionCode, CancellationToken cancellationToken = default(CancellationToken))
        {
            //TaskHelper.UpdateLastRunTime(trigger.JobKey.Name, TimeZoneInfo.ConvertTimeFromUtc(context.NextFireTimeUtc.Value.DateTime, TimeZoneInfo.Local));
        }

        /// <summary>
        /// 错过触发时调用
        /// </summary>
        /// <param name="trigger">触发器</param>
        public async Task TriggerMisfired(ITrigger trigger, CancellationToken cancellationToken = default(CancellationToken))
        {
        }
    }
}
