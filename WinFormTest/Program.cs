using MongoDB.Bson.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Silmoon.Data.MongoDB.Converters;
using Silmoon.Data.MongoDB.Serializer;

namespace WinFormTest
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new MongoDBTestForm());
            //Application.Run(new SqlServerTestForm());
        }
        public static void JsonTest()
        {
            JsonConvert.DefaultSettings = new Func<JsonSerializerSettings>(() =>
            {
                var settings = new JsonSerializerSettings();
                settings.Converters.Add(new ObjectIdJsonConverter());
                return settings;
            });
            BsonSerializer.RegisterSerializer(typeof(JObject), new JObjectBsonDocumentConvertSerializer());
        }
    }
}