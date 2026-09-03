using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using EthoriaMod.Common.Helpers;
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
        public int numArrows = 1;
        public float arrowSpread = float.Pi/16;
        public override string Texture => "EthoriaMod/Content/Items/Weapons/Ranged/KnockedArrow";
        private float interpSpd = 0.5f;

        public int arrowDist = 5;

        public int hiddenFrames = 0;

        private Vector2 shootDirection = new Vector2(0, 0), armPosition = new Vector2(0, 0), tipPosition = new Vector2(0, 0), featherPosition = new Vector2(0, 0);
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
            if (hiddenFrames > 0)
            {

                hiddenFrames--;
                return false;
            }

            Vector2 worldPixelPos = featherPosition;

            int tileX = (int)(worldPixelPos.X / 16f);
            int tileY = (int)(worldPixelPos.Y / 16f);

            Color color = Lighting.GetColor(tileX, tileY);

            Owner.heldProj = Projectile.whoAmI;
            Texture2D newTexture = TextureAssets.Projectile[actualType].Value;

            float rot = Projectile.rotation + float.Pi;

            Vector2 perp = new Vector2((float)Math.Cos(rot), (float)Math.Sin(rot));



            int fullDist = (numArrows - 1) * arrowDist;
            Vector2 drawPos = featherPosition - perp * (fullDist / 2);
            float rotStep = 0;
            if (numArrows > 1) {

                rotStep = arrowSpread / (numArrows - 1);
                rot -= arrowSpread / 2;
            }
            for (int i = 0; i < numArrows; i++)
            {

                Main.EntitySpriteDraw(newTexture, drawPos - Main.screenPosition, null, color, rot, new Vector2(newTexture.Width * 0.5f, newTexture.Height), 1f, 0, 0);
                drawPos += perp * arrowDist;
                rot += rotStep;
            }

            
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

            featherPosition = armPosition + shootDirection * (maxDrawDepth - chargedPercent * (maxDrawDepth - minDrawDepth));

            Texture2D arrowTex = TextureAssets.Projectile[actualType].Value;

            tipPosition = featherPosition + shootDirection * float.Max(arrowTex.Size().X, arrowTex.Size().Y);


            if (!Main.mouseLeft || (Owner.itemAnimation <= 1 && autoReuse))
            {
                Projectile.aiStyle = ProjAIStyleID.Arrow;
               
                Projectile.velocity = shootDirection * mag;
                Projectile.rotation = shootDirection.ToRotation();

                Owner.itemAnimation = 1;
                Owner.itemTime = 1;
                if (currentChargingFrames >= minChargingFrames) {

                    HelperFunctions.consumeAmmoForced(Owner, Owner.HeldItem, 1);


                    float rot = Projectile.rotation + float.Pi;

                    Vector2 perp = new Vector2((float)Math.Cos(rot), (float)Math.Sin(rot));
                    int fullDist = (numArrows - 1) * arrowDist;
                    Vector2 pos = tipPosition - perp * (fullDist / 2);

                    float rotStep = 0;
                    rot = 0;
                    if (numArrows > 1)
                    {
                        rotStep = arrowSpread / (numArrows - 1);
                        rot -= arrowSpread / 2;
                    }
                    
                    for (int i = 0; i < numArrows; i++)
                    {

                        Vector2 newVel = Projectile.velocity.RotatedBy(rot);


                        int arrowIdx = Projectile.NewProjectile(Projectile.GetSource_FromThis(), pos, newVel, actualType, Projectile.damage, Projectile.knockBack, Owner.whoAmI);
                       

                        Projectile actualArrow = Main.projectile[arrowIdx];
                        actualArrow.usesLocalNPCImmunity = true;
                        actualArrow.localNPCHitCooldown = -1; 
                        actualArrow.rotation = Projectile.velocity.ToRotation() + float.Pi / 2;

                        pos += perp * arrowDist;
                        rot += rotStep;
                    }

                }

                Projectile.Kill();
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
