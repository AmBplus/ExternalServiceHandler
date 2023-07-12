//using Microsoft.Extensions.Options;

//using Microsoft.Extensions.Hosting;
//using ExternalHandler.Services;
//using ExternalHandler.Settings;
//using System.Text.Json;
//using System.Text.Json.Nodes;

//namespace ExternalHandler.Services
//{

//    public class WritableOptions<T> : IWritableOptions<T> where T : class, new()
//    {
//        private readonly IWebHostEnvironment _environment;
//        private readonly IOptionsMonitor<T> _options;
//        private readonly IConfigurationRoot _configuration;
//        private readonly string _section;
//        private readonly string _file;

//        public WritableOptions(
//            IWebHostEnvironment environment,
//            IOptionsMonitor<T> options,
//            IConfigurationRoot configuration,
//            string section,
//            string file)
//        {
//            _environment = environment;
//            _options = options;
//            _configuration = configuration;
//            _section = section;
//            _file = file;
//        }

//        public T Value => _options.CurrentValue;
//        public T Get(string name) => _options.Get(name);

//        public void Update(Action<T> applyChanges)
//        {
//            var fileProvider = _environment.ContentRootFileProvider;
//            var fileInfo = fileProvider.GetFileInfo(_file);
//            var physicalPath = fileInfo.PhysicalPath;

//            var jObject = JsonSerializer.Deserialize<JsonObject>(File.ReadAllText(physicalPath));
//            var sectionObject = jObject.TryGetValue(_section, out  section) ?
//                JsonConvert.DeserializeObject<T>(section.ToString()) : Value ?? new T();

//            applyChanges(sectionObject);

//            jObject[_section] = JObject.Parse(JsonConvert.SerializeObject(sectionObject));
//            File.WriteAllText(physicalPath, JsonConvert.SerializeObject(jObject, Formatting.Indented));
//            _configuration.Reload();
//        }
//    }
//    public static class ServiceCollectionExtensions
//    {
//        public static void ConfigureWritable<T>(
//            this IServiceCollection services,
//            IConfigurationSection section,
//            string file = "appsettings.json") where T : class, new()
//        {
//            services.Configure<T>(section);
//            services.AddTransient<IWritableOptions<T>>(provider =>
//            {
//                var configuration = (IConfigurationRoot)provider.GetService<IConfiguration>();
//                var environment = provider.GetService<IWebHostEnvironment>();
//                var options = provider.GetService<IOptionsMonitor<T>>();
//                return new WritableOptions<T>(environment, options, configuration, section.Key, file);
//            });
//        }
//    }

//}
