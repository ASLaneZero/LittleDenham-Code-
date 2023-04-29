using System;
using System.Collections.Generic;
using StardewValley;
using StardewValley.Menus;

namespace LittleDenham
{
    internal class DialogueBoxWithActions : DialogueBox
    {
        private List<Action> ResponseActions;

        internal DialogueBoxWithActions(string dialogue, List<Response> responses, List<Action> Actions) : base(dialogue, responses)
        {
            this.ResponseActions = Actions;
        }

        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            int responseIndex = this.selectedResponse;
            base.receiveLeftClick(x, y, playSound);
            if (base.safetyTimer <= 0 && responseIndex > -1 && responseIndex < this.ResponseActions.Count && this.ResponseActions[responseIndex] != null)
            {
                this.ResponseActions[responseIndex]();
            }
        }
    }
}
