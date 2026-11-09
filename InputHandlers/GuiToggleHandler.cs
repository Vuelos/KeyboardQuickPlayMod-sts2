using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using System;
using System.Collections.Generic;

namespace KeyboardQuickPlay.Handlers;

public partial class GuiToggleHandler : Node
{
    private Node _node;
    private IScreenContext _screenContext;

    // Shared across handlers so the hidden state and the nodes we hid survive
    // room transitions. Each battle gets a fresh handler, but the top bar and
    // relic inventory live in GlobalUi and stay hidden unless we keep tracking
    // them here.
    private static bool _hidden;
    private static readonly Dictionary<Node, bool> _visibilityStates = new();
    private static readonly Dictionary<Type, bool> _typeMatchCache = new();
    private static ulong _lastSweepFrame = ulong.MaxValue;

    public void Init(Node node)
    {
        _node = node;
        _screenContext = node as IScreenContext;
        ProcessMode = ProcessModeEnum.Always;
        GD.Print("[GuiToggle] Handler initialized on " + node.GetType().FullName);

        // If the UI was hidden in a previous room/battle, hide anything new in
        // this screen too so the state stays consistent.
        if (_hidden)
            Callable.From(ReapplyHiddenState).CallDeferred();
    }

    public override void _Process(double delta)
    {
        if (!_hidden || !GodotObject.IsInstanceValid(_node) || !_node.IsInsideTree())
            return;

        // Some elements (e.g. the exhaust pile button) are shown by the game
        // mid-combat. Keep enforcing the hidden state so they cannot pop back.
        var frame = Engine.GetProcessFrames();
        if (frame == _lastSweepFrame)
            return;
        _lastSweepFrame = frame;

        var root = _node.GetTree()?.Root;
        if (root != null)
            HideMatchingNodes(root);
    }

    private void ReapplyHiddenState()
    {
        if (!_hidden)
            return;

        HideElements(reset: false);
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (_screenContext != null && !Helpers.IsTopScreen(_screenContext))
            return;

        if (@event is not InputEventKey key || !key.Pressed || key.Echo)
            return;

        if (key.Keycode == Key.H)
        {
            GD.Print("[GuiToggle] H pressed -> Toggle");
            Toggle();
            return;
        }

        if (!ModConfig.IsMatch(@event) && !ModConfig.IsHeld())
            return;
    }

    public override void _Input(InputEvent @event)
    {
        if (_screenContext != null && !Helpers.IsTopScreen(_screenContext))
            return;

        if (@event is not InputEventKey key || !key.Pressed || key.Echo)
            return;

        if (key.Keycode == Key.H)
        {
            GD.Print("[GuiToggle] _Input fallback: H");
            Toggle();
            GetViewport()?.SetInputAsHandled();
            return;
        }

        if (!ModConfig.IsMatch(@event) && !ModConfig.IsHeld())
            return;
    }

    private void Toggle()
    {
        GD.Print("[GuiToggle] Toggle() called");
        _hidden = !_hidden;

        if (_hidden)
            HideElements(reset: true);
        else
            ShowElements();

        GD.Print($"[GuiToggle] Hidden: {_hidden}");
    }

    private void HideElements(bool reset)
    {
        var root = _node.GetTree()?.Root;
        if (root == null)
        {
            GD.PrintErr("[GuiToggle] Tree root is null");
            return;
        }

        GD.Print("[GuiToggle] Searching for UI elements to hide...");
        if (reset)
            _visibilityStates.Clear();
        HideMatchingNodes(root);
        GD.Print($"[GuiToggle] Hide complete ({_visibilityStates.Count} nodes hidden)");
    }

    private void ShowElements()
    {
        GD.Print("[GuiToggle] Restoring UI elements...");
        int restored = 0;

        foreach (var (node, wasVisible) in _visibilityStates)
        {
            if (!GodotObject.IsInstanceValid(node))
                continue;

            if (node is CanvasItem canvasItem)
            {
                // Restore only what this mod hid, preserving the game's own
                // visibility and layout state for everything else.
                canvasItem.Visible = wasVisible;
                restored++;
                GD.Print($"[GuiToggle]   Shown: {node.GetType().FullName}");
            }
        }

        _visibilityStates.Clear();
        GD.Print($"[GuiToggle] Show complete ({restored} nodes restored)");
    }

    private void HideMatchingNodes(Node node)
    {
        if (ShouldHide(node))
        {
            if (node is CanvasItem canvasItem && canvasItem.Visible)
            {
                _visibilityStates[node] = true;
                canvasItem.Visible = false;
                GD.Print($"[GuiToggle]   Hidden: {node.GetType().FullName}");
            }
            return;
        }

        foreach (var child in node.GetChildren(false))
        {
            HideMatchingNodes(child);
        }
    }

    private bool ShouldHide(Node node)
    {
        var type = node.GetType();
        if (!_typeMatchCache.TryGetValue(type, out var hideByType))
        {
            var typeName = type.FullName ?? type.Name;

            hideByType =
                typeName.Contains("NRelic") || typeName.Contains("Relic") ||
                typeName.Contains("NExhaust") || typeName.Contains("Exhaust") ||
                typeName.Contains("NDiscard") || typeName.Contains("Discard") ||
                typeName.Contains("NDraw") ||
                typeName.Contains("NPeek") || typeName.Contains("Peek") ||
                typeName.Contains("NPing") || typeName.Contains("Ping") ||
                typeName.Contains("NEndTurn") || typeName.Contains("EndTurn") ||
                typeName.Contains("NPlayerTurnBanner") || typeName.Contains("NEnemyTurnBanner") ||
                typeName.Contains("NCombatStartBanner") ||
                typeName.Contains("NTopBar") ||
                typeName.Contains("NEnergyCounter") ||
                typeName.Contains("NProfile") ||
                typeName.Contains("NLogo");

            _typeMatchCache[type] = hideByType;
        }

        if (hideByType)
            return true;

        var name = node.Name;

        if (name == "TopBar" || name == "Profile" || name == "Logo")
            return true;

        if (name == "DebugInfo" || name == "ReleaseInfo" || name == "ModdedWarning" || name == "DebugSeed")
            return true;

        // These are the only checks that need substring matching.
        var nameText = name.ToString();
        if (nameText.Contains("ModList") || nameText.Contains("party") || nameText.Contains("Party"))
            return true;

        return false;
    }
}
