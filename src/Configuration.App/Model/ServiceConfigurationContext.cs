﻿namespace Microsoft.Extensions.DependencyInjection
{
    using System.Linq;
    using Microsoft.Extensions.Configuration;
    using Naos.Core.Common;

    public class ServiceConfigurationContext
    {
        public IServiceCollection Services { get; set; }

        public Naos.Core.Common.ServiceDescriptor Descriptor { get; set; }

        public IConfiguration Configuration { get; set; }

        public ServiceConfigurationContext AddTag(string tag)
        {
            this.Descriptor = this.Descriptor ?? new Naos.Core.Common.ServiceDescriptor();
            this.Descriptor.Tags = this.Descriptor.Tags.Insert(tag).ToArray();
            return this;
        }
    }
}
