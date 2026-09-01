using JetBrains.Annotations;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent;
using Terraria.UI;
using Terraria;

namespace EthoriaMod.Content.UI.Dialogue
{
    public class DialogueBox : UIElement
    {
        private string speaker = "Jonathan";
        private string text = "OOGABOOGAOOGABOOGAOOGABOOGAOOGABOOGAOOGABOOGAOOGABOOGAOOGABOOGAOOGABOOGAOOGABOOGAOOGABOOGAOOGABOOGAOOGABOOGAOOGABOOGAOOGABOOGAOOGABOOGAOOGABOOGAOOGABOOGAOOGABOOGA";

        public DialogueBox()
        {
            Width.Set(800, 0);
            Height.Set(180, 0);

            HAlign = 0.5f;
            VAlign = 1f;

            Top.Set(-125, 0);
        }

        public void SetDialogue(string speaker, string text)
        {
            this.speaker = speaker;
            this.text = text;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle dimensions = GetDimensions();

            spriteBatch.Draw(
                TextureAssets.MagicPixel.Value,
                dimensions.ToRectangle(),
                Color.Azure * 0.55f
            );

            // Speaker
            Utils.DrawBorderString(
                spriteBatch,
                speaker,
                new Vector2(
                    dimensions.X + 20f,
                    dimensions.Y + 15f
                ),
                Color.White
            );

            // DialogueText
            Utils.DrawBorderString(
                spriteBatch,
                text,
                new Vector2(
                    dimensions.X + 20f,
                    dimensions.Y + 55f
                ),
                Color.White
            );

            base.DrawSelf( spriteBatch );
        }
    }
}
