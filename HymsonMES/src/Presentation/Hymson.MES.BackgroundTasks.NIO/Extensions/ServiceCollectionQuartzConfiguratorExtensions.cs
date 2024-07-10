using Microsoft.Extensions.Configuration;
using Quartz;
using System.Reflection;

namespace Hymson.MES.BackgroundTasks.NIO.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class ServiceCollectionQuartzConfiguratorExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="quartz"></param>
        /// <param name="config"></param>
        /// <exception cref="Exception"></exception>
        public static void AddJobAndTrigger<T>(this IServiceCollectionQuartzConfigurator quartz, IConfiguration config)
        where T : IJob
        {

            // Use the name of the IJob as the appsettings.json key
            string jobName = typeof(T).Name;
            string assemblyName = Assembly.GetExecutingAssembly().GetName().Name ?? "Hymson.MES.BackgroundTasks.NIO";
            // Try and load the schedule from configuration
            var configKey = $"Quartz:{jobName}";
            var cronSchedule = config[configKey];

            // Some minor validation
            if (string.IsNullOrEmpty(cronSchedule))
            {
                throw new Exception($"No Quartz.NET Cron schedule found for job in configuration at {configKey}");
            }

            // register the job as before
            var jobKey = new JobKey(jobName, assemblyName);
            quartz.AddJob<T>(opts =>
            {
                opts.WithIdentity(jobKey);
                opts.WithDescription(jobName);
            });

            quartz.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity(jobName + "-Trigger", assemblyName)
                .WithCronSchedule(cronSchedule)); // use the schedule from configuration
        }

    }

}
