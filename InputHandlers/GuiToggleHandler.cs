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
    private bool _hidden;

    public void Init(Node node)
    {
        _node = node;
        _screenContext = node as IScreenContext;
        ProcessMode = ProcessModeEnum.Always;
        GD.Print("[GuiToggle] Handler initialized on " + node.GetType().FullName);
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
            HideElements();
        else
            ShowElements();

        GD.Print($"[GuiToggle] Hidden: {_hidden}");
    }

    private void HideElements()
    {
        var root = _node.GetTree()?.Root;
        if (root == null)
        {
            GD.PrintErr("[GuiToggle] Tree root is null");
            return;
        }

        GD.Print("[GuiToggle] Searching for UI elements to hide...");
        HideMatchingNodes(root);

        GD.Print("[GuiToggle] Hide complete");
    }

    private void ShowElements()
    {
        var root = _node.GetTree()?.Root;
        if (root == null)
        {
            GD.PrintErr("[GuiToggle] Tree root is null");
            return;
        }

        GD.Print("[GuiToggle] Searching for UI elements to show...");
        ShowMatchingNodes(root);

        GD.Print("[GuiToggle] Show complete");
    }

    private void HideMatchingNodes(Node node)
    {
        if (ShouldHide(node))
        {
            if (node is CanvasItem canvasItem && canvasItem.Visible)
            {
                canvasItem.Visible = false;
                GD.Print($"[GuiToggle]   Hidden: {node.GetType().FullName}");
            }
        }

        foreach (var child in node.GetChildren(false))
        {
            HideMatchingNodes(child);
        }
    }

    private void ShowMatchingNodes(Node node)
    {
        if (ShouldHide(node))
        {
            if (node is CanvasItem canvasItem && !canvasItem.Visible)
            {
                canvasItem.Visible = true;
                GD.Print($"[GuiToggle]   Shown: {node.GetType().FullName}");
            }
        }

        foreach (var child in node.GetChildren(false))
        {
            ShowMatchingNodes(child);
        }
    }

    private bool ShouldHide(Node node)
    {
        var typeName = node.GetType().FullName.ToString();
        var name = node.Name.ToString();

        if (typeName.Contains("NRelic") || typeName.Contains("Relic"))
            return true;

        if (typeName.Contains("NExhaust") || typeName.Contains("Exhaust"))
            return true;

        if (typeName.Contains("NDiscard") || typeName.Contains("Discard"))
            return true;

        if (typeName.Contains("NDraw") || typeName.Contains("Draw"))
            return true;

        if (typeName.Contains("NPeek") || typeName.Contains("Peek"))
            return true;

        if (typeName.Contains("NPing") || typeName.Contains("Ping"))
            return true;

        if (typeName.Contains("NEndTurn") || typeName.Contains("EndTurn"))
            return true;

        if (typeName.Contains("NPlayerTurnBanner") || typeName.Contains("NEnemyTurnBanner") || typeName.Contains("NCombatStartBanner"))
            return true;

        if (typeName.Contains("NTopBar") || name == "TopBar")
            return true;

        if (typeName.Contains("NEnergyCounter"))
            return true;

        if (typeName.Contains("NProfile") || name == "Profile")
            return true;

        if (typeName.Contains("NLogo") || name == "Logo")
            return true;

        if (name == "Version" || name.Contains("Version"))
            return true;

        if (name.Contains("ModList"))
            return true;

        if (name == "Party" || name.Contains("party") || name.Contains("Party"))
            return true;

        return false;
    }
}
