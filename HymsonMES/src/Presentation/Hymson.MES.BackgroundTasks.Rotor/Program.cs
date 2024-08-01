using AutoMapper;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.BackgroundTasks.Rotor.Extensions;
using Hymson.MES.BackgroundTasks.Rotor.HostedServices;
using Hymson.MES.BackgroundTasks.Rotor.Jobs;
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

           q.AddJobAndTrigger<ManuDataJob>(hostContext.Configuration);
           q.AddJobAndTrigger<PackListJob>(hostContext.Configuration);

           /*
           q.UsePersistentStore((persistentStoreOptions) =>
           {
               persistentStoreOptions.UseProperties = true;
               persistentStoreOptions.UseClustering();
               persistentStoreOptions.SetProperty("quartz.serializer.type", "json");
               persistentStoreOptions.SetProperty("quartz.jobStore.type", "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz");
               string assemblyName = Assembly.GetExecutingAssembly().GetName().Name ?? "Hymson.MES.BackgroundTasks.Rotor";
               persistentStoreOptions.SetProperty("quartz.scheduler.instanceName", assemblyName + hostContext.HostingEnvironment.EnvironmentName + programName);
               persistentStoreOptions.SetProperty("quartz.scheduler.instanceId", assemblyName + hostContext.HostingEnvironment.EnvironmentName + programName);
               persistentStoreOptions.UseMySql(mySqlConnection);
           });
           */
       });

       services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
       //services.AddHostedService<SubHostedService>();
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

