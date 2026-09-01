using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace EthoriaMod.Common.Helpers
{
    public class HelperFunctions
    {
        public static Texture2D pixelTexture()
        {
            return ModContent.Request<Texture2D>("EthoriaMod/Common/Helpers/Pixel").Value;
        }
        public static void drawLine(SpriteBatch spriteBatch, Vector2 start, Vector2 end, Color color, float width = 1)
        {
            if (start == end)
                return;

            Texture2D pixel = pixelTexture();
            float rotation = (end - start).ToRotation();
            Vector2 scale = new Vector2(Vector2.Distance(start, end) / pixel.Width, width);

            spriteBatch.Draw(pixel, start, null, color, rotation, pixel.Size() * Vector2.UnitY * 0.5f, scale, SpriteEffects.None, 0f);
        }
    }
}
