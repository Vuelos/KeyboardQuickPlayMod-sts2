using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Screens;
using KeyboardQuickPlay.Handlers;

namespace KeyboardQuickPlay.Patches;

[HarmonyPatch(typeof(NRewardsScreen), "_Ready")]
public static class NRewardsScreen_AddHotkeyControl
{
    public static void Postfix(NRewardsScreen __instance)
    {
        var node = new RewardHandler();
        __instance.AddChild(node);
        node.Init(__instance);
    }
}