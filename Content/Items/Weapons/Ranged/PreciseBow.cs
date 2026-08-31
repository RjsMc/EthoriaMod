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
   
        public override void SetDefaults()
        {
            Item.width = 7;
            Item.height = 25;
            Item.damage = 770;
            Item.DamageType = DamageClass.Ranged;
            Item.useAnimation = Item.useTime = 4;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 4.25f;
            Item.rare = ItemRarityID.Purple;
            Item.autoReuse = true;

            Item.shoot = ModContent.ProjectileType<KnockedArrow>();
            Item.shootSpeed = 100f;
            Item.useAmmo = AmmoID.Arrow;
        }

        
    }
}

