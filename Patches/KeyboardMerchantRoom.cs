using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using KeyboardQuickPlay.Handlers;

namespace KeyboardQuickPlay.Patches;

[HarmonyPatch(typeof(NMerchantRoom), "_Ready")]
public static class NMerchantRoom_Ready_Patch
{
    public static void Postfix(NMerchantRoom __instance)
    {
        if (!__instance.IsInsideTree())
        {
            return;
        }

        if (__instance.GetNodeOrNull<MerchantHandler>("MerchantHandler") != null)
        {
            return;
        }

        var handler = new MerchantHandler
        {
            Name = "MerchantHandler"
        };

        handler.Init(__instance);
        __instance.AddChild(handler);
    }
}
