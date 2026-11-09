using Godot;
using System.Linq;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.RestSite;

namespace KeyboardQuickPlay.Handlers;

public partial class RestSiteHandler : Node
{
    private NRestSiteRoom _room;

    public RestSiteHandler()
    {
    }

    public void Init(NRestSiteRoom room)
    {
        _room = room;
        ProcessMode = ProcessModeEnum.Always;
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (!Helpers.IsTopScreen(_room))
            return;

        if (@event is not InputEventKey keyEvent || !keyEvent.Pressed || keyEvent.Echo)
            return;

        if (keyEvent.Keycode == Key.Space && !ModConfig.IsMatch(@event) && !ModConfig.IsHeld())
            return;

        HandleInput(keyEvent);
    }

    public override void _Input(InputEvent @event)
    {
        if (!Helpers.IsTopScreen(_room))
            return;

        if (@event is not InputEventKey keyEvent || !keyEvent.Pressed || keyEvent.Echo)
            return;

        if (keyEvent.Keycode == Key.Space && !ModConfig.IsMatch(@event) && !ModConfig.IsHeld())
            return;

        HandleInput(keyEvent);
        GetViewport()?.SetInputAsHandled();
    }

    private void HandleInput(InputEventKey keyEvent)
    {
        if (keyEvent.Keycode == Key.Space || keyEvent.Keycode == Key.E)
        {
            TryProceed();
            return;
        }

        int index = Helpers.KeyToIndex(keyEvent.Keycode);
        if (index < 0)
            return;

        SelectOption(index);
    }

    private void TryProceed()
    {
        var proceed = _room.ProceedButton;

        if (proceed == null || !proceed.IsEnabled)
            return;

        proceed.Call("GrabFocus");
        proceed.EmitSignal("pressed");
    }

    private void SelectOption(int index)
    {
        var options = _room.Options;

        if (options == null || index >= options.Count)
            return;

        var children = _room._choicesContainer.GetChildren(false);

        var buttons = children
            .OfType<NRestSiteButton>()
            .Where(b => b.Visible)
            .ToList();

        if (index >= buttons.Count)
            return;

        var selectedButton = buttons[index];

        if (!selectedButton.IsEnabled)
            return;

        selectedButton.Call(NRestSiteButton.MethodName.OnPress);
        selectedButton.Call(NRestSiteButton.MethodName.OnRelease);
    }
}
