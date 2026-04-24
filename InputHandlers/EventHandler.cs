using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;

namespace KeyboardQuickPlay.Handlers;

public partial class EventHotkeyNode : Node
{
    private NEventRoom _room;

    public void Init(NEventRoom room)
    {
        _room = room;
        ProcessMode = ProcessModeEnum.Always;
    }
    public override void _UnhandledInput(InputEvent @event)
    {
        if (!Helpers.IsTopScreen(_room))
            return;

        if (@event is not InputEventKey keyEvent)
            return;

        if (!keyEvent.Pressed || keyEvent.Echo)
            return;

        var options = GetOptions();
        if (options.Count == 0)
            return;

        int index = Helpers.KeyToIndex(keyEvent.Keycode);
        if (index < 0 || index >= options.Count)
            return;

        Trigger(options[index], index);
    }

    #region Helpers

    private List<EventOption> GetOptions()
    {
        var evt = Traverse.Create(_room)
            .Field("_event")
            .GetValue<object>();

        if (evt == null)
            return new();

        var options = Traverse.Create(evt)
            .Property("CurrentOptions")
            .GetValue<IReadOnlyList<EventOption>>();

        return options != null ? new List<EventOption>(options) : new();
    }

    private void Trigger(EventOption option, int index)
    {
        if (option.IsLocked)
            return;

        var method = AccessTools.Method(typeof(NEventRoom), "OptionButtonClicked");
        method.Invoke(_room, new object[] { option, index });
    }

    #endregion
}
