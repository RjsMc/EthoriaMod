
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
namespace EthoriaMod.Content.Items.Weapons.Ranged
{
    internal class DrawnBow : GlobalItem
    {
        
        public static float minShootStrength = 1.0f;
        public override void SetDefaults(Item item)
        {
            if (item.useAmmo == AmmoID.Arrow && item.shoot > ProjectileID.None)
            {
                item.shoot = ModContent.ProjectileType<KnockedArrow>();
                item.UseSound = null;
            }
           

            

            
        }
   

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (item.useAmmo == AmmoID.Arrow && item.shoot > ProjectileID.None)
            {
                int consumedAmmoId = source.AmmoItemIdUsed;
                int firedProj = Projectile.NewProjectile(source, position, velocity, item.shoot, damage, knockback, player.whoAmI);
                KnockedArrow theArrow = (KnockedArrow)(Main.projectile[firedProj].ModProjectile);
                theArrow.minDrawDepth = item.width + 5 + 15;
                theArrow.maxDrawDepth = item.width + 20 + 15;
                theArrow.minShootStrength = minShootStrength;
                theArrow.maxShootStrength = item.shootSpeed;
                theArrow.maxChargingFrames = item.useTime;
                theArrow.itemID = consumedAmmoId;
                theArrow.actualType = type;
      
                return false;
            }

            return true;
            
        } 
    }
}
