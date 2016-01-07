using EncoOrszag.Tasks;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EncoOrszag.App_Start
{
    public class TaskConfig
    {
        public static void Init()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();

            scheduler.Start();

            IJobDetail job = JobBuilder.Create<KorvaltasTask>().WithIdentity("korvaltas", "default").Build();

            ITrigger trigger = TriggerBuilder.Create().StartNow().WithSimpleSchedule(x => x.WithIntervalInHours(1).RepeatForever()).Build();

            scheduler.ScheduleJob(job, trigger);
                
        }
    }
}