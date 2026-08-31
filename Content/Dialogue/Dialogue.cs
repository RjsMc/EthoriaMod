using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable enable
namespace EthoriaMod.Content.Dialogue
{
    public class Dialogue
    {
        public string? StartNode { get; set; }
        public Dictionary<string, DialogueNode> Nodes { get; } = new();
    }

    public class DialogueNode
    {
        public string Id { get; set; } = "";
        public string? Speaker { get; set; }
        public string? Text { get; set; }
        public string? NextNode { get; set; }
        public string? Action { get; set; }

        public List<DialoguePrompt> Prompts { get; } = new();
    }
}
