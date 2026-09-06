using HarmonyLib;
using KeyboardQuickPlay.Handlers;
using MegaCrit.Sts2.Core.Nodes.Screens.CardSelection;

namespace KeyboardQuickPlay.Patches;

[HarmonyPatch(typeof(NDeckCardSelectScreen), "_Ready")]
[HarmonyPatch(typeof(NDeckEnchantSelectScreen), "_Ready")]
[HarmonyPatch(typeof(NDeckTransformSelectScreen), "_Ready")]
[HarmonyPatch(typeof(NDeckUpgradeSelectScreen), "_Ready")]
public static class KeyboardCardGridSelectionPatch
{
    public static void Postfix(NCardGridSelectionScreen __instance)
    {
        var node = new CardGridSelectionHandler();
        node.Init(__instance);
        __instance.AddChild(node);
    }
}