using System.Linq;
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
            if (ev.Player.Role.Team != PlayerRoles.Team.ChaosInsurgency || !Plugin.Instance.Config.AddRadioinSpawn || !CustomItem.Registered.Contains(Plugin.Instance.Config.ChaosRadio))
                return;

            if (ev.Player.GetCustomRoles() == null)
                CustomItem.TryGive(ev.Player, Plugin.Instance.Config.ChaosRadio.Name, false);
            else
                if (Plugin.Instance.Config.AddEvenCustomRole)
                    CustomItem.TryGive(ev.Player, Plugin.Instance.Config.ChaosRadio.Name, false);
        }

        public void OnPlayerReceivingVoiceMessage(PlayerReceivingVoiceMessageEventArgs ev)
        {
            if (ev.Message.Channel != VoiceChat.VoiceChatChannel.Radio)
                return;

            ReferenceHub hub = ReferenceHub.GetHub(ev.Message.Speaker);
            if (hub == null)
                return;

            ev.IsAllowed = ev.Player.ReferenceHub.IshaveChaosRadio == hub.IshaveChaosRadio;
        }
    }
    public static class ReferenceHubExtension
    {
        public static bool IshaveChaosRadio(this ReferenceHub player) => player.inventory.UserInventory.Items.Keys.Any(i => Plugin.Instance.Config.ChaosRadio.TrackedSerials.Contains(i));
    }
}
