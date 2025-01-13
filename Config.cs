using Exiled.API.Interfaces;
using System.ComponentModel;

namespace ChaosRadio
{
    public class Config : IConfig
    {
        [Description("Whether the plugin is enabled or disabled")]
        public bool IsEnabled { get; set; } = true;

        [Description("debug open or not")]
        public bool Debug { get; set; } = false; 
        
        [Description("Should every chaos insurgency get a chaos radio when they spawn?")]
        public bool AddRadioinSpawn { get; set; } = true;

        [Description("Should chaos insurgency get chaos radio even if it is a Custom Role?")]
        public bool AddEvenCustomRole  { get; set; } = true;
        public KaosTelsiz ChaosRadio { get; set; } = new KaosTelsiz();
    }
}
