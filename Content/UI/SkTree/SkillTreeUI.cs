using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Terraria.GameInput;
using Terraria.ModLoader;

namespace EthoriaMod.Content.UI.SkTree
{
    public class SkillTreeUI : ModSystem
    {
        private static RenderTarget2D cutoutSurface;
        public static Vector2 displacement = new Vector2(0, 0);
        public static float skillTreeDrawX = 0.5f;
        public static float skillTreeDrawY = 0.2f;
        public static float skillTreeWindowW = 0.5f;
        public static float skillTreeWindowH = 0.3f;

        public static bool dragging = false;
        public static int oldMouseX = -1;
        public static int oldMouseY = -1;

        
        public static void Draw(SpriteBatch spriteBatch, Player player)
        {
        
            EthoriaPlayer ethPlayer = player.GetModPlayer<EthoriaPlayer>();
            SkillTree skillTree = ethPlayer.skillTree;

            Vector2 drawPos = new Vector2(Main.screenWidth / 2.0f, 100);
            
            GraphicsDevice graphicsDevice = Main.instance.GraphicsDevice;


            int midX = Main.screenWidth / 2;
            int midY = Main.screenHeight / 2;

            int drawScreenX = (int)(((float)Main.screenWidth) * skillTreeDrawX);
            int drawScreenY = (int)(((float)Main.screenHeight) * skillTreeDrawY);

            int skillTreeWindowScreenW = (int)(skillTreeWindowW * ((float)Main.screenWidth));
            int skillTreeWindowScreenH = (int)(skillTreeWindowH * ((float)Main.screenHeight));



            Rectangle sourceRect = new Rectangle(midX - skillTreeWindowScreenW / 2, midY - skillTreeWindowScreenH / 2, skillTreeWindowScreenW, skillTreeWindowScreenH);

            Rectangle backgroundRect = new Rectangle(drawScreenX - skillTreeWindowScreenW / 2, drawScreenY - skillTreeWindowScreenH / 2, skillTreeWindowScreenW, skillTreeWindowScreenH);

            graphicsDevice.PresentationParameters.RenderTargetUsage = RenderTargetUsage.PreserveContents;

            if (cutoutSurface == null || cutoutSurface.Width != Main.screenWidth || cutoutSurface.Height != Main.screenHeight)
            {
                cutoutSurface = new RenderTarget2D(graphicsDevice, Main.screenWidth, Main.screenHeight);
                skillTree.updateChildrenPositions();
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

            Vector2 cutoutPos = new Vector2(midX - skillTreeWindowScreenW / 2, midY - skillTreeWindowScreenH / 2);
            Vector2 windowPosition = new Vector2(drawScreenX - skillTreeWindowScreenW / 2, drawScreenY - skillTreeWindowScreenH / 2);
            skillTree.drawSkillTree(spriteBatch, displacement, cutoutPos, windowPosition, backgroundRect);
         
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

            spriteBatch.Draw(TextureAssets.MagicPixel.Value, backgroundRect, Color.White);

            spriteBatch.Draw(
                cutoutSurface,
                new Vector2(drawScreenX - skillTreeWindowScreenW / 2, drawScreenY - skillTreeWindowScreenH / 2),          
                sourceRect,               
                Color.White              
            );
            MouseState ms = Mouse.GetState();
            if (backgroundRect.Contains(new Point(Main.mouseX, Main.mouseY)))
            {
                
                Main.LocalPlayer.mouseInterface = true;
                if (Main.mouseLeft && Main.mouseLeftRelease)
                {
                    dragging = true;
                }
            }
            if (!Main.mouseLeft)
            {
                dragging = false;
            }

            if (dragging)
            {
                if (oldMouseX != -1)
                {
                    displacement.X += ((float) (Main.mouseX - oldMouseX)) / Main.screenWidth;
                }
                if (oldMouseY != -1)
                {
                    displacement.Y += ((float) (Main.mouseY - oldMouseY)) / Main.screenHeight;
                }
                oldMouseX = Main.mouseX;
                oldMouseY = Main.mouseY;
            } else
            {
                oldMouseX = -1;
                oldMouseY = -1;
            }

        }
    
    }
}
