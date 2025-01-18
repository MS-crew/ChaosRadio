using Exiled.CustomRoles.API;
using Exiled.Events.EventArgs.Player;
using Exiled.CustomItems.API.Features;

namespace ChaosRadio
{
    public class EventHandlers
    {
        public readonly Plugin plugin;
        public EventHandlers(Plugin plugin) => this.plugin = plugin;
        public void OnSpawned(SpawnedEventArgs ar)
        {
            if (ar.Player.Role.Team == PlayerRoles.Team.ChaosInsurgency)
            {
                if (ar.Player.GetCustomRoles() != null) 
                { 
                    if (plugin.Config.AddEvenCustomRole)
                        CustomItem.TryGive(ar.Player, plugin.Config.ChaosRadio.Name, false);
                }
                else
                    CustomItem.TryGive(ar.Player, plugin.Config.ChaosRadio.Name, false);
            }
        }
    }
}
