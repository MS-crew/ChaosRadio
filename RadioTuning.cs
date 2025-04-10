using Mirror;
using HarmonyLib;
using System.Linq;
using UnityEngine;
using System.Reflection;
using System.Reflection.Emit;
using Exiled.API.Features.Pools;
using Exiled.API.Features.Items;
using System.Collections.Generic;
using static HarmonyLib.AccessTools;

using InventorySystem;
using InventorySystem.Items;
using VoiceChat.Networking;

namespace ChaosRadio
{
    [HarmonyPatch(typeof(VoiceTransceiver), nameof(VoiceTransceiver.ServerReceiveMessage))]
    public static class ServerReceiveMessageTranspilers
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> Yenikodlar = ListPool<CodeInstruction>.Pool.Get(instructions);

            LocalBuilder Kaostelsizmi = generator.DeclareLocal(typeof(bool));

            Label Atla = generator.DefineLabel();
            Label Atla2 = generator.DefineLabel();
            Label Donguatla = generator.DefineLabel();

            int GonderenKontrol = Yenikodlar.FindIndex(instruction => instruction.opcode == OpCodes.Call && instruction.operand.ToString().Contains("get_AllHubs"));

            Yenikodlar.InsertRange(GonderenKontrol,
            [
                new(OpCodes.Ldloc, 1),
                new(OpCodes.Ldc_I4, 2),
                new(OpCodes.Bne_Un, Atla2),

                new(OpCodes.Ldarg_0),
                new(OpCodes.Callvirt, PropertyGetter(typeof(NetworkConnection), nameof(NetworkConnection.identity))),
                new(OpCodes.Callvirt, PropertyGetter(typeof(NetworkIdentity), nameof(NetworkIdentity.gameObject))),
                new(OpCodes.Callvirt, Method(typeof(ReferenceHub), nameof(ReferenceHub.GetHub),[typeof(GameObject)])),
                new(OpCodes.Ldfld, Field(typeof(ReferenceHub), nameof(ReferenceHub.inventory))),
                new(OpCodes.Ldfld, Field(typeof(Inventory), nameof(Inventory.UserInventory))),
                new(OpCodes.Ldfld, Field(typeof(InventoryInfo), nameof(InventoryInfo.Items))),
                new(OpCodes.Callvirt, PropertyGetter(typeof(Dictionary<ushort, ItemBase>), nameof(Dictionary<ushort, ItemBase>.Values))),
                new(OpCodes.Call,  Method(typeof(ServerReceiveMessageTranspilers), nameof(ServerReceiveMessageTranspilers.KaosTelsizGonderen))),
                new(OpCodes.Stloc, Kaostelsizmi.LocalIndex),

                new CodeInstruction(OpCodes.Nop).WithLabels(Atla2),
            ]);

            int Alankontrol = Yenikodlar.FindIndex(instruction => instruction.opcode == OpCodes.Ldarga_S);

            Yenikodlar.InsertRange(Alankontrol,
            [
                new(OpCodes.Ldloc, 5),
                new(OpCodes.Ldc_I4, 2),
                new(OpCodes.Bne_Un, Atla),

                new(OpCodes.Ldloc_3),
                new(OpCodes.Ldfld, Field(typeof(ReferenceHub), nameof(ReferenceHub.inventory))),
                new(OpCodes.Ldfld, Field(typeof(Inventory), nameof(Inventory.UserInventory))),
                new(OpCodes.Ldfld, Field(typeof(InventoryInfo), nameof(InventoryInfo.Items))),
                new(OpCodes.Callvirt, PropertyGetter(typeof(Dictionary<ushort, ItemBase>), nameof(Dictionary<ushort, ItemBase>.Values))),
                new(OpCodes.Call, Method(typeof(ServerReceiveMessageTranspilers), nameof(ServerReceiveMessageTranspilers.KaosTelsizGonderen))),
                new(OpCodes.Ldloc, Kaostelsizmi.LocalIndex),
                new(OpCodes.And),
                new(OpCodes.Brfalse_S, Donguatla),

                new CodeInstruction(OpCodes.Nop).WithLabels(Atla),
            ]);

            int DonguDevam = Yenikodlar.FindIndex(instruction => instruction.opcode == OpCodes.Call && instruction.operand is MethodInfo method4 && method4.Name == "MoveNext");
            Yenikodlar[DonguDevam - 1].labels.Add(Donguatla);

            for (int z = 0; z < Yenikodlar.Count; z++)
                yield return Yenikodlar[z];

            ListPool<CodeInstruction>.Pool.Return(Yenikodlar);
        }
        public static bool KaosTelsizGonderen(ICollection<ItemBase> items) => items.Any(item => KaosTelsiz.telsiz.Check(Item.Get(item)));
    }
}