using UnityEngine;
using Exiled.API.Enums;
using Exiled.API.Features.Spawn;
using System.Collections.Generic;
using Exiled.API.Features.Attributes;
using Exiled.CustomItems.API.Features;

namespace ChaosRadio
{
    [CustomItem(ItemType.Radio)]
    public class KaosTelsiz : CustomItem
    {
        public static KaosTelsiz telsiz;
        public override uint Id { get; set; } = 40;
        public override float Weight { get; set; } = 1.7f;
        public override string Name { get; set; } = "Chaos Radio"; 
        public override ItemType Type { get; set; } = ItemType.Radio;
        public override Vector3 Scale { get; set; } = new Vector3(1, 1, 1);
        public override string Description { get; set; } = "A special radio for Chaos's communication network.";
        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            Limit = 2,
            DynamicSpawnPoints =
            {
                new()
                {
                        Location = SpawnLocationType.InsideGateA,
                        Chance = 100
                }
            },
            StaticSpawnPoints = new List<StaticSpawnPoint>()
            {
                new()
                {
                    Position = new Vector3(0,0,0),
                    Chance = 0
                }
            }
        };
        public override void Init()
        {
            base.Init();
            telsiz = this;
        }
        public override void Destroy()
        {
            base.Destroy();
            telsiz = null;
        }
    }
}
