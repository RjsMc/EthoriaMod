using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EthoriaMod.Content.Dialogue
{
    public class DialogueNode
    {
        public string Id { get; set; }
        public string Speaker { get; set; }
        public string Text { get; set; }
        public List<DialoguePrompt> Prompts { get; set; } = new();
        public string NextNode { get; set; }
        public string Action { get; set; }
    }
}
