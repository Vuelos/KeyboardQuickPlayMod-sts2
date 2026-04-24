using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using KeyboardQuickPlay.Handlers;

namespace KeyboardQuickPlay.Patches;

[HarmonyPatch(typeof(NEventRoom), "_Ready")]
public static class KeyboardEventRoomPatch
{
    static void Postfix(NEventRoom __instance)
    {
        var node = new EventHotkeyNode();
        node.Init(__instance);
        __instance.AddChild(node);
    }
}
