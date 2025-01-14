using Mirror;
using VoiceChat;
using HarmonyLib;
using System.Linq;
using UnityEngine;
using PlayerRoles.Voice;
using Exiled.API.Features;
using VoiceChat.Networking;
using Exiled.API.Features.Items;
using PlayerRoles.FirstPersonControl;

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
            bool oyuncuKaosTelsiziVar = player.Items.Any(item => KaosTelsiz.telsiz.Check(item));

            if (msg.SpeakerNull || msg.Speaker.netId != conn.identity.netId)
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

                bool hedefKaosTelsiziVar = Hub.inventory.UserInventory.Items.Values.Any(item => KaosTelsiz.telsiz.Check(Item.Get(item)));
                if (oyuncuKaosTelsiziVar == hedefKaosTelsiziVar)
                {
                    VoiceChatChannel targetChannel = targetRole.VoiceModule.ValidateReceive(msg.Speaker, validatedChannel);
                    if (targetChannel == VoiceChatChannel.None)
                        continue;

                    msg.Channel = targetChannel;
                    Hub.connectionToClient.Send(msg, 0);
                }
                else
                {
                    if (Vector3.Distance(Hub.GetPosition(), player.Position) >= 3f)
                        continue;

                    VoiceChatChannel targetChannel = targetRole.VoiceModule.ValidateReceive(msg.Speaker, VoiceChatChannel.Proximity);
                    if (targetChannel == VoiceChatChannel.None)
                        continue;

                    msg.Channel = targetChannel;
                    Hub.connectionToClient.Send(msg, 0);
                }
            }
            return false;
        }
    }
}
