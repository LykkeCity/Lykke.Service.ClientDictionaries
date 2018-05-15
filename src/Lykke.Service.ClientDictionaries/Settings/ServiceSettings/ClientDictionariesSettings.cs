using JetBrains.Annotations;

namespace Lykke.Service.ClientDictionaries.Settings.ServiceSettings
{
    [UsedImplicitly]
    public class ClientDictionariesSettings
    {
        public DbSettings Db { get; set; }
        public int MaxPayloadSizeBytes { get; set; }
    }
}
