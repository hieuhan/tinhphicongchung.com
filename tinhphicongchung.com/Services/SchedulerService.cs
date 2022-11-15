using Quartz.Impl;
using Quartz;
using System;
using System.Threading.Tasks;
using tinhphicongchung.com.helper;

namespace tinhphicongchung.com.Services
{
    public class SchedulerService
    {
        public static async Task StartAsync()
        {
			try
			{
                var scheduler = await StdSchedulerFactory.GetDefaultScheduler();

                if (!scheduler.IsStarted)
                {
                    await scheduler.Start();
                }

                var job = JobBuilder.Create<TaskService>()
                    .WithIdentity("ExecuteTaskServiceCallJob1", "group1")
                    .Build();

                var trigger = TriggerBuilder.Create()
                    .WithIdentity("ExecuteTaskServiceCallTrigger1", "group1")
                    .StartNow()
                    .WithSimpleSchedule(x => x
                    .WithIntervalInMinutes(2)
                    .RepeatForever())
                    .Build();

                await scheduler.ScheduleJob(job, trigger);
            }
			catch (Exception ex)
			{
                LogHelper.Error("SchedulerService > StartAsync", ex);
            }
        }
    }
}