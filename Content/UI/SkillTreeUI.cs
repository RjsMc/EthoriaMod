using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EthoriaMod.Content.EthPlayer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Steamworks;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace EthoriaMod.Content.UI
{
    public class SkillTreeUI : ModSystem
    {
        private static RenderTarget2D cutoutSurface;
        public static Vector2 displacement = new Vector2(0, 0);
        public static Vector2 windowPosition = new Vector2(0, 0);
        public static int skillTreeWindowW = 100;
        public static int skillTreeWindowH = 100;

        public static bool dragging = false;
        public static void Draw(SpriteBatch spriteBatch, Player player)
        {

            EthoriaPlayer ethPlayer = player.GetModPlayer<EthoriaPlayer>();
            SkillTree skillTree = ethPlayer.skillTree;
            Vector2 drawPos = new Vector2(Main.screenWidth / 2.0f, 100);
            
            GraphicsDevice graphicsDevice = Main.instance.GraphicsDevice;
            graphicsDevice.PresentationParameters.RenderTargetUsage = RenderTargetUsage.PreserveContents;

            if (cutoutSurface == null)
            {
                cutoutSurface = new RenderTarget2D(graphicsDevice, Main.screenWidth, Main.screenHeight);
            }
            spriteBatch.End();

            graphicsDevice.SetRenderTarget(cutoutSurface);
            graphicsDevice.Clear(Color.Transparent);


            spriteBatch.Begin(
                SpriteSortMode.Immediate,
                BlendState.AlphaBlend,
                Main.DefaultSamplerState,
                DepthStencilState.None,
                Main.Rasterizer,
                null,
                Main.GameViewMatrix.EffectMatrix);

            Utils.DrawBorderString(spriteBatch, "Test", new Vector2(Main.screenWidth / 2.0f, Main.screenHeight / 2.0f) + displacement, Color.White);
            spriteBatch.End();

            graphicsDevice.SetRenderTarget(null);

            spriteBatch.Begin(
                SpriteSortMode.Immediate,
                BlendState.AlphaBlend, 
                Main.DefaultSamplerState, 
                DepthStencilState.None, 
                Main.Rasterizer, 
                null,
                Main.GameViewMatrix.EffectMatrix);

            int midX = Main.screenWidth / 2;
            int midY = Main.screenHeight / 2;
            Rectangle sourceRect = new Rectangle(midX - skillTreeWindowW / 2, midY - skillTreeWindowH / 2, skillTreeWindowW, skillTreeWindowH);

            spriteBatch.Draw(TextureAssets.MagicPixel.Value, sourceRect, Color.White);

            spriteBatch.Draw(
                cutoutSurface,
                new Vector2(midX - skillTreeWindowW / 2, midY - skillTreeWindowH / 2),          
                sourceRect,               
                Color.White              
            );
            MouseState ms = Mouse.GetState();
            if (Main.mouseLeft)
            {
                //Main.LocalPlayer.mouseInterface = true;
                dragging = true;
            } else
            {
                dragging = false;
            }


            if (dragging)
            {
                displacement.X++;
            }
        }
    
    }
}
