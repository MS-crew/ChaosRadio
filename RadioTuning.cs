﻿using Mirror;
using HarmonyLib;
using System.Linq;
using UnityEngine;
using System.Reflection;
using VoiceChat.Networking;
using System.Reflection.Emit;
using Exiled.API.Features.Pools;
using Exiled.API.Features.Items;
using System.Collections.Generic;
using InventorySystem;
using InventorySystem.Items;

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

            Yenikodlar[GonderenKontrol].labels.Add(Atla2);
            Yenikodlar.InsertRange(GonderenKontrol, new List<CodeInstruction>
            {
                new CodeInstruction(OpCodes.Ldloc, 1),
                new CodeInstruction(OpCodes.Ldc_I4, 2),
                new CodeInstruction(OpCodes.Bne_Un, Atla2),

                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(NetworkConnection), nameof(NetworkConnection.identity))),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(NetworkIdentity), nameof(NetworkIdentity.gameObject))),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(ReferenceHub), nameof(ReferenceHub.GetHub),[typeof(GameObject)])),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(ReferenceHub), nameof(ReferenceHub.inventory))),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(Inventory), nameof(Inventory.UserInventory))),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(InventoryInfo), nameof(InventoryInfo.Items))),
                new CodeInstruction(OpCodes.Call,  AccessTools.Method(typeof(Kontrol), nameof(Kontrol.KaosTelsizGonderen))),
                new CodeInstruction(OpCodes.Stloc, Kaostelsizmi.LocalIndex)
            });

            int Alankontrol = Yenikodlar.FindIndex(instruction => instruction.opcode == OpCodes.Ldarga_S);
            Yenikodlar[Alankontrol].labels.Add(Atla);
            Yenikodlar.InsertRange(Alankontrol, new List<CodeInstruction>
            {
                new CodeInstruction(OpCodes.Ldloc, 5),
                new CodeInstruction(OpCodes.Ldc_I4, 2),
                new CodeInstruction(OpCodes.Bne_Un, Atla),

                new CodeInstruction(OpCodes.Ldloc_3),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(ReferenceHub), nameof(ReferenceHub.inventory))),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(Inventory), nameof(Inventory.UserInventory))),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(InventoryInfo), nameof(InventoryInfo.Items))),
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Kontrol), nameof(Kontrol.KaosTelsizGonderen))),
                new CodeInstruction(OpCodes.Ldloc, Kaostelsizmi.LocalIndex),
                new CodeInstruction(OpCodes.And),
                new CodeInstruction(OpCodes.Brfalse_S, Donguatla),
            });

            int DonguDevam = Yenikodlar.FindIndex(instruction => instruction.opcode == OpCodes.Call && instruction.operand is MethodInfo method4 && method4.Name == "MoveNext");
            Yenikodlar[DonguDevam - 1].labels.Add(Donguatla);

            for (int z = 0; z < Yenikodlar.Count; z++)
                yield return Yenikodlar[z];

            ListPool<CodeInstruction>.Pool.Return(Yenikodlar);
        }

        public static class Kontrol
        {
            public static bool KaosTelsizGonderen(Dictionary<ushort,ItemBase> Items)
            {
                return Items.Values.Any(item => KaosTelsiz.telsiz.Check(Item.Get(item)));
            }
        }
    }
}