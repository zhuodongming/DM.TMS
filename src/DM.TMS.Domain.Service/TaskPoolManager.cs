using DM.Infrastructure.Helper;
using DM.TMS.Domain.TMS;
using Microsoft.Extensions.PlatformAbstractions;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using Quartz.Impl.Triggers;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;

namespace DM.TMS.Domain.Service
{
    public static class TaskPoolManager
    {
        private static IScheduler scheduler = null;

        /// <summary>
        /// 初始化任务调度对象
        /// </summary>
        public static async Task InitScheduler()
        {
            try
            {
                if (scheduler != null)
                {
                    Log.Warn("任务管理器已经初始化过");
                    return;
                }

                scheduler = await new StdSchedulerFactory().GetScheduler();
                scheduler.ListenerManager.AddTriggerListener(new CustomTriggerListener(), GroupMatcher<TriggerKey>.AnyGroup());//添加全局监听

                Log.Info("任务调度初始化成功！");
            }
            catch (Exception ex)
            {
                Log.Error("任务调度初始化失败！", ex);
            }
        }

        /// <summary>
        /// 初始化 远程Quartz服务器中的，各个Scheduler实例。
        /// 提供给远程管理端的后台，用户获取Scheduler实例的信息。
        /// </summary>
        public static async void InitRemoteScheduler(string remoteServer, int port)
        {
            try
            {
                if (scheduler != null)
                {
                    Log.Warn("任务管理器已经初始化过");
                    return;
                }

                NameValueCollection properties = new NameValueCollection();
                properties["quartz.scheduler.instanceName"] = "ExampleQuartzScheduler";
                properties["quartz.scheduler.proxy"] = "true";
                properties["quartz.scheduler.proxy.address"] = $"tcp://{remoteServer}:{port}/QuartzScheduler";

                scheduler = await new StdSchedulerFactory(properties).GetScheduler();

                Log.Info("远程任务调度初始化成功！");
            }
            catch (Exception ex)
            {
                Log.Error("初始化远程任务管理器失败" + ex);
            }
        }

        /// <summary>
        /// 启用任务调度
        /// 启动调度时会把任务表中状态为“执行中”的任务加入到任务调度队列中
        /// </summary>
        public static async Task StartScheduler(List<TaskModel> taskModelList)
        {
            try
            {
                if (!scheduler.IsStarted)
                {
                    await scheduler.Start();

                    foreach (TaskModel taskUtil in taskModelList)
                    {
                        try
                        {
                            await ScheduleJob(taskUtil);
                        }
                        catch (Exception e)
                        {
                            Log.Error($"任务“{taskUtil.TaskName}”启动失败！", e);
                        }
                    }
                    Log.Info("任务调度启动成功！");
                }
            }
            catch
            {
                Log.Error("任务调度启动失败！");
                throw;
            }
        }

        /// <summary>
        /// 停止任务调度
        /// </summary>
        public static async Task StopSchedule()
        {
            try
            {
                //判断调度是否已经关闭
                if (!scheduler.IsShutdown)
                {
                    //等待任务运行完成
                    await scheduler.Shutdown(true);
                    Log.Info("任务调度停止！");
                }
            }
            catch
            {
                Log.Error("任务调度停止失败！");
                throw;
            }
        }

        /// <summary>
        /// 启用任务
        /// <param name="task">任务信息</param>
        /// <param name="isDeleteOldTask">是否删除原有任务</param>
        /// <returns>返回任务trigger</returns>
        /// </summary>
        public static async Task ScheduleJob(TaskModel task, bool isDeleteOldTask = false)
        {
            if (isDeleteOldTask)
            {
                await DeleteJob(task.TaskID);//先删除现有已存在任务
            }

            if (CronExpression.IsValidExpression(task.CronExpressionString))//验证是否正确的Cron表达式
            {
                IJobDetail job = new JobDetailImpl(task.TaskID, GetClassTypeInfo(task.AssemblyFullName, task.ClassFullName));
                CronTriggerImpl trigger = new CronTriggerImpl();
                trigger.CronExpressionString = task.CronExpressionString;
                trigger.Name = task.TaskID;
                trigger.Description = task.TaskName;
                await scheduler.ScheduleJob(job, trigger);
                //if (task.Status == TaskStatus.STOP)
                //{
                //    JobKey jk = new JobKey(task.TaskID);
                //    await scheduler.PauseJob(jk);
                //}

                Log.Info($"任务“{task.TaskName}”启动成功,未来5次运行时间如下:");
                List<DateTime> list = GetNextFireTime(task.CronExpressionString, 5);
                foreach (var time in list)
                {
                    Log.Info(time.ToString());
                }
            }
            else
            {
                throw new Exception(task.CronExpressionString + "不是正确的Cron表达式,无法启动该任务!");
            }
        }

        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="jobKey"></param>
        public static async Task DeleteJob(string jobKey)
        {
            try
            {
                JobKey jk = new JobKey(jobKey);
                if (await scheduler.CheckExists(jk))
                {
                    await scheduler.DeleteJob(jk);
                    Log.Info($"任务“{jobKey}”删除成功");
                }
            }
            catch
            {
                Log.Error($"任务“{jobKey}”删除失败");
                throw;
            }
        }

        /// <summary>
        /// 暂停任务
        /// </summary>
        /// <param name="JobKey"></param>
        public static async Task PauseJob(string jobKey)
        {
            try
            {
                JobKey jk = new JobKey(jobKey);
                if (await scheduler.CheckExists(jk))
                {
                    await scheduler.PauseJob(jk);
                    Log.Info($"任务“{jobKey}”已经暂停");
                }
            }
            catch
            {
                Log.Error($"暂停任务“{jobKey}”失败!");
                throw;
            }
        }

        /// <summary>
        /// 恢复任务
        /// </summary>
        /// <param name="JobKey">任务key</param>
        public static async Task ResumeJob(string JobKey)
        {
            try
            {
                JobKey jk = new JobKey(JobKey);
                if (await scheduler.CheckExists(jk))
                {
                    await scheduler.ResumeJob(jk);
                    Log.Info($"任务“{JobKey}”恢复运行");
                }
            }
            catch
            {
                Log.Error("恢复任务失败!");
                throw;
            }
        }

        /// 获取类的属性、方法  
        /// </summary>  
        /// <param name="assemblyName">程序集</param>  
        /// <param name="className">类名</param>  
        private static Type GetClassTypeInfo(string assemblyFullName, string classFullName)
        {
            try
            {
                AssemblyLoadContext assemblyContext = AssemblyLoadContext.Default;
                assemblyContext.Resolving += ((arg1, arg2) =>
                {
                    string dllPath = new ApplicationEnvironment().ApplicationBasePath + "\\Tasks\\" + arg2.Name + ".dll";
                    Assembly dllAssembly = assemblyContext.LoadFromAssemblyPath(dllPath);
                    return dllAssembly;
                });

                string filePath = new ApplicationEnvironment().ApplicationBasePath + "\\Tasks\\" + assemblyFullName;
                Assembly assembly = assemblyContext.LoadFromAssemblyPath(filePath);
                Type type = assembly.GetType(classFullName, true, true);
                return type;
            }
            catch
            {
                Log.Error($"加载程序集{assemblyFullName}类型{classFullName}出错");
                throw;
            }
        }


        /// <summary>
        /// 获取任务在未来周期内哪些时间会运行
        /// </summary>
        /// <param name="CronExpressionString">Cron表达式</param>
        /// <param name="numTimes">运行次数</param>
        /// <returns>运行时间段</returns>
        public static List<DateTime> GetNextFireTime(string CronExpressionString, int numTimes)
        {
            if (numTimes < 0)
            {
                throw new Exception("参数numTimes值大于等于0");
            }
            //时间表达式
            ITrigger trigger = TriggerBuilder.Create().WithCronSchedule(CronExpressionString).Build();
            IList<DateTimeOffset> dates = TriggerUtils.ComputeFireTimes(trigger as IOperableTrigger, null, numTimes);
            List<DateTime> list = new List<DateTime>();
            foreach (DateTimeOffset dtf in dates)
            {
                list.Add(TimeZoneInfo.ConvertTime(dtf.DateTime, TimeZoneInfo.Local));
            }
            return list;
        }

        /// <summary>
        /// 获取当前执行的Task 对象
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static TaskModel GetTaskDetail(IJobExecutionContext context)
        {
            TaskModel task = new TaskModel();

            if (context != null)
            {
                task.TaskID = context.Trigger.Key.Name;
                task.TaskName = context.Trigger.Description;
                task.LastRunTime = DateTime.Now;
            }
            return task;
        }
    }
}
