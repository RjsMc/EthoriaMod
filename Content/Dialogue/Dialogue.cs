using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EthoriaMod.Content.Dialogue
{
    public class Dialogue
    {
        public string StartNode;
        public Dictionary<string, DialogueNode> Nodes = new();
    }
}
