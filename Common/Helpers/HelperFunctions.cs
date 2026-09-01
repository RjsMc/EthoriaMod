using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
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

        public static void consumeAmmoForced(Player player, Item item, int ammoConsumed)
        {
            Item itemAmmo = new Item();
            bool hasEnoughAmmo = false;
            bool dontConsumeAmmo = false;

            for (int i = 54; i < Main.InventorySlotsTotal; i++)
            {
                if (player.inventory[i].ammo == item.useAmmo && (player.inventory[i].stack >= ammoConsumed || !player.inventory[i].consumable))
                {
                    itemAmmo = player.inventory[i];
                    hasEnoughAmmo = true;
                    break;
                }
            }

            if (!hasEnoughAmmo)
            {
                for (int j = 0; j < 54; j++)
                {
                    if (player.inventory[j].ammo == item.useAmmo && (player.inventory[j].stack >= ammoConsumed || !player.inventory[j].consumable))
                    {
                        itemAmmo = player.inventory[j];
                        break;
                    }
                }
            }

            if (player.magicQuiver && (item.useAmmo == AmmoID.Arrow || item.useAmmo == AmmoID.Stake) && Main.rand.NextBool(5))
                dontConsumeAmmo = true;
            if (player.huntressAmmoCost90 && Main.rand.NextBool(10))
                dontConsumeAmmo = true;
            if (player.ammoBox && Main.rand.NextBool(5))
                dontConsumeAmmo = true;
            if (player.ammoPotion && Main.rand.NextBool(5))
                dontConsumeAmmo = true;
            if (player.ammoCost80 && Main.rand.NextBool(5))
                dontConsumeAmmo = true;
            if (player.chloroAmmoCost80 && Main.rand.NextBool(5))
                dontConsumeAmmo = true;
            if (player.ammoCost75 && Main.rand.NextBool(4))
                dontConsumeAmmo = true;
          

            if (!dontConsumeAmmo && itemAmmo.consumable)
            {
                itemAmmo.stack -= ammoConsumed;
                if (itemAmmo.stack <= 0)
                {
                    itemAmmo.active = false;
                    itemAmmo.TurnToAir();
                }
            }
        }

    }
}
