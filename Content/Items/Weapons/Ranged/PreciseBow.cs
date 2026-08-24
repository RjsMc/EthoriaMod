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
        public override void SetDefaults()
        {
            Item.width = 48;
            Item.height = 96;
            Item.damage = 770;
            Item.DamageType = DamageClass.Ranged;
            Item.useAnimation = Item.useTime = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 4.25f;
            Item.rare = ItemRarityID.Purple;

            Item.autoReuse = true;
            Item.shootSpeed = 15f;
            Item.shoot = ProjectileID.Bullet;
            Item.useAmmo = AmmoID.Arrow;
        }

   
        
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Main.NewText(source);
            return false;
        }
        

    }
}

