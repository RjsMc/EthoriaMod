using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Terraria.UI;
using Microsoft.Xna.Framework;

namespace EthoriaMod.Content.UI.Dialogue
{
    public class DialoguePromptContainer : UIElement
    {
        public DialoguePromptContainer(DialogueBox dialogueBox)
        {
            HAlign = 0.5f;
            VAlign = 1f;

            Width.Set(dialogueBox.BoxWidth, 0f);
            Height.Set(0f, 0f);
        }

        public void SetSize(float width, float height)
        {
            Width.Set(width, 0f);
            Height.Set(height, 0f);
        }
    }
}
