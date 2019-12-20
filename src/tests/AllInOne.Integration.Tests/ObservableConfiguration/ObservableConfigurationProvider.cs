using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace AllInOne.Integration.Tests.ObservableConfiguration
{
    public class ObservableConfigurationProvider : ConfigurationProvider
    {
        /// <summary>
        /// The source settings for this provider.
        /// </summary>
        public ObservableConfigurationSource Source { get; }

        public ObservableConfigurationProvider([NotNull]ObservableConfigurationSource source)
        {
            Source = source ?? throw new ArgumentNullException(nameof(source));

            if (Source.InitialData != null && Source.InitialData.Value != null)
            {
                Source.InitialData.Subscribe(data =>
                {
                    Load(data);
                });
            }
        }

        private void Load(IEnumerable<KeyValuePair<string, string>> data)
        {
            foreach (var pair in data)
            {
                Data.Add(pair.Key, pair.Value);
            }
        }
    }
}
