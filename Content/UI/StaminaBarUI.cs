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
    public class StaminaBarUI : ModSystem
    {
        private static float maxBarW = 100.0f;
        private static Texture2D barBackgroundTex, barTex;
        public override void OnModLoad()
        {
            barTex = ModContent.Request<Texture2D>("EthoriaMod/Content/UI/StaminaBarFill", AssetRequestMode.ImmediateLoad).Value;
         
        }

        public override void Unload()
        {
        }

        public static void Draw(SpriteBatch spriteBatch, Player player)
        {


            Vector2 drawPos = new Vector2(Main.screenWidth / 2.0f, Main.screenHeight / 2.0f);

            EthoriaPlayer ethPlayer = player.GetModPlayer<EthoriaPlayer>();
            float staminaPercent = ethPlayer.stamina / ethPlayer.maxStamina;
            Rectangle barRectangle = new Rectangle(0, 0, barTex.Width, barTex.Height);
            spriteBatch.Draw(barTex, drawPos, barRectangle, Color.White, 0.0f, new Vector2(0, 0), new Vector2(maxBarW * staminaPercent, 1.0f), SpriteEffects.None, 0);
            
        }
    }
}
