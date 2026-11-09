using Godot;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Multiplayer;

namespace KeyboardQuickPlay.Handlers;

/// <summary>
/// Confirms the quit-game popup with Enter. The popup's Yes button is bound to
/// the gamepad/keyboard "select" action, which does not include Enter.
/// </summary>
public partial class ExitConfirmHandler : Node
{
    private NGenericPopup _popup;

    public void Init(NGenericPopup popup)
    {
        _popup = popup;
        ProcessMode = ProcessModeEnum.Always;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is not InputEventKey keyEvent || !keyEvent.Pressed || keyEvent.Echo)
            return;

        if (keyEvent.Keycode != Key.Enter && keyEvent.Keycode != Key.KpEnter)
            return;

        if (!Helpers.IsTopScreen(_popup))
            return;

        var yesButton = _popup._verticalPopup?.YesButton;
        if (yesButton == null || !yesButton.IsVisible() || !yesButton.IsEnabled)
            return;

        yesButton.GrabFocus();
        yesButton.EmitSignal(NClickableControl.SignalName.Released, yesButton);
        GetViewport()?.SetInputAsHandled();
    }
}
