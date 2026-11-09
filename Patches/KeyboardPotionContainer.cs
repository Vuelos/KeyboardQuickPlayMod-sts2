using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Potions;
using KeyboardQuickPlay.Handlers;

namespace KeyboardQuickPlay.Patches;

[HarmonyPatch(typeof(NPotionContainer), "_Ready")]
public static class KeyboardPotionContainerPatch
{
    static void Postfix(NPotionContainer __instance)
    {
        if (!__instance.IsInsideTree())
        {
            return;
        }

        if (__instance.GetNodeOrNull<PotionContainerHandler>("PotionContainerHotkeyNode") != null)
        {
            return;
        }

        var node = new PotionContainerHandler
        {
            Name = "PotionContainerHotkeyNode"
        };

        __instance.AddChild(node);
        node.Init(__instance);
    }
}
