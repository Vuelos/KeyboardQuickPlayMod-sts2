using HarmonyLib;
using Godot;
using MegaCrit.Sts2.Core.Nodes.Screens;
using KeyboardQuickPlay.Handlers;
using MegaCrit.Sts2.Core.Nodes.Screens.CardSelection;

namespace KeyboardQuickPlay.Patches;

[HarmonyPatch(typeof(NCardRewardSelectionScreen), "_Ready")]
public static class NCardRewardsScreen_AddHotkeyControl
{
    static void Postfix(NCardRewardSelectionScreen __instance)
    {
        if (__instance.GetNodeOrNull<CardRewardHandler>("CardRewardHandler") != null)
            return;

        var node = new CardRewardHandler(__instance)
        {
            Name = "CardRewardHandler"
        };

        __instance.AddChild(node);
    }
}