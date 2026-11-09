using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using KeyboardQuickPlay.Handlers;

namespace KeyboardQuickPlay.Patches;

[HarmonyPatch(typeof(NTreasureRoom), "_Ready")]
public static class TreasureRoomPatch
{
    static void Postfix(NTreasureRoom __instance)
    {
        if (!__instance.IsInsideTree())
        {
            return;
        }

        if (__instance.GetNodeOrNull<TreasureRoomHandler>("TreasureRoomHotkeyNode") != null)
        {
            return;
        }

        var handler = new TreasureRoomHandler(__instance)
        {
            Name = "TreasureRoomHotkeyNode"
        };

        __instance.AddChild(handler);    
    }
}
