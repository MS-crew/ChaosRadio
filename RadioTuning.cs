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
                return false;

            if (msg.SpeakerNull || msg.Speaker.netId != conn.identity.netId || !(msg.Speaker.roleManager.CurrentRole is IVoiceRole voiceRole) || !voiceRole.VoiceModule.CheckRateLimit() || VoiceChatMutes.IsMuted(msg.Speaker))
                return false;

            VoiceChatChannel voiceChatChannel = voiceRole.VoiceModule.ValidateSend(msg.Channel);
            if (voiceChatChannel == VoiceChatChannel.None)
                return false;

            bool oyuncuKaosTelsiziVar = false;
            bool hedefKaosTelsiziVar = false;

            if (ReferenceHub.TryGetHubNetID(conn.identity.netId, out ReferenceHub hub))
                oyuncuKaosTelsiziVar = hub.inventory.UserInventory.Items.Values.Any(item => KaosTelsiz.telsiz.Check(Item.Get(item)));

            voiceRole.VoiceModule.CurrentChannel = voiceChatChannel;
            foreach (ReferenceHub allHub in ReferenceHub.AllHubs)
            {
                if (allHub.roleManager.CurrentRole is IVoiceRole voiceRole2)
                {
                    VoiceChatChannel voiceChatChannel2 = voiceRole2.VoiceModule.ValidateReceive(msg.Speaker, voiceChatChannel);
                    hedefKaosTelsiziVar = allHub.inventory.UserInventory.Items.Values.Any(item => KaosTelsiz.telsiz.Check(Item.Get(item)));
                    if (voiceChatChannel2 == 0 || (voiceChatChannel2 == VoiceChatChannel.Radio && oyuncuKaosTelsiziVar != hedefKaosTelsiziVar))
                        continue;

                    msg.Channel = voiceChatChannel2;
                    allHub.connectionToClient.Send(msg);
                }
            }
            return false;
        }
        /*public static bool Prefix(NetworkConnection conn, VoiceMessage msg)
        {
            if (msg.Channel != VoiceChatChannel.Radio)
                return true;

            bool oyuncuKaosTelsiziVar = false;
            if (ReferenceHub.TryGetHubNetID(conn.identity.netId, out ReferenceHub hub))
                oyuncuKaosTelsiziVar = hub.inventory.UserInventory.Items.Values.Any(item => KaosTelsiz.telsiz.Check(Item.Get(item)));

            if (msg.SpeakerNull )//|| (msg.Speaker.netId != conn.identity.netId))
                return false;

            IVoiceRole speakerRole = msg.Speaker.roleManager.CurrentRole as IVoiceRole;
            if (speakerRole == null || !speakerRole.VoiceModule.CheckRateLimit() || VoiceChatMutes.IsMuted(msg.Speaker, false))
                return false;

            VoiceChatChannel validatedChannel = speakerRole.VoiceModule.ValidateSend(msg.Channel);
            if (validatedChannel == VoiceChatChannel.None)
                return false;

            speakerRole.VoiceModule.CurrentChannel = validatedChannel;
            foreach (ReferenceHub Hub in ReferenceHub.AllHubs)
            {
                if (Hub == msg.Speaker)
                    continue;

                IVoiceRole targetRole = Hub.roleManager.CurrentRole as IVoiceRole;
                if (targetRole == null)
                    continue;

                VoiceChatChannel targetChannel = targetRole.VoiceModule.ValidateReceive(msg.Speaker, validatedChannel);
                if (targetChannel == VoiceChatChannel.None)
                   continue;
                bool hedefKaosTelsiziVar = Hub.inventory.UserInventory.Items.Values.Any(item => KaosTelsiz.telsiz.Check(Item.Get(item)));
                if (targetChannel == VoiceChatChannel.Radio && oyuncuKaosTelsiziVar != hedefKaosTelsiziVar)
                    continue;

                msg.Channel = targetChannel;
                Hub.connectionToClient.Send(msg, 0);
            }
            return false;
        }*/
    }
}
