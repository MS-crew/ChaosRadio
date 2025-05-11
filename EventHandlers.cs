using System.Linq;
using Exiled.API.Features;
using Exiled.CustomRoles.API;
using Exiled.Events.EventArgs.Player;
using Exiled.CustomItems.API.Features;
using LabApi.Events.Arguments.PlayerEvents;

namespace ChaosRadio
{
    public class EventHandlers
    {
        public void OnSpawned(SpawnedEventArgs ev)
        {
            if (ev.Player.Role.Team != PlayerRoles.Team.ChaosInsurgency || !Plugin.Instance.Config.AddRadioinSpawn || !CustomItem.TryGet(Plugin.Instance.Config.ChaosRadio.Id , out CustomItem radio))
                return;

            if (ev.Player.GetCustomRoles() == null)
                radio.Give(ev.Player, false);
            else
                if (Plugin.Instance.Config.AddEvenCustomRole)
                    radio.Give(ev.Player, false);
        }

        public void OnPlayerReceivingVoiceMessage(PlayerReceivingVoiceMessageEventArgs ev)
        {
            if (ev.Message.Channel != VoiceChat.VoiceChatChannel.Radio)
                return;

            ev.IsAllowed = ev.Player.IshaveChaosRadio == ev.Sender.IshaveChaosRadio;
        }
    }
    public static class ReferenceHubExtension
    {
        public static bool IshaveChaosRadio(this LabApi.Features.Wrappers.Player player) => player.Inventory.UserInventory.Items.Keys.Any(i => CustomItem.Get(Plugin.Instance.Config.ChaosRadio.Id).TrackedSerials.Contains(i));
    }
}
