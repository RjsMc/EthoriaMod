using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
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

            Width.Set(
                promptTexture.Value.Width * scale,
                0
            );

            Height.Set(
                promptTexture.Value.Height * scale,
                0
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
            Width.Set(
                promptTexture.Value.Width * scale,
                0
            );

            Height.Set(
                promptTexture.Value.Height * scale,
                0
            );

            CalculatedStyle dimensions = GetDimensions();
            Texture2D texture = hovered ? promptHoverTexture.Value : promptTexture.Value; 

            spriteBatch.Draw(
                texture,
                dimensions.ToRectangle(),
                Color.White
            );
        }
    }
}