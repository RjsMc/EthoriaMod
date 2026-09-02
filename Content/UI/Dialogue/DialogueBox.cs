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
using EthoriaMod.Content.Dialogue;

#nullable enable
namespace EthoriaMod.Content.UI.Dialogue
{
    public class DialogueBox : UIElement
    {
        private string speaker = "";
        private string text = "";

        private int maxTypewriterTimer = 5;
        private int typewriterTimer = 0;
        private int currChar = 0;
        private int borderThickness = 30;
        private int margin = 10;
        private int nametagOffset = 7;

        private float scale = 0.75f;
        public float BoxWidth => dialogueBoxTexture.Value.Width * scale;
        public float BoxHeight => dialogueBoxTexture.Value.Height * scale;
        private DialogueSession? session;
        
        protected Asset<Texture2D> dialogueBoxTexture;
        protected Asset<Texture2D> dialogueNameTexture;

        public void SetSession(DialogueSession? session)
        {
            this.session = session;

            if (session == null)
            {
                speaker = "";
                text = "";
                currChar = 0;
                typewriterTimer = 0;
                return;
            }

            speaker = session.CurrentNode.Speaker ?? "%null%";
            text = session.CurrentNode.Text ?? "%null%";
            currChar = 0;
            typewriterTimer = 0;
        }
        public DialogueBox()
        {

            dialogueBoxTexture = ModContent.Request<Texture2D>("EthoriaMod/Assets/UI/Dialogue/DialogueBox");
            dialogueNameTexture = ModContent.Request<Texture2D>("EthoriaMod/Assets/UI/Dialogue/DialogueName");


            Width.Set(dialogueBoxTexture.Value.Width * scale, 0);
            Height.Set(dialogueBoxTexture.Value.Height * scale, 0);

            HAlign = 0.5f;
            VAlign = 1f;

            Top.Set(-130, 0);
        }

        public void SetDialogue(string speaker, string text)
        {
            this.speaker = speaker;
            this.text = text;
            currChar = 0;
            typewriterTimer = 0;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {

            Width.Set(dialogueBoxTexture.Value.Width * scale, 0);
            Height.Set(dialogueBoxTexture.Value.Height * scale, 0);

            int scaledMargin = (int) (margin * scale);
            int scaledBorderThickness = (int) (borderThickness * scale);


            CalculatedStyle dimensions = GetDimensions();

            spriteBatch.Draw(
                dialogueBoxTexture.Value,
                dimensions.ToRectangle(),
                Color.White
            );

            Vector2 dialogueNameSize = dialogueNameTexture.Value.Size();
            Vector2 dialogueNamePos = new Vector2(dimensions.X + scaledBorderThickness, dimensions.Y - scaledBorderThickness);
            Rectangle dialogueNameRectangle = new Rectangle((int) dialogueNamePos.X + nametagOffset, (int) dialogueNamePos.Y + nametagOffset, (int) ((float) dialogueNameSize.X * scale), (int) ((float) dialogueNameSize.Y * scale));
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
                if (textSize.X >= dimensions.Width - 2 * (scaledMargin + scaledBorderThickness))
                {
                    drawText = drawText.Insert(drawText.Length - 1, "\n");
                }
            }


            // DialogueText
            Utils.DrawBorderString(
                spriteBatch,
                drawText,
                new Vector2(
                    dimensions.X + scaledMargin + scaledBorderThickness,
                    dimensions.Y + scaledMargin + scaledBorderThickness + nametagOffset
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
