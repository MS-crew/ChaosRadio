using Exiled.Events.EventArgs.Player;

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
                if (ar.Player.Role.Type == PlayerRoles.RoleTypeId.CustomRole && !plugin.Config.AddEvenCustomRole)
                    return;
                else
                    plugin.Config.ChaosRadio.Give(ar.Player, false);
            }
        }
    }
}
