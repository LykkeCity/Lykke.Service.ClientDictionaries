using Lykke.Service.ClientDictionaries.Settings.ServiceSettings;
using Lykke.Service.ClientDictionaries.Settings.SlackNotifications;

namespace Lykke.Service.ClientDictionaries.Settings
{
    public class AppSettings
    {
        public ClientDictionariesSettings ClientDictionariesService { get; set; }
        public SlackNotificationsSettings SlackNotifications { get; set; }
    }
}
