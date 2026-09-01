
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

        public static List<int> exclude = [3854, ItemID.Phantasm, ItemID.Tsunami, ItemID.FairyQueenRangedItem, ItemID.HellwingBow, ItemID.PulseBow, ItemID.GoldBow];
        public override void SetDefaults(Item item)
        {
            if (!exclude.Contains(item.type) && item.useAmmo == AmmoID.Arrow && item.shoot > ProjectileID.None)
            {

                item.useTime = item.useAnimation;
                item.useStyle = ItemUseStyleID.Shoot;

                item.channel = true;
                
                item.shoot = ModContent.ProjectileType<KnockedArrow>();
                item.UseSound = null;
            }
        }

        public override bool CanConsumeAmmo(Item weapon, Item ammo, Player player)
        {
       
            if (!exclude.Contains(weapon.type) && weapon.useAmmo == AmmoID.Arrow && weapon.shoot > ProjectileID.None)
            {
                return false;
            }
            return true;
        }   

        
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (!exclude.Contains(item.type) && item.useAmmo == AmmoID.Arrow && item.shoot > ProjectileID.None)
            {
                int consumedAmmoId = source.AmmoItemIdUsed;
                int firedProj = Projectile.NewProjectile(source, position, velocity, item.shoot, damage, knockback, player.whoAmI);
                KnockedArrow theArrow = (KnockedArrow)(Main.projectile[firedProj].ModProjectile);
                theArrow.minDrawDepth = int.Min(item.width, item.height) + 5 + 15;
                theArrow.maxDrawDepth = int.Min(item.width, item.height) + 20 + 15;
                theArrow.minShootStrength = minShootStrength;
                theArrow.maxShootStrength = velocity.Length(); 
                theArrow.maxChargingFrames = item.useTime;
                theArrow.autoReuse = item.autoReuse;
                theArrow.itemID = consumedAmmoId;
                theArrow.actualType = type;
                theArrow.minChargingFrames = int.Min(item.useAnimation, 10);


                return false;
            }

            return true;
            
        } 
    }
}
