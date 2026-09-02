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
        private int borderThickness = 30;
        private int margin = 10;


        protected Asset<Texture2D> dialogueBoxTexture;
        protected Asset<Texture2D> dialogueNameTexture;
        public DialogueBox()
        {

            dialogueBoxTexture = ModContent.Request<Texture2D>("EthoriaMod/Assets/UI/Dialogue/DialogueBox");
            dialogueNameTexture = ModContent.Request<Texture2D>("EthoriaMod/Assets/UI/Dialogue/DialogueName");


            Width.Set(dialogueBoxTexture.Value.Width, 0);
            Height.Set(dialogueBoxTexture.Value.Height, 0);

            HAlign = 0.5f;
            VAlign = 1f;

            Top.Set(-175, 0);
        }

        public void SetDialogue(string speaker, string text)
        {
            this.speaker = speaker;
            this.text = text;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {

            Width.Set(dialogueBoxTexture.Value.Width, 0);
            Height.Set(dialogueBoxTexture.Value.Height, 0);


            CalculatedStyle dimensions = GetDimensions();

            spriteBatch.Draw(
                dialogueBoxTexture.Value,
                dimensions.ToRectangle(),
                Color.White
            );

            Vector2 dialogueNameSize = dialogueNameTexture.Value.Size();
            Rectangle dialogueNameRectangle = new Rectangle((int) dimensions.X + borderThickness, (int) dimensions.Y - borderThickness, (int) dialogueNameSize.X, (int) dialogueNameSize.Y);
            spriteBatch.Draw(
                dialogueNameTexture.Value,
                dialogueNameRectangle,
                Color.White
            );



            DynamicSpriteFont font = FontAssets.MouseText.Value;
            Vector2 speakerSize = font.MeasureString(speaker);

            // Speaker
            Utils.DrawBorderString(
                spriteBatch,
                speaker,
                new Vector2(
                    dialogueNameRectangle.X + ((dialogueNameRectangle.Width - speakerSize.X) / 2),
                    dialogueNameRectangle.Y + ((dialogueNameRectangle.Height - speakerSize.Y/2) / 2)
                ),
                Color.White
            );
            

            String subText = text.Substring(0, currChar);
            String drawText = "";

            String[] words = drawText.Split(' ');
            for (int i = 0; i < currChar; i++)
            {
                drawText += subText[i];

                Vector2 textSize = font.MeasureString(drawText);
                if (textSize.X >= dimensions.Width - 2 * (margin + borderThickness))
                {
                    drawText = drawText.Insert(drawText.Length - 1, "\n");
                }
            }


            // DialogueText
            Utils.DrawBorderString(
                spriteBatch,
                drawText,
                new Vector2(
                    dimensions.X + margin + borderThickness,
                    dimensions.Y + margin + borderThickness 
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
