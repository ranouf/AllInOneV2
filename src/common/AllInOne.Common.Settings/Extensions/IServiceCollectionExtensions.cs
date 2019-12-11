using AllInOne.Common.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AllInOne.Common.Settings.Extensions
{
    public static class IServiceCollectionExtensions
    {
        const string settings = "Settings";
        public static IServiceCollection ConfigureAndValidate<T>(
            this IServiceCollection serviceCollection,
            IConfiguration config,
            string section = null
        ) where T : class
        {
            var configType = typeof(T).Name;
            if (string.IsNullOrEmpty(section))
            {
                section = configType;
                if (section.EndsWith(settings))
                {
                    section = section.Remove(configType.Length - settings.Length); // ex: ConfigurationSettings => Configuration
                }
            }

            return serviceCollection
                .Configure<T>(config.GetSection(section))
                .PostConfigure<T>(settings =>
                {
                    var configErrors = settings.ValidationErrors().ToArray();
                    if (configErrors.Any())
                    {
                        var aggrErrors = string.Join(",", configErrors);
                        var count = configErrors.Length;
                        throw new ApplicationException($"Found {count} configuration error(s) in {configType}: {aggrErrors}, settings:'{settings.ToJson()}'");
                    }
                });
        }

        private static IEnumerable<string> ValidationErrors(this object obj)
        {
            var context = new ValidationContext(obj, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(obj, context, results, true);
            foreach (var validationResult in results)
            {
                yield return validationResult.ErrorMessage;
            }
        }
    }
}
