using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.UI;

namespace EthoriaMod.Content.UI.Dialogue
{
    public class DialoguePromptButton : UIElement
    {
        private Asset<Texture2D> promptTexture;
        private Asset<Texture2D> promptHoverTexture;
        private bool hovered;

        private float scale = 0.75f;

        private string text;

        public int PromptIndex { get; }

        public float PromptWidth => scale;
        public float PromptHeight => scale;

        public DialoguePromptButton(int promptIndex, string text)
        {
            PromptIndex = promptIndex;
            this.text = text;

            promptTexture = ModContent.Request<Texture2D>(
                "EthoriaMod/Assets/UI/Dialogue/DialoguePromptUnhover"
            );

            promptHoverTexture = ModContent.Request<Texture2D>(
                "EthoriaMod/Assets/UI/Dialogue/DialoguePromptHover"
            );

            OnMouseOver += (_, _) =>
            {
                hovered = true;
                Main.NewText($"Mouse Out Prompt {PromptIndex}");
            };
            
            OnMouseOut += (_, _) =>
            {
                hovered = false;
                Main.NewText($"Mouse Out Prompt {PromptIndex}");
            };
            
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            Width.Set(promptTexture.Value.Width * scale, 0);

            Height.Set(promptTexture.Value.Height * scale, 0);

            CalculatedStyle dimensions = GetDimensions();
            Texture2D texture = hovered ? promptHoverTexture.Value : promptTexture.Value; 

            spriteBatch.Draw(
                texture,
                dimensions.ToRectangle(),
                Color.White
            );

            // Printing text
            DynamicSpriteFont font = FontAssets.MouseText.Value;
            Vector2 textSize = font.MeasureString(text);

            Vector2 textPosition = new Vector2(dimensions.X + (dimensions.Width - textSize.X) / 2f, dimensions.Y + (dimensions.Height - textSize.Y) / 1.5f);
            Utils.DrawBorderString(spriteBatch, text, textPosition, Color.White);

        }
    }
}