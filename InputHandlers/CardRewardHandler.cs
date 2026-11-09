using Godot;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.Screens.CardSelection;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using HarmonyLib;

namespace KeyboardQuickPlay.Handlers;

public partial class CardRewardHandler : Node
{
    private readonly NCardRewardSelectionScreen _screen;
    private Control _cardRow;

    public CardRewardHandler(NCardRewardSelectionScreen screen)
    {
        _screen = screen;
        ProcessMode = ProcessModeEnum.Always;
    }

    public override void _Ready()
    {
        var field = AccessTools.Field(typeof(NCardRewardSelectionScreen), "_cardRow");
        _cardRow = (Control)field?.GetValue(_screen);
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is not InputEventKey keyEvent || !keyEvent.Pressed || keyEvent.Echo)
            return;

        if (keyEvent.Keycode == Key.Space)
        {
            TriggerSkip();
            return;
        }

        if (!ModConfig.IsMatch(@event) && !ModConfig.IsHeld())
            return;

        int index = Helpers.KeyToIndex(keyEvent.Keycode);
        if (index < 0)
            return;

        var cards = GetCards();
        if (index >= cards.Count)
            return;

        cards[index].EmitSignal(NCardHolder.SignalName.Pressed, cards[index]);
    }

    private List<NGridCardHolder> GetCards()
    {
        if (_cardRow == null)
            return new List<NGridCardHolder>();

        return _cardRow.GetChildren()
            .OfType<NGridCardHolder>()
            .ToList();
    }

    public override void _Input(InputEvent @event)
    {
        if (!Helpers.IsTopScreen(_screen))
            return;

        if (@event is not InputEventKey keyEvent || !keyEvent.Pressed || keyEvent.Echo)
            return;

        if (keyEvent.Keycode == Key.Space)
        {
            TriggerSkip();
            GetViewport()?.SetInputAsHandled();
            return;
        }

        int index = Helpers.KeyToIndex(keyEvent.Keycode);
        if (index < 0)
            return;

        var cards = GetCards();
        if (index >= cards.Count)
            return;

        cards[index].EmitSignal(NCardHolder.SignalName.Pressed, cards[index]);
        GetViewport()?.SetInputAsHandled();
    }

    private void TriggerSkip()
    {
        var altContainerField = AccessTools.Field(
            typeof(NCardRewardSelectionScreen),
            "_rewardAlternativesContainer"
        );

        var altContainer = altContainerField?.GetValue(_screen) as Control;

        if (altContainer != null)
        {
            var altButtons = altContainer
                .GetChildren(false)
                .OfType<NCardRewardAlternativeButton>()
                .Where(b => b.Visible && b.IsEnabled)
                .ToList();

            if (altButtons.Count > 0)
            {
                foreach (var button in altButtons)
                {
                    button.GrabFocus();
                    button.EmitSignal(NClickableControl.SignalName.Released, button);
                }
                return;
            }
        }

        var skipField = AccessTools.Field(
            typeof(NCardRewardSelectionScreen),
            "_skipButton"
        );

        var button2 = skipField?.GetValue(_screen) as NChoiceSelectionSkipButton;
        if (button2 == null)
            return;

        button2.GrabFocus();
        button2.EmitSignal(NClickableControl.SignalName.Released, button2);
    }

    private static void Trigger(NCardRewardAlternativeButton button)
    {
        button.GrabFocus();
        button.EmitSignal(NClickableControl.SignalName.Released, button);
    }
}
