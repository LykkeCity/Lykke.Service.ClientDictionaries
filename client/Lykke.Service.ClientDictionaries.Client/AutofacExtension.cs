using System;
using Autofac;
using Common.Log;

namespace Lykke.Service.ClientDictionaries.Client
{
    public static class AutofacExtension
    {
        public static void RegisterClientDictionariesClient(this ContainerBuilder builder, string serviceUrl, ILog log)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (serviceUrl == null) throw new ArgumentNullException(nameof(serviceUrl));
            if (log == null) throw new ArgumentNullException(nameof(log));
            if (string.IsNullOrWhiteSpace(serviceUrl))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(serviceUrl));

            builder.RegisterType<ClientDictionariesClient>()
                .WithParameter("serviceUrl", serviceUrl)
                .As<IClientDictionariesClient>()
                .SingleInstance();
        }

        public static void RegisterClientDictionariesClient(this ContainerBuilder builder, ClientDictionariesServiceClientSettings settings, ILog log)
        {
            builder.RegisterClientDictionariesClient(settings?.ServiceUrl, log);
        }
    }
}
