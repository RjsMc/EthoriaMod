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

namespace EthoriaMod.Common.Developer
{
    public class HelperFunctions
    {
        public static void drawLine(SpriteBatch spriteBatch, Vector2 start, Vector2 end, Color color, float width = 1)
        {
            if (start == end)
                return;

            Texture2D pixel = ModContent.Request<Texture2D>("EthoriaMod/Common/Developer/Pixel").Value;
            float rotation = (end - start).ToRotation();
            Vector2 scale = new Vector2(Vector2.Distance(start, end) / pixel.Width, width);

            spriteBatch.Draw(pixel, start, null, color, rotation, pixel.Size() * Vector2.UnitY * 0.5f, scale, SpriteEffects.None, 0f);
        }
    }
}
