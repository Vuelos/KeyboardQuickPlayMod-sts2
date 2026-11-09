using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using KeyboardQuickPlay.Handlers;

namespace KeyboardQuickPlay.Patches;

[HarmonyPatch(typeof(NRestSiteRoom), "_Ready")]
public static class NRestSiteRoom_Ready_Patch
{
    public static void Postfix(NRestSiteRoom __instance)
    {
        if (!__instance.IsInsideTree())
        {
            return;
        }

        if (__instance.GetNodeOrNull<RestSiteHandler>("RestSiteHandler") != null)
        {
            return;
        }

        var handler = new RestSiteHandler
        {
            Name = "RestSiteHandler"
        };

        handler.Init(__instance);
        __instance.AddChild(handler);
    }
}