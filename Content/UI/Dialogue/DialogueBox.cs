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
using System.Media;
using Terraria.Audio;
using Terraria.ID;
using Steamworks;

#nullable enable
namespace EthoriaMod.Content.UI.Dialogue
{
    public class DialogueBox : UIElement
    {
        private int maxTypewriterTimer = 1;
        private int maxSoundTimerMultiplier = 10; 
        private int typewriterTimer = 0;
        private int soundTimer = 0;
        private int soundClip;
        private int currChar = 0;
        private int borderThickness = 30;
        private int margin = 10;
        private int arrowMargin = 5;
        private int nametagOffset = 7;
        private float scale = 0.75f;
        public float BoxWidth => dialogueBoxTexture.Value.Width * scale;
        public float BoxHeight => dialogueBoxTexture.Value.Height * scale;
        private DialogueSession? session;

        protected Asset<Texture2D> dialogueBoxTexture;
        protected Asset<Texture2D> dialogueNameTexture;
        protected Asset<Texture2D> continueArrowTexture;

        // arrow
        private float arrowTimer = 0f;
        private bool showContinueArrow = false;
        private float bounce = 0f;

        public bool IsTextFinished
        {
            get
            {
                if (session == null) return true;
                string text = session.CurrentNode.Text ?? "";
                return currChar >= text.Length;
            }
        }

        public void SetSession(DialogueSession? session)
        {
            this.session = session;
            ResetWriter();
        }
        public DialogueBox(float yOffset)
        {

            dialogueBoxTexture = ModContent.Request<Texture2D>("EthoriaMod/Assets/UI/Dialogue/DialogueBox");
            dialogueNameTexture = ModContent.Request<Texture2D>("EthoriaMod/Assets/UI/Dialogue/DialogueName");
            continueArrowTexture = ModContent.Request<Texture2D>("EthoriaMod/Assets/UI/Dialogue/DialogueArrow");

            Width.Set(dialogueBoxTexture.Value.Width * scale, 0);
            Height.Set(dialogueBoxTexture.Value.Height * scale, 0);

            HAlign = 0.5f;
            VAlign = 1f;

            Top.Set(yOffset, 0);
        }

        public void FinishText()
        {
            if (session == null) { return; }
            SoundEngine.PlaySound(SoundID.Item127);
            string text = session.CurrentNode.Text ?? "";
            currChar = text.Length;
        }

        public void ResetWriter()
        {
            currChar = 0;
            typewriterTimer = 0;
        }

        private void UpdateArrow(DialogueNode node)
        {
            bounce = MathF.Sin(arrowTimer) * 2f;
            arrowTimer += 0.1f;

            showContinueArrow =
                IsTextFinished &&
                node.Prompts.Count == 0 &&
                node.NextNode != null;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (session == null)
            {
                showContinueArrow = false;
                return;
            }

            DialogueNode node = session.CurrentNode;

            UpdateArrow(node);

            Width.Set(dialogueBoxTexture.Value.Width * scale, 0);
            Height.Set(dialogueBoxTexture.Value.Height * scale, 0);

            CalculatedStyle dimensions = GetDimensions();

            int scaledMargin = (int)(margin * scale);
            int scaledBorderThickness = (int)(borderThickness * scale);

            spriteBatch.Draw(
                dialogueBoxTexture.Value,
                dimensions.ToRectangle(),
                Color.White
            );

            if (showContinueArrow)
            {
                Vector2 position = new Vector2(
                    dimensions.X + dimensions.Width - borderThickness - arrowMargin - continueArrowTexture.Value.Width,
                    dimensions.Y + dimensions.Height - borderThickness - arrowMargin - continueArrowTexture.Value.Height + bounce
                );

                spriteBatch.Draw(
                    continueArrowTexture.Value,
                    position,
                    Color.White
                );
            }

            string speaker = node.Speaker ?? "";
            string text = node.Text ?? "";

            Vector2 dialogueNameSize = dialogueNameTexture.Value.Size();
            Vector2 dialogueNamePos = new Vector2(dimensions.X + scaledBorderThickness, dimensions.Y - scaledBorderThickness);
            Rectangle dialogueNameRectangle = new Rectangle((int)dialogueNamePos.X + nametagOffset, (int)dialogueNamePos.Y + nametagOffset, (int)((float)dialogueNameSize.X * scale), (int)((float)dialogueNameSize.Y * scale));
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
                    dialogueNameRectangle.Y + ((dialogueNameRectangle.Height - speakerSize.Y / 2) / 2)
                ),
                Color.White
            );

            String subText = text.Substring(0, currChar);
            String drawText = "";

            string[] words = subText.Split(' ');

            string currentLine = "";

            foreach (string word in words)
            {
                string testLine = currentLine.Length == 0
                    ? word
                    : currentLine + " " + word;

                Vector2 textSize = font.MeasureString(testLine);

                if (textSize.X >= dimensions.Width - 2 * (margin + borderThickness))
                {
                    drawText += currentLine + "\n";
                    currentLine = word;
                }
                else
                {
                    currentLine = testLine;
                }
            }

            drawText += currentLine;

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

            if (soundTimer > 0)
            {
                soundTimer--;
            } else
            {
                if (!IsTextFinished)
                {
                    soundClip = Main.rand.Next(1, 3);
                    SoundEngine.PlaySound(new SoundStyle($"EthoriaMod/Assets/Sounds/Dialogue/baseTalk{soundClip}").WithVolumeScale(0.4f) with
                    {
                        MaxInstances = 0
                    });
                    soundTimer = maxTypewriterTimer * maxSoundTimerMultiplier;
                }
            }

            if (typewriterTimer > 0)
            {
                typewriterTimer--;
            }
            else
            {
                if (currChar < text.Length)
                {
                    currChar++;

                    

                    //if (!playedTextSound)
                    //{
                    //    playedTextSound = true;
                    //    //SoundEngine.PlaySound(new SoundStyle("EthoriaMod/Assets/Sounds/Dialogue/baseTalk") with
                    //    //{
                    //    //    MaxInstances = 0
                    //    //});
                    //}

                    typewriterTimer = maxTypewriterTimer;
                }

            }
        }
    }
}
