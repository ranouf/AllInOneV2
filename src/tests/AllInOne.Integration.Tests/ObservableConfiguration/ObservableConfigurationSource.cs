using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Reactive.Subjects;

namespace AllInOne.Integration.Tests.ObservableConfiguration
{
    public class ObservableConfigurationSource : IConfigurationSource
    {
        public BehaviorSubject<IEnumerable<KeyValuePair<string, string>>> InitialData { get; set; }
        public ObservableConfigurationProvider ObservableConfigurationProvider { get; set; }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new ObservableConfigurationProvider(this);
        }
    }
}