using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EthoriaMod.Common.Developer;
using EthoriaMod.Content.EthPlayer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Steamworks;
using Terraria;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.UI;

namespace EthoriaMod.Content.UI.SkTree
{
    public class SkillTreeUI : ModSystem
    {
        public enum SkillTreeState
        {
            Closed,
            Open,
            EnumSize
        }

        private static RenderTarget2D cutoutSurface;
        public static Vector2 displacement = new Vector2(0, 0);
        public static float skillTreeDrawX = 0.5f;
        public static float skillTreeDrawY = 0.5f;
        public static float skillTreeWindowW = 0.5f;
        public static float skillTreeWindowH = 0.5f;

        public static bool dragging = false;
        public static int oldMouseX = -1;
        public static int oldMouseY = -1;

        public static int oldMouseScroll = 0;

        public static float skillTreeZoom = 1;

        public static SkillTreeState state = SkillTreeState.Closed;
        
        public static void Draw(SpriteBatch spriteBatch, Player player)
        {

            EthoriaPlayer ethPlayer = player.GetModPlayer<EthoriaPlayer>();
            SkillTree skillTree = ethPlayer.skillTree;
            GraphicsDevice graphicsDevice = Main.instance.GraphicsDevice;

            int midX = Main.screenWidth / 2;
            int midY = Main.screenHeight / 2;


            switch (state)
            {
                case SkillTreeState.Closed:
                    if (EthoriaKeybinds.skillTreeHotKey.JustPressed)
                    {
                        state = SkillTreeState.Open;
                    }
                    break;

                case SkillTreeState.Open:
                    if (EthoriaKeybinds.skillTreeHotKey.JustPressed)
                    {
                        state = SkillTreeState.Closed;
                    }

                    Vector2 drawPos = new Vector2(Main.screenWidth / 2.0f, 100);

                    int drawScreenX = (int)(((float)Main.screenWidth) * skillTreeDrawX);
                    int drawScreenY = (int)(((float)Main.screenHeight) * skillTreeDrawY);

                    int skillTreeWindowScreenW = (int)(skillTreeWindowW * ((float)Main.screenWidth));
                    int skillTreeWindowScreenH = (int)(skillTreeWindowH * ((float)Main.screenHeight));

                    int cutoutWindowW = (int) (((float) skillTreeWindowScreenW) * skillTreeZoom);
                    int cutoutWindowH = (int) (((float) skillTreeWindowScreenH) * skillTreeZoom);
                 

                    Rectangle sourceRect = new Rectangle(midX - cutoutWindowW / 2, midY - cutoutWindowH / 2, cutoutWindowW, cutoutWindowH);

                    Rectangle backgroundRect = new Rectangle(drawScreenX - skillTreeWindowScreenW / 2, drawScreenY - skillTreeWindowScreenH / 2, skillTreeWindowScreenW, skillTreeWindowScreenH);

                    graphicsDevice.PresentationParameters.RenderTargetUsage = RenderTargetUsage.PreserveContents;

                    if (cutoutSurface == null || cutoutSurface.Width != Main.screenWidth || cutoutSurface.Height != Main.screenHeight)
                    {
                        cutoutSurface = new RenderTarget2D(graphicsDevice, Main.screenWidth, Main.screenHeight);
                        skillTree.updatePositions();
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
                    skillTree.DrawSkillTree(spriteBatch, displacement, cutoutPos, windowPosition, backgroundRect);

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
                        backgroundRect,
                        sourceRect,
                        Color.White
                    );

                    if (backgroundRect.Contains(new Point(Main.mouseX, Main.mouseY)))
                    {
                        MouseState mouseState = Mouse.GetState();
                        int mouseScroll = mouseState.ScrollWheelValue;

                        Main.LocalPlayer.mouseInterface = true;

                        PlayerInput.LockVanillaMouseScroll("SkillTreeUI");

                        if (Main.mouseLeft && Main.mouseLeftRelease)
                        {
                            dragging = true;
                        }
                        int dScroll = mouseScroll - oldMouseScroll;
                       
                        oldMouseScroll = mouseScroll;
                        skillTreeZoom -= int.Sign(dScroll) * 0.1f;
                        skillTreeZoom = float.Clamp(skillTreeZoom, 0.25f, 2);
                    }
                    if (!Main.mouseLeft)
                    {
                        dragging = false;
                    }

                    if (dragging)
                    {
                        if (oldMouseX != -1)
                        {
                            displacement.X += (((float)(Main.mouseX - oldMouseX)) / Main.screenWidth) * skillTreeZoom;
                        }
                        if (oldMouseY != -1)
                        {
                            displacement.Y += (((float)(Main.mouseY - oldMouseY)) / Main.screenHeight) * skillTreeZoom;
                        }
                        oldMouseX = Main.mouseX;
                        oldMouseY = Main.mouseY;
                    }
                    else
                    {
                        oldMouseX = -1;
                        oldMouseY = -1;
                    }

                    break;
            }
            

            
        }
    
    }
}
