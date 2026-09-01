using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static System.Net.Mime.MediaTypeNames;

namespace EthoriaMod.Content.Items.Weapons.Ranged
{
    public class KnockedArrow : ModProjectile
    {


        private Player Owner => Main.player[Projectile.owner];

        public float minDrawDepth = 0;
        public float maxDrawDepth = 0;
        public float minShootStrength = 0;
        public float maxShootStrength = 0;
        public int currentChargingFrames = 0;
        public int maxChargingFrames = 1000;
        public int actualType = 0;
        public int bowId = 0;
        public int minChargingFrames = 0;
        public bool autoReuse = false;
        public int itemID = 0;
        public override string Texture => "EthoriaMod/Content/Items/Weapons/Ranged/KnockedArrow";
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
            Vector2 worldPixelPos = tipPosition; // Example position

            // 2. Convert pixel position to tile coordinates
            int tileX = (int)(worldPixelPos.X / 16f);
            int tileY = (int)(worldPixelPos.Y / 16f);

            // 3. Get the lighting color at that tile
            Color color = Lighting.GetColor(tileX, tileY);
            
            Owner.heldProj = Projectile.whoAmI;
            Texture2D newTexture = TextureAssets.Projectile[actualType].Value;
            

            Main.EntitySpriteDraw(newTexture, tipPosition - Main.screenPosition, null, color, Projectile.rotation + float.Pi, new Vector2(newTexture.Width * 0.5f , 0), 1f, 0, 0);

            
            return false;
        }

        public override bool? CanDamage() => false;
    
 
        public override void AI()
        {
            currentChargingFrames++;
            currentChargingFrames = int.Min(maxChargingFrames, currentChargingFrames);
            
            float chargedPercent = float.Min(1, (float)currentChargingFrames / (float)maxChargingFrames);
            float mag = minShootStrength + (maxShootStrength - minShootStrength) * chargedPercent;

            shootDirection = Projectile.velocity.SafeNormalize(Vector2.UnitX * Owner.direction);
            armPosition = Owner.RotatedRelativePoint(Owner.MountedCenter, true);
            tipPosition = armPosition + shootDirection * (maxDrawDepth - chargedPercent * (maxDrawDepth - minDrawDepth));
            
            if (!Main.mouseLeft || (Owner.itemAnimation <= 1 && autoReuse))
            {

                Projectile.aiStyle = ProjAIStyleID.Arrow;

                
                Projectile.velocity = shootDirection * mag;
                Owner.itemAnimation = 1;
                Owner.itemTime = 1;

                Projectile.Kill();

                if (currentChargingFrames >= minChargingFrames) {
                    int arrowIdx = Projectile.NewProjectile(Projectile.GetSource_FromThis(), tipPosition, Projectile.velocity, actualType, Projectile.damage, Projectile.knockBack, Owner.whoAmI);
                    Projectile actualArrow = Main.projectile[arrowIdx];
                    actualArrow.rotation = Projectile.velocity.ToRotation() + float.Pi / 2;
                }
            } else
            {
                if (Owner.itemAnimation <= 1)
                {
                    Owner.itemAnimation = 2;
                    Owner.itemTime = 2;
                }

                Projectile.rotation = Projectile.velocity.ToRotation() - float.Pi / 2;

                float normalizedAngle = Projectile.rotation - (float.Floor(Projectile.rotation / (2 * float.Pi)) * (2 * float.Pi));
                Projectile.direction = 1;
                if (normalizedAngle > 0 && normalizedAngle < float.Pi)
                {
                    Projectile.direction = -1;
                }

                Projectile.velocity = Vector2.Lerp(Projectile.velocity, Owner.DirectionTo(Main.MouseWorld), interpSpd);



                Owner.ChangeDir(Projectile.direction);

                Owner.itemRotation = (Projectile.velocity * Projectile.direction).ToRotation();



                Owner.heldProj = Projectile.whoAmI;


                float angle = Projectile.rotation + Projectile.direction * (chargedPercent * float.Pi / 2);

                Owner.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, Projectile.velocity.ToRotation() - (float.Pi / 2));
                List<Player.CompositeArmStretchAmount> armStretches = [ Player.CompositeArmStretchAmount.Full, Player.CompositeArmStretchAmount.ThreeQuarters, Player.CompositeArmStretchAmount.Quarter, Player.CompositeArmStretchAmount.None, Player.CompositeArmStretchAmount.None];
                int armIdx = (int) (((float) (armStretches.Count - 1)) * chargedPercent);
                Owner.SetCompositeArmFront(true, armStretches[armIdx], Projectile.velocity.ToRotation() - (float.Pi / 2));
               
            }



        }
    }
}
