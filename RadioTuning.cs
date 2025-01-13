using Mirror;
using VoiceChat;
using HarmonyLib;
using System.Linq;
using PlayerRoles.Voice;
using Exiled.API.Features;
using VoiceChat.Networking;
using System.Collections.Generic;

namespace ChaosRadio
{
    [HarmonyPatch(typeof(VoiceTransceiver), nameof(VoiceTransceiver.ServerReceiveMessage))]
    public static class Voice
    {
        public static bool Prefix(NetworkConnection conn, VoiceMessage msg)
        {
            if (msg.Channel != VoiceChatChannel.Radio)
                return true;

            Player player = Player.Get(conn);
            bool kaosmu = player.Items.Any(item => KaosTelsiz.telsiz.Check(item));

            if (msg.SpeakerNull || msg.Speaker.netId != conn.identity.netId)
                return false;

            IVoiceRole voiceRole = msg.Speaker.roleManager.CurrentRole as IVoiceRole;
            if (voiceRole == null || !voiceRole.VoiceModule.CheckRateLimit() || VoiceChatMutes.IsMuted(msg.Speaker, false))
                return false;

            VoiceChatChannel voiceChatChannel = voiceRole.VoiceModule.ValidateSend(msg.Channel);
            if (voiceChatChannel == VoiceChatChannel.None)
                return false;

            voiceRole.VoiceModule.CurrentChannel = voiceChatChannel;
            List<Player> RadyoyaGore = Player.List.Where(p => p.Items.Any(item => KaosTelsiz.telsiz.Check(item)) == kaosmu).ToList();
            foreach (Player target in RadyoyaGore)
            {
                IVoiceRole voiceRole2 = target.ReferenceHub.roleManager.CurrentRole as IVoiceRole;
                if (voiceRole2 != null)
                {
                    VoiceChatChannel voiceChatChannel2 = voiceRole2.VoiceModule.ValidateReceive(msg.Speaker, voiceChatChannel);
                    if (voiceChatChannel2 != VoiceChatChannel.None)
                    {
                        msg.Channel = voiceChatChannel2;
                        target.ReferenceHub.connectionToClient.Send<VoiceMessage>(msg, 0);
                    }
                }
            }
            return false;
        }
    }
}
