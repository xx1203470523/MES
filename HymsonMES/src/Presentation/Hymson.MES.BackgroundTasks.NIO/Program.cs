using AutoMapper;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.BackgroundTasks.NIO;
using Hymson.MES.BackgroundTasks.NIO.Extensions;
using Hymson.MES.BackgroundTasks.NIO.HostedServices;
using Hymson.MES.CoreServices.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using Quartz;

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
   .ConfigureServices((hostContext, services) =>
   {
       //services.Configure<PrintOptions>(hostContext.Configuration.GetSection(nameof(PrintOptions)));
       services.AddLocalization();

       services.AddSqlLocalization(hostContext.Configuration);
       services.AddBackgroundServices(hostContext.Configuration);
       services.AddMemoryCache();
       //services.AddPrintBackgroundService(hostContext.Configuration);
       services.AddClearCacheService(hostContext.Configuration);
       //services.AddPrintService(hostContext.Configuration);

       var mySqlConnection = hostContext.Configuration.GetSection("ConnectionOptions").GetValue<string>("HymsonQUARTZDB");
       var programName = hostContext.Configuration.GetSection("Quartz").GetValue<string>("ProgramName");
       // Add the required Quartz.NET services
       services.AddQuartz(q =>
       {
           // Use a Scoped container to create jobs. I'll touch on this later
           q.UseMicrosoftDependencyInjectionJobFactory();

           #region jobs
           /*
           q.AddJobAndTrigger<BuzAttachmentJob>(hostContext.Configuration);
           q.AddJobAndTrigger<BuzCollectionJob>(hostContext.Configuration);
           q.AddJobAndTrigger<BuzCommonJob>(hostContext.Configuration);
           q.AddJobAndTrigger<BuzDataEnvJob>(hostContext.Configuration);
           q.AddJobAndTrigger<BuzIssueJob>(hostContext.Configuration);
           q.AddJobAndTrigger<BuzMaterialJob>(hostContext.Configuration);
           q.AddJobAndTrigger<BuzPassrateProductJob>(hostContext.Configuration);
           q.AddJobAndTrigger<BuzPassrateStationJob>(hostContext.Configuration);
           q.AddJobAndTrigger<BuzProductionJob>(hostContext.Configuration);
           q.AddJobAndTrigger<BuzWorkOrderJob>(hostContext.Configuration);
           */

           //q.AddJobAndTrigger<MasterEnvFieldJob>(hostContext.Configuration);
           //q.AddJobAndTrigger<MasterFieldJob>(hostContext.Configuration);
           //q.AddJobAndTrigger<MasterPassrateTargetJob>(hostContext.Configuration);
           //q.AddJobAndTrigger<MasterPersonCertJob>(hostContext.Configuration);
           q.AddJobAndTrigger<MasterProductJob>(hostContext.Configuration);
           q.AddJobAndTrigger<MasterStationJob>(hostContext.Configuration);
           //q.AddJobAndTrigger<MasterTeamSchedulingJob>(hostContext.Configuration);

           //q.AddJobAndTrigger<MockHelloJob>(hostContext.Configuration);
           q.AddJobAndTrigger<PushNIOJob>(hostContext.Configuration);
           #endregion

           /*
           q.UsePersistentStore((persistentStoreOptions) =>
           {
               persistentStoreOptions.UseProperties = true;
               persistentStoreOptions.UseClustering();
               persistentStoreOptions.SetProperty("quartz.serializer.type", "json");
               persistentStoreOptions.SetProperty("quartz.jobStore.type", "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz");
               string assemblyName = Assembly.GetExecutingAssembly().GetName().Name ?? "Hymson.MES.BackgroundTasks.NIO";
               persistentStoreOptions.SetProperty("quartz.scheduler.instanceName", assemblyName + hostContext.HostingEnvironment.EnvironmentName + programName);
               persistentStoreOptions.SetProperty("quartz.scheduler.instanceId", assemblyName + hostContext.HostingEnvironment.EnvironmentName + programName);
               persistentStoreOptions.UseMySql(mySqlConnection);
           });
           */

       });

       services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
       services.AddHostedService<SubHostedService>();
       //services.AddSqlExecuteTaskService(hostContext.Configuration);
       services.AddNLog(hostContext.Configuration);
       //services.AddEventBusRabbitMQService(hostContext.Configuration);
       AddAutoMapper();

   });

static void AddAutoMapper()
{
    //find mapper configurations provided by other assemblies
    var typeFinder = Singleton<ITypeFinder>.Instance;
    var mapperConfigurations = typeFinder.FindClassesOfType<IOrderedMapperProfile>();

    //create and sort instances of mapper configurations
    var instances = mapperConfigurations
        .Select(mapperConfiguration => (IOrderedMapperProfile)Activator.CreateInstance(mapperConfiguration))
        .OrderBy(mapperConfiguration => mapperConfiguration.Order);

    //create AutoMapper configuration
    var config = new MapperConfiguration(cfg =>
    {
        foreach (var instance in instances)
        {
            cfg.AddProfile(instance.GetType());
        }
    });

    //register
    AutoMapperConfiguration.Init(config);
}

