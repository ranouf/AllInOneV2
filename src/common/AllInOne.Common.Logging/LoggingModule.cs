using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace AllInOne.Common.Logging
{
    public class LoggingModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(LoggerService<>)).As(typeof(ILoggerService<>));
        }
    }
}
