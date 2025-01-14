using System;
using HarmonyLib;
using Exiled.API.Features;
using Exiled.CustomItems.API.Features;
using P = Exiled.Events.Handlers.Player;

namespace ChaosRadio
{
    public class Plugin : Plugin<Config>
    {
        public static Plugin Instance { get; private set; }

        public static EventHandlers eventHandlers;
        public override string Author => "ZurnaSever";
        public override string Name => "ChaosRadio";
        public override string Prefix => "ChaosRadio";
        public override Version RequiredExiledVersion { get; } = new Version(9, 3, 0);
        public override Version Version { get; } = new Version(1, 1, 7);
        private Harmony harmony;
        public override void OnEnabled()
        {
            Instance = this;
            eventHandlers = new EventHandlers(this);

            CustomItem.RegisterItems();
            harmony = new Harmony("KaosTelsizi");
            harmony.PatchAll();

            if (Config.AddRadioinSpawn) P.Spawned += eventHandlers.OnSpawned;
            base.OnEnabled();
        }
        public override void OnDisabled()
        {
            if (Config.AddRadioinSpawn) P.Spawned -= eventHandlers.OnSpawned;
            
            CustomItem.UnregisterItems();
            harmony.UnpatchAll(harmonyID: "KaosTelsizi");

            eventHandlers = null;
            Instance = null;
            base.OnDisabled();
        }
    }
}
