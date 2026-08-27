using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace EthoriaMod.Content.Items.Weapons.Ranged
{
    public class PreciseBow : ModItem
    {
        public new string LocalizationCategory => "Items.Weapons.Ranged";
        public float minShootStrength = 1.0f;
        public float maxShootStrength = 5.0f;
        public override void SetDefaults()
        {
            Item.width = 7;
            Item.height = 25;
            Item.damage = 770;
            Item.DamageType = DamageClass.Ranged;
            Item.useAnimation = Item.useTime = 30;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 4.25f;
            Item.rare = ItemRarityID.Purple;
            Item.autoReuse = true;

            Item.shoot = ModContent.ProjectileType<KnockedArrow>();
            Item.shootSpeed = 5f;
            Item.useAmmo = AmmoID.Arrow;
        }

        public override bool CanUseItem(Player player) => (player.itemAnimation == 0);

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int consumedAmmoId = source.AmmoItemIdUsed;
            int firedProj = Projectile.NewProjectile(source, position, velocity, Item.shoot, damage, knockback, player.whoAmI);
            KnockedArrow theArrow = (KnockedArrow) (Main.projectile[firedProj].ModProjectile);
            theArrow.minDrawDepth = Item.width + 5 + 15;
            theArrow.maxDrawDepth = Item.width + 20 + 15;
            theArrow.minShootStrength = 1;
            theArrow.maxShootStrength = 10;
            theArrow.maxChargingFrames = Item.useTime;
            theArrow.itemID = consumedAmmoId;
            theArrow.actualType = type;
            return false;

            
        } 
        

    }
}

