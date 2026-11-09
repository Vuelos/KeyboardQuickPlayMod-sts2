using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;
using MegaCrit.Sts2.Core.Nodes.Screens.PauseMenu;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using KeyboardQuickPlay.Handlers;

namespace KeyboardQuickPlay.Patches;

[HarmonyPatch(typeof(NCombatRoom), "_Ready")]
public static class KeyboardCombatGuiTogglePatch
{
    static void Postfix(NCombatRoom __instance)
    {
        if (!__instance.IsInsideTree())
            return;

        if (__instance.GetNodeOrNull<GuiToggleHandler>("GuiToggleHotkeyNode") != null)
            return;

        var node = new GuiToggleHandler
        {
            Name = "GuiToggleHotkeyNode"
        };

        __instance.AddChild(node);
        node.Init(__instance);
    }
}

[HarmonyPatch(typeof(NRestSiteRoom), "_Ready")]
public static class KeyboardRestSiteGuiTogglePatch
{
    static void Postfix(NRestSiteRoom __instance)
    {
        if (!__instance.IsInsideTree())
            return;

        if (__instance.GetNodeOrNull<GuiToggleHandler>("GuiToggleHotkeyNode") != null)
            return;

        var node = new GuiToggleHandler
        {
            Name = "GuiToggleHotkeyNode"
        };

        __instance.AddChild(node);
        node.Init(__instance);
    }
}

[HarmonyPatch(typeof(NEventRoom), "_Ready")]
public static class KeyboardEventRoomGuiTogglePatch
{
    static void Postfix(NEventRoom __instance)
    {
        if (!__instance.IsInsideTree())
            return;

        if (__instance.GetNodeOrNull<GuiToggleHandler>("GuiToggleHotkeyNode") != null)
            return;

        var node = new GuiToggleHandler
        {
            Name = "GuiToggleHotkeyNode"
        };

        __instance.AddChild(node);
        node.Init(__instance);
    }
}

[HarmonyPatch(typeof(NMapScreen), "_Ready")]
public static class KeyboardMapGuiTogglePatch
{
    static void Postfix(NMapScreen __instance)
    {
        if (!__instance.IsInsideTree())
            return;

        if (__instance.GetNodeOrNull<GuiToggleHandler>("GuiToggleHotkeyNode") != null)
            return;

        var node = new GuiToggleHandler
        {
            Name = "GuiToggleHotkeyNode"
        };

        __instance.AddChild(node);
        node.Init(__instance);
    }
}

[HarmonyPatch(typeof(NMainMenu), "_Ready")]
public static class KeyboardMainMenuGuiTogglePatch
{
    static void Postfix(NMainMenu __instance)
    {
        if (!__instance.IsInsideTree())
            return;

        if (__instance.GetNodeOrNull<GuiToggleHandler>("GuiToggleHotkeyNode") != null)
            return;

        var node = new GuiToggleHandler
        {
            Name = "GuiToggleHotkeyNode"
        };

        __instance.AddChild(node);
        node.Init(__instance);
    }
}

[HarmonyPatch(typeof(NPauseMenu), "_Ready")]
public static class KeyboardPauseMenuGuiTogglePatch
{
    static void Postfix(NPauseMenu __instance)
    {
        if (!__instance.IsInsideTree())
            return;

        if (__instance.GetNodeOrNull<GuiToggleHandler>("GuiToggleHotkeyNode") != null)
            return;

        var node = new GuiToggleHandler
        {
            Name = "GuiToggleHotkeyNode"
        };

        __instance.AddChild(node);
        node.Init(__instance);
    }
}
