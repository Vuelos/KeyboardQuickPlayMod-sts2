using Godot;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace KeyboardQuickPlay.Handlers;

public partial class MerchantHandler : Node
{
    private NMerchantRoom _room;

    public void Init(NMerchantRoom room)
    {
        _room = room;
        ProcessMode = ProcessModeEnum.Always;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is not InputEventKey keyEvent || !keyEvent.Pressed || keyEvent.Echo)
            return;

        if (keyEvent.Keycode != Key.Space)
            return;

        if (!Helpers.IsTopScreen(_room))
            return;

        if (_room.Inventory != null && _room.Inventory.IsOpen)
            return;

        _room.OpenInventory();
        GetViewport()?.SetInputAsHandled();
    }
}
