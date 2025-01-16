using Mirror;
using VoiceChat;
using HarmonyLib;
using System.Linq;
using PlayerRoles.Voice;
using VoiceChat.Networking;
using Exiled.API.Features.Items;

namespace ChaosRadio
{
    [HarmonyPatch(typeof(VoiceTransceiver), nameof(VoiceTransceiver.ServerReceiveMessage))]
    public static class Voice
    {
        public static bool Prefix(NetworkConnection conn, VoiceMessage msg)
        {
            if (msg.Channel != VoiceChatChannel.Radio)
                return true;

            if (msg.SpeakerNull || msg.Speaker.netId != conn.identity.netId || !(msg.Speaker.roleManager.CurrentRole is IVoiceRole voiceRole) || !voiceRole.VoiceModule.CheckRateLimit() || VoiceChatMutes.IsMuted(msg.Speaker))
                return false;

            VoiceChatChannel voiceChatChannel = voiceRole.VoiceModule.ValidateSend(msg.Channel);
            if (voiceChatChannel == VoiceChatChannel.None)
                return false;

            bool oyuncuKaosTelsiziVar = false;
            if (ReferenceHub.TryGetHubNetID(conn.identity.netId, out ReferenceHub hub))
                oyuncuKaosTelsiziVar = hub.inventory.UserInventory.Items.Values.Any(item => KaosTelsiz.telsiz.Check(Item.Get(item)));

            voiceRole.VoiceModule.CurrentChannel = voiceChatChannel;
            foreach (ReferenceHub allHub in ReferenceHub.AllHubs)
            {
                if (allHub.roleManager.CurrentRole is not IVoiceRole voiceRole2)
                    continue;

                VoiceChatChannel voiceChatChannel2 = voiceRole2.VoiceModule.ValidateReceive(msg.Speaker, voiceChatChannel);
                if (voiceChatChannel2 == 0)
                    continue;

                if (voiceChatChannel2 == VoiceChatChannel.Radio && (oyuncuKaosTelsiziVar != allHub.inventory.UserInventory.Items.Values.Any(item => KaosTelsiz.telsiz.Check(Item.Get(item)))))
                    continue;

                msg.Channel = voiceChatChannel2;
                allHub.connectionToClient.Send(msg);
            }
            return false;
        }
    }
}
