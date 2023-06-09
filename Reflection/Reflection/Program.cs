using Microsoft.Extensions.FileProviders;
using System;
using System.Reflection;

namespace Reflection
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var fileConfig = "ConfigurationProviders.FileConfigurationProvider";
            var configManager = "ConfigurationProviders.ConfigurationManagerConfigurationProvider";
            var filePath = @".\config.txt";

            var providers = LoadProviders();
            FileProvider(providers[0], fileConfig, filePath);
            ConfigurationManagementProvider(providers[0], configManager);

            static Assembly[] LoadProviders()
            {
                var providers = new List<Assembly>();

                foreach(var file in Directory.GetFiles(@".\Plugins", "*.dll"))
                {
                    var asm = Assembly.LoadFrom(Directory.GetCurrentDirectory() + file);
                    providers.Add(asm);
                }
                return providers.ToArray();
            }

            static void FileProvider(Assembly fileProvider, string fileConfig, string filePath)
            {
                Type? type = fileProvider.GetType(fileConfig);

                MethodInfo? getMethod = type?.GetMethod("GetValue");
                object[] args1 = new object[] { "Lambdas" };

                object? instance = Activator.CreateInstance(type, filePath);
                Console.WriteLine(getMethod?.Invoke(instance, args1));
            }

            static void ConfigurationManagementProvider(Assembly configProvider, string configManager)
            {
                Type? type = configProvider.GetType(configManager);

                MethodInfo? getMethod = type?.GetMethod("GetValue");
                object[] args1 = new object[] { "AwsS3BucketName" };

                object? instance = Activator.CreateInstance(type);
                Console.WriteLine(getMethod?.Invoke(instance, args1));
            }
        }
        }
    }