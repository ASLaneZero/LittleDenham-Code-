using System;
using System.Collections.Generic;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;

namespace LittleDenham
{
    internal class DialogueBoxWithActions : DialogueBox
{
    private List<Action> ResponseActions;
        private List<Response> choices;
        private List<Action> selectionActions;

        public DialogueBoxWithActions(string dialogue, List<Response> choices, List<Action> selectionActions) : base(dialogue)
        {
            this.choices = choices;
            this.selectionActions = selectionActions;
        }

        internal DialogueBoxWithActions(string dialogue, Response[] responses, List<Action> Actions) : base(dialogue, responses)
    {
        this.ResponseActions = Actions;
    }

    public override void receiveLeftClick(int x, int y, bool playSound = true)
    {
        if (base.safetyTimer <= 0 && StardewModdingAPI.Constants.TargetPlatform == GamePlatform.Android)
        {
            base.receiveLeftClick(x, y, playSound);
        }
        int responseIndex = this.selectedResponse;
        base.receiveLeftClick(x, y, playSound);
        if (base.safetyTimer <= 0 && responseIndex > -1 && responseIndex < this.ResponseActions.Count && this.ResponseActions[responseIndex] != null)
        {
            this.ResponseActions[responseIndex]();
        }
    }
}
}