using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EthoriaMod.Content.EthPlayer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;

namespace EthoriaMod.Content.UI
{
    public class ExpBarUI : ModSystem
    {
        private static float maxBarW = 100.0f;
        private static Texture2D barBackgroundTex, barTex;

        public override void OnModLoad()
        {
            barTex = ModContent.Request<Texture2D>("EthoriaMod/Content/UI/BarFill", AssetRequestMode.ImmediateLoad).Value;

        }

        public static void Draw(SpriteBatch spriteBatch, Player player)
        {


            Vector2 drawPos = new Vector2(Main.screenWidth / 2.0f, 100);

            EthoriaPlayer ethPlayer = player.GetModPlayer<EthoriaPlayer>();

            float expPercent = (float) ethPlayer.currentExp / (float) ethPlayer.expToLevelUp();

            Rectangle barRectangle = new Rectangle(0, 0, barTex.Width, barTex.Height);
            spriteBatch.Draw(barTex, drawPos, barRectangle, Color.White, 0.0f, new Vector2(0, 0), new Vector2(maxBarW * expPercent, 1.0f), SpriteEffects.None, 0);

            String expBarStr = "Lv " + ethPlayer.level.ToString() + " - " + ethPlayer.currentExp.ToString() + "/" + ethPlayer.expToLevelUp().ToString();
            Utils.DrawBorderString(Main.spriteBatch, expBarStr, drawPos, Color.White);



        }
    }
}
