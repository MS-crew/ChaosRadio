using Exiled.CustomRoles.API;
using Exiled.Events.EventArgs.Player;
using Exiled.CustomItems.API.Features;

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
    }
}
