using Godot;
using MegaCrit.Sts2.Core.Nodes.Potions;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using System.Collections.Generic;

namespace KeyboardQuickPlay.Handlers;

public partial class PotionContainerHandler : Node
{
    private NPotionContainer _container;

    public void Init(NPotionContainer container)
    {
        _container = container;
        ProcessMode = ProcessModeEnum.Always;
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is not InputEventKey key || !key.Pressed || key.Echo)
            return;

        if (IsPotionPopupOpen())
        {
            HandlePopupInput(key);
            return;
        }

        if (!key.CtrlPressed && !key.MetaPressed)
            return;

        int index = Helpers.KeyToIndex(key.Keycode);
        if (index < 0)
            return;

        OpenPotion(index);
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is not InputEventKey key || !key.Pressed || key.Echo)
            return;

        if (IsPotionPopupOpen())
        {
            if (!key.CtrlPressed && !key.MetaPressed)
            {
                HandlePopupInput(key);
                GetViewport()?.SetInputAsHandled();
            }
            else if (key.CtrlPressed || key.MetaPressed)
            {
                ClosePotionPopup();
            }
            return;
        }

        if (!key.CtrlPressed && !key.MetaPressed)
            return;

        int index = Helpers.KeyToIndex(key.Keycode);
        if (index < 0)
            return;

        OpenPotion(index);
        GetViewport()?.SetInputAsHandled();
    }

    private void OpenPotion(int index)
    {
        ClosePotionPopup();

        var holders = GetVisibleHolders();
        if (index >= holders.Count)
            return;

        var holder = holders[index];
        holder.Call(NPotionHolder.MethodName.OnPress);
        holder.Call(NPotionHolder.MethodName.OnRelease);
    }

    private void ClosePotionPopup()
    {
        var popup = GetVisiblePopup();
        if (popup != null)
        {
            popup.Visible = false;
        }
    }

    private void HandlePopupInput(InputEventKey key)
    {
        int index = Helpers.KeyToIndex(key.Keycode);
        if (index < 0)
            return;

        var popup = GetVisiblePopup();
        if (popup == null)
            return;

        var buttons = popup.GetChildren(false)
            .OfType<Button>()
            .Where(b => b.Visible && !b.Disabled)
            .ToList();

        if (index >= buttons.Count)
            return;

        var selectedButton = buttons[index];
        selectedButton.GrabFocus();
        selectedButton.EmitSignal(Button.SignalName.Pressed);
    }

    private bool IsPotionPopupOpen()
    {
        return GetVisiblePopup() != null;
    }

    private NPotionPopup GetVisiblePopup()
    {
        if (_container == null || _container.GetTree() == null)
            return null;

        var root = _container.GetTree().Root;
        if (root == null)
            return null;

        return FindVisiblePopup(root);
    }

    private NPotionPopup FindVisiblePopup(Node node)
    {
        if (node is NPotionPopup popup && popup.Visible)
            return popup;

        foreach (var child in node.GetChildren(false))
        {
            var result = FindVisiblePopup(child);
            if (result != null)
                return result;
        }

        return null;
    }

    private List<NPotionHolder> GetVisibleHolders()
    {
        var holders = new List<NPotionHolder>();

        if (_container == null || _container.GetTree() == null)
            return holders;

        CollectPotionHolders(_container, holders);

        if (holders.Count == 0)
        {
            CollectPotionHolders(_container.GetTree().Root, holders);
        }

        return holders;
    }

    private void CollectPotionHolders(Node node, List<NPotionHolder> holders)
    {
        if (node is NPotionHolder holder && holder.Visible)
        {
            holders.Add(holder);
        }

        foreach (var child in node.GetChildren(false))
        {
            CollectPotionHolders(child, holders);
        }
    }
}
