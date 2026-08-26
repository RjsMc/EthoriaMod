using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static System.Net.Mime.MediaTypeNames;

namespace EthoriaMod.Content.Items.Weapons.Ranged
{
    public class knockedArrow : ModProjectile
    {


        private Player Owner => Main.player[Projectile.owner];

        public float minDrawDepth = 0;
        public float maxDrawDepth = 0;
        public float minShootStrength = 0;
        public float maxShootStrength = 0;
        public int currentChargingFrames = 0;
        public int maxChargingFrames = 1000;
        public int actualType = 0;

        public int itemID = 0;
        public override string Texture => "EthoriaMod/Content/Items/Weapons/Ranged/knockedArrow";
        private float interpSpd = 0.5f;

        private Vector2 shootDirection = new Vector2(0, 0), armPosition = new Vector2(0, 0), tipPosition = new Vector2(0, 0);
        public override void SetDefaults()
        {
            Projectile.width = 7;
            Projectile.height = 25;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.ignoreWater = true;
            DrawHeldProjInFrontOfHeldItemAndArms = false;

        }
 
        public override bool PreDraw(ref Color lightColor)
        {

            if (Projectile.aiStyle == ProjAIStyleID.Arrow)
            {
                return true;
            }

            Owner.heldProj = Projectile.whoAmI;
            Texture2D newTexture = TextureAssets.Item[itemID].Value;
        

            Main.EntitySpriteDraw(newTexture, tipPosition - Main.screenPosition, null, Color.White, Projectile.rotation, newTexture.Size() - new Vector2(newTexture.Width * 0.5f , 0), 1f, 0, 0);

            
            return false;
        }

        // public override bool? CanDamage() => false;
    
 
        public override void AI()
        {
            currentChargingFrames++;
            
            if (Projectile.aiStyle == ProjAIStyleID.Arrow)
            {
                return;
            }

            float chargedPercent = (float)currentChargingFrames / (float)maxChargingFrames;
            float mag = minShootStrength + (maxShootStrength - minShootStrength) * chargedPercent;

            shootDirection = Projectile.velocity.SafeNormalize(Vector2.UnitX * Owner.direction);
            armPosition = Owner.RotatedRelativePoint(Owner.MountedCenter, true);
            tipPosition = armPosition + shootDirection * (maxDrawDepth - chargedPercent * (maxDrawDepth - minDrawDepth));

            if (!Main.mouseLeft || Owner.itemAnimation <= 1)
            {

                Projectile.aiStyle = ProjAIStyleID.Arrow;

                
                Projectile.velocity = shootDirection * mag;
                Owner.itemAnimation = 0;
                Owner.itemTime = 0;

                Projectile.Kill();
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, Projectile.velocity, actualType, Projectile.damage, Projectile.knockBack, Owner.whoAmI);
            } else
            {

                // The arrow is rotated around the bottom middle
                // The bow is rotated around HoldoutOffset()

                Owner.itemRotation = (Projectile.velocity * Projectile.direction).ToRotation();

                Texture2D arrowTex = ModContent.Request<Texture2D>("EthoriaMod/Content/Items/Weapons/Ranged/knockedArrow").Value;

                Vector2 texOffSet = arrowTex.Size();
                texOffSet.Y *= 2;
                
                Projectile.position = tipPosition - texOffSet * 0.5f;

                Projectile.rotation = Projectile.velocity.ToRotation() - float.Pi / 2;

                Projectile.velocity = Vector2.Lerp(Projectile.velocity, Owner.DirectionTo(Main.MouseWorld), interpSpd);

                Owner.ChangeDir(Projectile.direction);

            }
            

        }
    }
}
