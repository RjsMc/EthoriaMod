using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.UI;

namespace EthoriaMod.Content.UI.Dialogue
{
    internal class DialogueButton : UIElement
    {
        private string text;
        public DialogueButton(string text)
        {
            this.text = text;

            Width.Set(300, 0);
            Height.Set(50, 0);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle dimensions = GetDimensions();

            spriteBatch.Draw(
                TextureAssets.MagicPixel.Value,
                dimensions.ToRectangle(),
                Color.DarkGray
            );

            Utils.DrawBorderString(
                spriteBatch,
                text,
                new Vector2(
                    dimensions.X + 15f,
                    dimensions.Y + 12f
                ),
                Color.White
            );

            base.DrawSelf( spriteBatch );
        }
    }
}
