using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EthoriaMod.Content.UI.Dialogue
{
    public class TextSnippet
    {
        public string Text { get; }
        public TextSettings Settings { get; }
        public TextSnippet(string text, TextSettings settings)
        {
            Text = text;
            Settings = settings;
        }
    }
}
