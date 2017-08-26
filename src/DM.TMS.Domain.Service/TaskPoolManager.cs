using DM.Infrastructure.Helper;
using DM.TMS.Domain.TMS;
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
using System.Linq;
using DM.Infrastructure;

namespace DM.TMS.Domain.Service
{
    public static class TaskPoolManager
    {
        private static IScheduler scheduler = null;
        private static object lockObj = new object();//锁对象

        static TaskPoolManager()
        {
            scheduler = new StdSchedulerFactory().GetScheduler().Result;
            scheduler.ListenerManager.AddTriggerListener(new CustomTriggerListener(), GroupMatcher<TriggerKey>.AnyGroup());//添加全局监听
            scheduler.Start().Wait();
        }

        ///// <summary>
        ///// 初始化任务调度对象
        ///// </summary>
        //public static void InitScheduler()
        //{
        //    try
        //    {
        //        lock (lockObj)
        //        {
        //            if (scheduler != null)
        //            {
        //                throw new Exception("任务管理器已经初始化过");
        //            }

        //            scheduler = new StdSchedulerFactory().GetScheduler().Result;
        //            scheduler.ListenerManager.AddTriggerListener(new CustomTriggerListener(), GroupMatcher<TriggerKey>.AnyGroup());//添加全局监听
        //        }

        //        Log.Info("任务调度初始化成功");
        //    }
        //    catch
        //    {
        //        Log.Error("任务调度初始化失败");
        //        throw;
        //    }
        //}

        ///// <summary>
        ///// 初始化 远程Quartz服务器中的，各个Scheduler实例。
        ///// 提供给远程管理端的后台，用户获取Scheduler实例的信息。
        ///// </summary>
        //public static void InitRemoteScheduler(string remoteServer, int port)
        //{
        //    try
        //    {
        //        lock (lockObj)
        //        {
        //            if (scheduler != null)
        //            {
        //                throw new Exception("任务管理器已经初始化过");
        //            }

        //            NameValueCollection properties = new NameValueCollection();
        //            properties["quartz.scheduler.instanceName"] = "ExampleQuartzScheduler";
        //            properties["quartz.scheduler.proxy"] = "true";
        //            properties["quartz.scheduler.proxy.address"] = $"tcp://{remoteServer}:{port}/QuartzScheduler";

        //            scheduler = new StdSchedulerFactory(properties).GetScheduler().Result;
        //            scheduler.ListenerManager.AddTriggerListener(new CustomTriggerListener(), GroupMatcher<TriggerKey>.AnyGroup());//添加全局监听
        //        }

        //        Log.Info("远程任务调度初始化成功");
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error("初始化远程任务管理器失败", ex);
        //    }
        //}


        /// <summary>
        /// 启动任务调度器
        /// </summary>
        public static async Task Start()
        {
            try
            {
                if (!scheduler.IsStarted)
                {
                    await scheduler.Start();
                    Log.Info("启动任务调度器成功");
                }
            }
            catch
            {
                Log.Error("启动任务调度器失败");
                throw;
            }
        }

        /// <summary>
        /// 停止任务调度器
        /// </summary>
        public static async Task Stop()
        {
            try
            {
                if (!scheduler.IsShutdown)
                {
                    await scheduler.Shutdown();
                    Log.Info("停止任务调度器成功");
                }
            }
            catch
            {
                Log.Error("停止任务调度器失败");
                throw;
            }
        }

        /// <summary>
        /// 启用任务列表
        /// </summary>
        /// <param name="taskModelList">任务列表</param>
        /// <returns></returns>
        public static async Task ScheduleJobs(List<TaskModel> taskModelList)
        {
            try
            {
                foreach (TaskModel taskModel in taskModelList)
                {
                    await ScheduleJob(taskModel);
                }
                Log.Info("启用任务列表成功");
            }
            catch
            {
                Log.Error("启用任务列表失败");
                throw;
            }
        }

        /// <summary>
        /// 启用任务
        /// </summary>
        public static async Task ScheduleJob(TaskModel taskModel)
        {
            try
            {
                JobKey jobKey = new JobKey(taskModel.TaskID);
                if (await scheduler.CheckExists(jobKey))
                {
                    throw new Exception($"任务{taskModel.TaskID}已经存在，不能重复启用任务");
                }

                if (CronExpression.IsValidExpression(taskModel.CronExpressionString))//验证是否正确的Cron表达式
                {
                    IJobDetail job = new JobDetailImpl(taskModel.TaskID, GetClassTypeInfo(taskModel.AssemblyFullName, taskModel.ClassFullName));
                    CronTriggerImpl trigger = new CronTriggerImpl();
                    trigger.CronExpressionString = taskModel.CronExpressionString;
                    trigger.Name = taskModel.TaskID;
                    trigger.Description = taskModel.TaskName;
                    await scheduler.ScheduleJob(job, trigger);
                    if (taskModel.Status == 0)
                    {
                        JobKey jk = new JobKey(taskModel.TaskID);
                        await scheduler.PauseJob(jk);
                    }

                    Log.Info($"任务“{taskModel.TaskName}”启动成功,未来5次运行时间如下:");
                    List<DateTime> list = GetNextFireTime(taskModel.CronExpressionString, 5);
                    foreach (var time in list)
                    {
                        Log.Info(time.ToString());
                    }

                    Log.Info($"启用任务“{taskModel.TaskName}”成功");
                }
                else
                {
                    throw new Exception(taskModel.CronExpressionString + "不是正确的Cron表达式,无法启动该任务!");
                }
            }
            catch
            {
                Log.Error($"启用任务“{taskModel.TaskName}”失败");
                throw;
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
                    Log.Info($"删除任务“{jobKey}”成功");
                }
                else
                {
                    throw new Exception($"任务“{jobKey}”不存在,不能删除任务");
                }
            }
            catch
            {
                Log.Error($"删除任务“{jobKey}”失败");
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
                    Log.Info($"暂停任务“{jobKey}”成功");
                }
                else
                {
                    throw new Exception($"任务“{jobKey}”不存在,不能暂停任务");
                }
            }
            catch
            {
                Log.Error($"暂停任务“{jobKey}”失败");
                throw;
            }
        }

        /// <summary>
        /// 恢复任务
        /// </summary>
        /// <param name="jobKey">任务key</param>
        public static async Task ResumeJob(string jobKey)
        {
            try
            {
                JobKey jk = new JobKey(jobKey);
                if (await scheduler.CheckExists(jk))
                {
                    await scheduler.ResumeJob(jk);
                    Log.Info($"恢复任务“{jobKey}”成功");
                }
                else
                {
                    throw new Exception($"任务“{jobKey}”不存在,不能恢复任务");
                }
            }
            catch
            {
                Log.Error("恢复任务失败");
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
                string folderPath = AppContext.BaseDirectory + "Tasks" + Path.DirectorySeparatorChar;
                string filePath = folderPath + assemblyFullName;
                AssemblyLoader asl = new AssemblyLoader(folderPath);
                Assembly assembly = asl.LoadFromAssemblyPath(filePath);
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
        /// 立即运行一次任务
        /// </summary>
        /// <param name="jobKey">任务key</param>
        public static async Task RunOnceTask(string jobKey)
        {
            JobKey jk = new JobKey(jobKey);
            if (await scheduler.CheckExists(jk))
            {
                var jobDetail = await scheduler.GetJobDetail(jk);
                var triggers = await scheduler.GetTriggersOfJob(jk);
                string taskName = jobKey;
                if (triggers != null && triggers.Count > 0)
                {
                    taskName = triggers.First().Description;
                }
                var type = jobDetail.JobType;
                var instance = Activator.CreateInstance(type);
                var method = type.GetMethod("Execute");
                method.Invoke(instance, new object[] { null });
                Log.Info(string.Format("任务“{0}”立即运行", taskName));
            }
            else
            {
                throw new Exception($"任务{jobKey}不存在,不能立即运行一次任务");
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
            IReadOnlyList<DateTimeOffset> dates = TriggerUtils.ComputeFireTimes(trigger as IOperableTrigger, null, numTimes);
            List<DateTime> list = new List<DateTime>();
            foreach (DateTimeOffset dtf in dates)
            {
                list.Add(TimeZoneInfo.ConvertTime(dtf.DateTime, TimeZoneInfo.Local));
            }
            return list;
        }
    }
}
