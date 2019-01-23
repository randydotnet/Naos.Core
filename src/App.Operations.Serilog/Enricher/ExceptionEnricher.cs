﻿namespace Naos.Core.Commands.Operations.Serilog
{
    using System.Diagnostics;
    using global::Serilog.Core;
    using global::Serilog.Events;

    public class ExceptionEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.Exception?.Demystify();
        }
    }
}
