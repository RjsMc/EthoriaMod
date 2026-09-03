using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Terraria;

namespace EthoriaMod.Content.Dialogue
{
    public class DialogueSession
    {
        public Dialogue Dialogue { get; }
        public DialogueNode CurrentNode { get; private set; }
        public DialogueSession(Dialogue dialogue)
        {
            Dialogue = dialogue;
            CurrentNode = dialogue.Nodes[dialogue.StartNode!];
        }

        public bool Advance()
        {
            if (CurrentNode.NextNode == null) { return false; }

            CurrentNode = Dialogue.Nodes[CurrentNode.NextNode];
            ExecuteCurrentAction();
            return true;
        }

        public bool SelectPrompt(int index)
        {
            if (index < 0 || index >= CurrentNode.Prompts.Count) { return false; } // Wont accidentally pick unpickable prompt

            DialoguePrompt prompt = CurrentNode.Prompts[index];

            if (prompt.NextNode == null) { return false; }

            CurrentNode = Dialogue.Nodes[prompt.NextNode];
            ExecuteCurrentAction();
            return true;
        }

        private void ExecuteCurrentAction()
        {
           switch (CurrentNode.Action)
            {
                case "End": // End the dialogue;
                    DialogueManager.EndDialogue();
                    break;
            } 
        }
    }
}
