using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.UI;
using ReLogic.Content;

namespace EthoriaMod.Content.UI.Dialogue
{
    public class DialogueBox : UIElement
    {
        private string speaker = "Jonathan";
        private string text = "OOGABOOGAOOGABOOGAOOGABOOGAOOGABOOGAOOGABOOGAOOGABOOGAOOGABOOGAOOGABOOGAOOGABOOGAOOGABOOGAOOGABOOGAOOGABOOGAOOGABOOGAOOGABOOGAOOGABOOGAOOGABOOGAOOGABOOGAOOGABOOGA";

        private int maxTypewriterTimer = 5;
        private int typewriterTimer = 0;
        private int currChar = 0;
        private int margin = 0;

        private Asset<Texture2D> dialogueBoxTexture;
        public DialogueBox()
        {
            Width.Set(715, 0);
            Height.Set(208, 0);

            HAlign = 0.5f;
            VAlign = 1f;

            Top.Set(-175, 0);

            dialogueBoxTexture = ModContent.Request<Texture2D>("EthoriaMod/Assets/UI/Dialogue/DialogueBox");
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
                dialogueBoxTexture.Value,
                dimensions.ToRectangle(),
                Color.White
            );

            // Speaker
            Utils.DrawBorderString(
                spriteBatch,
                speaker,
                new Vector2(
                    dimensions.X + 80,
                    dimensions.Y + margin
                ),
                Color.White
            );
            DynamicSpriteFont font = FontAssets.MouseText.Value;
            Vector2 speakerSize = font.MeasureString(speaker);
            

            String subText = text.Substring(0, currChar);
            String drawText = "";

            String[] words = drawText.Split(' ');
            for (int i = 0; i < currChar; i++)
            {
                drawText += subText[i];

                Vector2 textSize = font.MeasureString(drawText);
                if (textSize.X >= dimensions.Width - 2 * margin)
                {
                    drawText = drawText.Insert(drawText.Length - 1, "\n");
                }
            }


            // DialogueText
            Utils.DrawBorderString(
                spriteBatch,
                drawText,
                new Vector2(
                    dimensions.X + margin,
                    dimensions.Y + margin + speakerSize.Y
                ),
                Color.White
            );
            
            if (typewriterTimer > 0)
            {
                typewriterTimer--;
            } else
            {
                if (currChar < text.Length)
                {
                    currChar++;
                    typewriterTimer = maxTypewriterTimer;
                }

            }
                base.DrawSelf(spriteBatch);
        }
    }
}
