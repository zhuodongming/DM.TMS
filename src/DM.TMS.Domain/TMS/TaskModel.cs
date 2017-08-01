using NPoco;
using System;
using System.Collections.Generic;
using System.Text;

namespace DM.TMS.Domain.TMS
{
    [TableName("Task")]
    [PrimaryKey("TaskID", AutoIncrement = false)]
    public class TaskModel
    {
        public string TaskID { get; set; }//任务ID

        public string TaskName { get; set; }//任务名称

        public string CronExpressionString { get; set; }//运行频率设置Cron表达式

        public string CronRemark { get; set; }//任务运频率中文说明

        public string AssemblyFullName { get; set; }//任务所在DLL对应的程序集名称

        public string ClassFullName { get; set; }//任务所在类

        public DateTime? LastRunTime { get; set; }//任务上一次运行时间

        public DateTime? NextRunTime { get; set; }//任务下一次运行时间

        public int Status { get; set; }//任务状态

        public int IsEnabled { get; set; }//是否启用

        public string Remark { get; set; }//备注

        public DateTime CreateTime { get; set; }//任务创建时间

        public DateTime? ModifyTime { get; set; }//任务修改时间
    }
}
