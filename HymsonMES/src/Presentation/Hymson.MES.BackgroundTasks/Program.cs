using Hymson.MES.BackgroundTasks;
using Hymson.MES.BackgroundTasks.Jobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using Quartz;
using System.Reflection;
using System.Runtime.Caching;

try
{
    CreateHostBuilder(args).Build().Run();
    return 0;
}
catch (Exception ex)
{
    Console.WriteLine(ex);
    return 1;
}
finally
{
    LogManager.Shutdown();
}

IHostBuilder CreateHostBuilder(string[] args) =>
Host.CreateDefaultBuilder(args)
   .ConfigureServices((hostContext, services) =>
   {
       //hostContext.Configuration.OnChange((configuration) =>
       //{
       //    NLog.LogManager.Configuration = new NLogLoggingConfiguration(configuration.GetSection("NLog"));
       //});

       var mySqlConnection = hostContext.Configuration.GetSection("ConnectionOptions").GetValue<string>("HymsonQUARTZDB");

       services.AddMemoryCache();
       services.AddSequenceService(hostContext.Configuration);
       //添加后台服务Service
       services.AddBackgroundServices(hostContext.Configuration);

       // Add the required Quartz.NET services
       services.AddQuartz(q =>
       {
           // Use a Scoped container to create jobs. I'll touch on this later
           q.UseMicrosoftDependencyInjectionJobFactory();
           #region jobs

           //q.AddJobAndTrigger<HelloWorld1Job>(hostContext.Configuration);
           //q.AddJobAndTrigger<HelloWorld2Job>(hostContext.Configuration);
           q.AddJobAndTrigger<EquHeartbeatJob>(hostContext.Configuration);
           q.AddJobAndTrigger<EquHeartBeatRecordJob>(hostContext.Configuration);

           #endregion
           q.UsePersistentStore((persistentStoreOptions) =>
           {
               persistentStoreOptions.UseProperties = true;
               persistentStoreOptions.UseClustering();
               persistentStoreOptions.SetProperty("quartz.serializer.type", "json");
               persistentStoreOptions.SetProperty("quartz.jobStore.type", "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz");
               string assemblyName = Assembly.GetExecutingAssembly().GetName().Name ?? "Hymson.MES.BackgroundTasks";
               persistentStoreOptions.SetProperty("quartz.scheduler.instanceName", assemblyName+ hostContext.HostingEnvironment.EnvironmentName);
               persistentStoreOptions.SetProperty("quartz.scheduler.instanceId", assemblyName + hostContext.HostingEnvironment.EnvironmentName);
               persistentStoreOptions.UseMySql(mySqlConnection);
           });
       });

       services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);


       services.AddNLog(hostContext.Configuration);


   });