using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Reactive.Subjects;

namespace AllInOne.Integration.Tests.ObservableConfiguration
{
    public static class ObservableConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddObservableConfiguration(
            this IConfigurationBuilder configurationBuilder,
            BehaviorSubject<IEnumerable<KeyValuePair<string, string>>> initialData)
        {
            if (configurationBuilder == null)
            {
                throw new ArgumentNullException(nameof(configurationBuilder));
            }

            configurationBuilder.Add(new ObservableConfigurationSource { InitialData = initialData });
            return configurationBuilder;
        }
    }
}
