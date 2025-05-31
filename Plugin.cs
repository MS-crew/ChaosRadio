using System;
using Exiled.API.Features;
using Exiled.CustomItems.API.Features;
using P = Exiled.Events.Handlers.Player;
using Ply = LabApi.Events.Handlers.PlayerEvents;

namespace ChaosRadio
{
    public class Plugin : Plugin<Config>
    {
        public static Plugin Instance { get; private set; }

        public static EventHandlers eventHandlers;

        public override string Author => "ZurnaSever";

        public override string Name => "ChaosRadio";

        public override string Prefix => "ChaosRadio";

        public override Version RequiredExiledVersion { get; } = new Version(9, 6, 0);

        public override Version Version { get; } = new Version(1, 7, 2);

        public override void OnEnabled()
        {
            Instance = this; 
            eventHandlers = new EventHandlers();

            CustomItem.RegisterItems();

            P.Spawned += eventHandlers.OnSpawned;
            Ply.ReceivingVoiceMessage += eventHandlers.OnPlayerReceivingVoiceMessage;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            P.Spawned -= eventHandlers.OnSpawned;
            Ply.ReceivingVoiceMessage -= eventHandlers.OnPlayerReceivingVoiceMessage;

            CustomItem.UnregisterItems();

            eventHandlers = null;
            Instance = null;
            base.OnDisabled();
        }
    }
}
