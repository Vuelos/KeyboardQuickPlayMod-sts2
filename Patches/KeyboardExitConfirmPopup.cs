using HarmonyLib;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.Multiplayer;
using KeyboardQuickPlay.Handlers;

namespace KeyboardQuickPlay.Patches;

/// <summary>
/// Adds an Enter handler to the quit-game confirmation popup only.
/// </summary>
[HarmonyPatch(typeof(NGenericPopup), nameof(NGenericPopup.WaitForConfirmation))]
public static class QuitConfirmPopup_Patch
{
    private const string LocTable = "main_menu_ui";
    private const string QuitConfirmHeader = "QUIT_CONFIRM_POPUP.header";

    public static void Postfix(NGenericPopup __instance, LocString __1)
    {
        if (__1 == null || __1.LocTable != LocTable || __1.LocEntryKey != QuitConfirmHeader)
        {
            return;
        }

        if (__instance.GetNodeOrNull<ExitConfirmHandler>("ExitConfirmHandler") != null)
        {
            return;
        }

        var handler = new ExitConfirmHandler
        {
            Name = "ExitConfirmHandler"
        };

        handler.Init(__instance);
        __instance.AddChild(handler);
    }
}
