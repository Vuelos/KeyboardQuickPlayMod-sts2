using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Screens.CardSelection;

namespace KeyboardQuickPlay.Handlers;

public partial class CardGridSelectionHandler : Node
{
    private NCardGridSelectionScreen _screen;

    public void Init(NCardGridSelectionScreen screen)
    {
        _screen = screen;
        GD.Print("[CardGridSelectionHandler] Initialized with screen: ", screen?.Name ?? "null");
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (!Helpers.IsTopScreen(_screen))
        {
            GD.Print("[CardGridSelectionHandler] Skipping — not top screen");
            return;
        }

        if (@event is not InputEventKey keyEvent || !keyEvent.Pressed || keyEvent.Echo)
        {
            GD.Print("[CardGridSelectionHandler] Skipping — not a valid key press (type: ", @event?.GetType().Name, ")");
            return;
        }

        int index = Helpers.KeyToIndex(keyEvent.Keycode);
        GD.Print("[CardGridSelectionHandler] Key mapped to index: ", index);

        if (index < 0)
        {
            GD.Print("[CardGridSelectionHandler] Index is negative — ignoring");
            return;
        }
        var field = AccessTools.Field(_screen.GetType(), "_cards");
        var cards = (IReadOnlyList<CardModel>)field?.GetValue(_screen);
        GD.Print("[CardGridSelectionHandler] Cards available: ", cards?.Count.ToString() ?? "null");

        if (index >= cards.Count)
        {
            GD.Print("[CardGridSelectionHandler] Index ", index, " out of range (count: ", cards.Count, ") — ignoring");
            return;
        }

        var card = cards[index];
        GD.Print("[CardGridSelectionHandler] Triggering card at index ", index, ": ", card?.ToString() ?? "null");
        Trigger(card);
    }

    private void Trigger(CardModel cardModel)
    {
        GD.Print("[CardGridSelectionHandler] Trigger called for card: ", cardModel?.ToString() ?? "null");
        _screen.OnCardClicked(cardModel);
        GD.Print("[CardGridSelectionHandler] OnCardClicked invoked successfully");
    }
}