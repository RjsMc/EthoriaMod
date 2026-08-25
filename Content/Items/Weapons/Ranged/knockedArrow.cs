using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace EthoriaMod.Content.Items.Weapons.Ranged
{
    public class knockedArrow : ModProjectile
    {


        private Player Owner => Main.player[Projectile.owner];
        public override string Texture => "EthoriaMod/Content/Items/Weapons/Ranged/knockedArrow";
        private float interpSpd = 0.5f;
       
        public override void SetDefaults()
        {
            Projectile.width = 7;
            Projectile.height = 25;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.ignoreWater = true;

        }
     
        public override void AI()
        {

            if (Projectile.aiStyle == ProjAIStyleID.Arrow)
            {
                return;
            }
            Vector2 shootDirection = Projectile.velocity.SafeNormalize(Vector2.UnitX * Owner.direction);
            Vector2 armPosition = Owner.RotatedRelativePoint(Owner.MountedCenter, true);
            Vector2 tipPosition = armPosition + shootDirection * Projectile.width * 0.5f;

            if (!Main.mouseLeft || Owner.itemAnimation <= 1)
            {

                Projectile.aiStyle = ProjAIStyleID.Arrow;

                Owner.itemAnimation = 0;
                Owner.itemTime = 0;                
            } else
            {
                Projectile.position = armPosition - Projectile.Size * 0.5f + shootDirection;
                Projectile.rotation = Projectile.velocity.ToRotation();

                Projectile.velocity = Vector2.Lerp(Projectile.velocity, Owner.DirectionTo(Main.MouseWorld), interpSpd);



                Owner.ChangeDir(Projectile.direction);

                Owner.itemRotation = (Projectile.velocity * Projectile.direction).ToRotation();
            }
            
            /*
            int oldDirection = Projectile.spriteDirection;
            if (oldDirection == -1)
                Projectile.rotation += MathHelper.Pi;

            Projectile.direction = Projectile.spriteDirection = (Projectile.velocity.X > 0).ToDirectionInt();
            // If the direction differs from what it originaly was, undo the previous 180 degree turn.
            // If this is not done, the bow will have 1 frame of rotational "jitter" when the direction changes based on the
            // original angle. This effect looks very strange in-game.
            if (Projectile.spriteDirection != oldDirection)
                Projectile.rotation -= MathHelper.Pi;
            */
        }
    }
}
