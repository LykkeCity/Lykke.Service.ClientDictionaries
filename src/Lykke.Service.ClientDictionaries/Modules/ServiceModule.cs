using Autofac;
using Autofac.Extensions.DependencyInjection;
using AzureStorage.Blob;
using AzureStorage.Tables;
using AzureStorage.Tables.Templates.Index;
using Common.Log;
using Lykke.Service.ClientDictionaries.AzureRepositories.ClientDictionaryBlob;
using Lykke.Service.ClientDictionaries.AzureRepositories.ClientKeysToBlobKeys;
using Lykke.Service.ClientDictionaries.Core.Services;
using Lykke.Service.ClientDictionaries.Settings.ServiceSettings;
using Lykke.Service.ClientDictionaries.Services;
using Lykke.SettingsReader;
using Microsoft.Extensions.DependencyInjection;

namespace Lykke.Service.ClientDictionaries.Modules
{
    public class ServiceModule : Module
    {
        private readonly IReloadingManager<ClientDictionariesSettings> _settings;
        private readonly ILog _log;
        private readonly IServiceCollection _services;

        public ServiceModule(IReloadingManager<ClientDictionariesSettings> settings, ILog log)
        {
            _settings = settings;
            _log = log;

            _services = new ServiceCollection();
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(_log)
                .As<ILog>()
                .SingleInstance();

            builder.RegisterType<HealthService>()
                .As<IHealthService>()
                .SingleInstance();

            builder.RegisterType<StartupManager>()
                .As<IStartupManager>();

            builder.RegisterType<ShutdownManager>()
                .As<IShutdownManager>();

            builder.RegisterType<InputValidator>()
                .As<IInputValidator>()
                .SingleInstance()
                .WithParameter(TypedParameter.From(_settings.CurrentValue.MaxPayloadSizeBytes));

            builder.Register(ctx => new ClientDictionaryBlob(
                    AzureBlobStorage.Create(
                        _settings.ConnectionString(x => x.Db.DataConnString)),
                    new ClientKeysToBlobKeys(
                        AzureTableStorage<ClientKeysToBlobKeyEntity>.Create(
                            _settings.ConnectionString(x => x.Db.DataConnString),
                            ClientKeysToBlobKeys.TableName,
                            _log))))
                .As<IClientDictionary>()
                .SingleInstance();

            builder.Populate(_services);
        }
    }
}
