using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace EthoriaMod.Content.UI
{
    
    public class MouseStrUI : ModSystem
    {
        public static string mouseStr = "";
        public static void Draw(SpriteBatch spriteBatch, Player player)
        {
            int d = 16;
            Utils.DrawBorderString(spriteBatch, mouseStr, new Vector2(Main.mouseX + d, Main.mouseY + d), Color.White);
            mouseStr = "";
        }
    }
}
