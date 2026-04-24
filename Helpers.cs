using Godot;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using KeyboardQuickPlay;
using MegaCrit.Sts2.Core.Nodes.Screens.CardSelection;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;

namespace KeyboardQuickPlay;

public static class Helpers
{

    public static int KeyToIndex(Key key)
    {
        return key switch
        {
            Key.Key1 => 0,
            Key.Key2 => 1,
            Key.Key3 => 2,
            Key.Key4 => 3,
            Key.Key5 => 4,
            Key.Key6 => 5,
            Key.Key7 => 6,
            Key.Key8 => 7,
            Key.Key9 => 8,
            _ => -1
        };
    }

    public static bool IsTopScreen(IScreenContext instance)
    {
        return ActiveScreenContext.Instance?.IsCurrent(instance) ?? false;
    }
    public static bool IsTopOverlay(IOverlayScreen instance)
    {
        return NOverlayStack.Instance?.Peek()  == instance;
    }
}