using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            if (CurrentNode.NextNode == null)
            {
                return false;
            }

            CurrentNode = Dialogue.Nodes[CurrentNode.NextNode];

            return true; ;
        }
    }
}
