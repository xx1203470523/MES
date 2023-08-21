using Hymson.EventBus.Abstractions;
using Hymson.MES.BackgroundServices.EventHandling;
using Hymson.MES.BackgroundTasks;
using Hymson.MES.BackgroundTasks.HostedServices;
using Hymson.MES.BackgroundTasks.Jobs;
using Hymson.MES.CoreServices.DependencyInjection;
using Hymson.MES.CoreServices.IntegrationEvents.Events.Messages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using Quartz;
using System.Reflection;

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
    LogManager.Flush();
    LogManager.Shutdown();
}

IHostBuilder CreateHostBuilder(string[] args) =>
Host.CreateDefaultBuilder(args)
   .ConfigureServices((Action<HostBuilderContext, IServiceCollection>)((hostContext, services) =>
   {
       //hostContext.Configuration.OnChange((configuration) =>
       //{
       //    NLog.LogManager.Configuration = new NLogLoggingConfiguration(configuration.GetSection("NLog"));
       //});
       AddEventBusServices(services);
       services.AddBackgroundServices(hostContext.Configuration);

       var mySqlConnection = hostContext.Configuration.GetSection("ConnectionOptions").GetValue<string>("HymsonQUARTZDB");
       // Add the required Quartz.NET services
       services.AddQuartz(q =>
       {
           // Use a Scoped container to create jobs. I'll touch on this later
           q.UseMicrosoftDependencyInjectionJobFactory();
           #region jobs

           q.AddJobAndTrigger<MessagePushJob>(hostContext.Configuration);
           //q.AddJobAndTrigger<HelloWorld2Job>(hostContext.Configuration);

           #endregion
           q.UsePersistentStore((persistentStoreOptions) =>
           {
               persistentStoreOptions.UseProperties = true;
               persistentStoreOptions.UseClustering();
               persistentStoreOptions.SetProperty("quartz.serializer.type", "json");
               persistentStoreOptions.SetProperty("quartz.jobStore.type", "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz");
               string assemblyName = Assembly.GetExecutingAssembly().GetName().Name ?? "Hymson.MES.BackgroundTasks";
               persistentStoreOptions.SetProperty("quartz.scheduler.instanceName", assemblyName);
               persistentStoreOptions.SetProperty("quartz.scheduler.instanceId", assemblyName);
               persistentStoreOptions.UseMySql(mySqlConnection);
           });
       });

       services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
       services.AddHostedService<SubHostedService>();

       services.AddNLog(hostContext.Configuration);
       services.AddEventBusRabbitMQService(hostContext.Configuration);



   }));



static void AddEventBusServices(IServiceCollection services)
{
    services.AddSingleton<IIntegrationEventHandler<MessageProcessingUpgradeEvent>, MessageProcessingUpgradeEventHandler>();
    services.AddSingleton<IIntegrationEventHandler<MessageReceiveUpgradeEvent>, MessageReceiveUpgradeEventHandler>();
    services.AddSingleton<IIntegrationEventHandler<MessageTriggerUpgradeEvent>, MessageTriggerUpgradeEventHandler>();
}