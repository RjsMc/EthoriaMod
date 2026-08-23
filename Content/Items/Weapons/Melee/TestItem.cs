using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EthoriaMod.Content.Items.Weapons.Melee
{
    internal class TestItem : ModItem
    {
        public override void SetDefaults()
        {
            Item.Size = new Vector2(32, 32);
            Item.DamageType = DamageClass.Melee;

            Item.damage = 1;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.autoReuse = true;
            Item.crit = 96;

            Item.scale = 2.00f;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            int d = Dust.NewDust(hitbox.TopLeft(), hitbox.Width, hitbox.Height, DustID.BlueTorch);
            Dust dust = Main.dust[d];
            dust.noGravity = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.CopperShortsword, 1)
                .AddIngredient(ItemID.CopperOre, 999)
                .AddTile(TileID.WorkBenches)
                .Register();
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hit.Crit)
            {
                for (int i = 0; i < 20; i++)
                {
                    Dust.NewDustPerfect(target.Center, DustID.BlueTorch, Main.rand.NextVector2Circular(5f, 5f)).noGravity = true;
                }

                Item.NewItem(player.GetSource_OnHit(target), target.getRect(), ItemID.Zenith);
            }
        }

    }
}
