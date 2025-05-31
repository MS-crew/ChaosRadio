using Exiled.CustomRoles.API;
using System.Collections.Generic;
using Exiled.Events.EventArgs.Player;
using Exiled.CustomItems.API.Features;
using LabApi.Events.Arguments.PlayerEvents;
using Player = LabApi.Features.Wrappers.Player;
using VoiceChat;

namespace ChaosRadio
{
    public class EventHandlers
    {
        public void OnSpawned(SpawnedEventArgs ev)
        {
            if (ev.Player.Role.Team != PlayerRoles.Team.ChaosInsurgency || !Plugin.Instance.Config.AddRadioinSpawn || !CustomItem.Registered.Contains(KaosTelsiz.chaosradio))
                return;

            if (ev.Player.GetCustomRoles() == null)
                KaosTelsiz.chaosradio.Give(ev.Player, false);
            else
                if (Plugin.Instance.Config.AddEvenCustomRole)
                    KaosTelsiz.chaosradio.Give(ev.Player, false);
        }

        public void OnPlayerReceivingVoiceMessage(PlayerReceivingVoiceMessageEventArgs ev)
        {
            if (!ev.IsAllowed)
                return;

            if (ev.Message.Channel != VoiceChatChannel.Radio)
                return;

            if (ev.Player.IshaveChaosRadio() == ev.Sender.IshaveChaosRadio())
                return;

            ev.Message.Channel = VoiceChatChannel.Proximity;
        }
    }
    public static class ReferenceHubExtension
    {
        public static bool IshaveChaosRadio(this Player player)
        {
            HashSet<int> serials = KaosTelsiz.chaosradio.TrackedSerials;
            foreach (ushort itemSerial in player.Inventory.UserInventory.Items.Keys)
            {
                if (serials.Contains(itemSerial))
                    return true;
            }

            return false;
        }
    }
}
