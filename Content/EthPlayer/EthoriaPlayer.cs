using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EthoriaMod.Content.EthPlayer
{
    public class EthoriaPlayer : ModPlayer
    {
        public float stamina = 0.0f;
        public float maxStamina = 300.0f;
        private float minStaminaToRun = 10.0f;

        public bool sprinting = false;
        public float baseSpd = 0.0f;

        private bool lastLeftControl = false;
        private bool lastRightControl = false;
        private bool controlLeftPressed = false;
        private bool controlRightPressed = false;

        private int lTap = 0;
        private int rTap = 0;

        private int doubleTapWindow = 15;

        public override void PreUpdate()
        {

            if (!sprinting)
            {
                stamina++;
                stamina = float.Clamp(stamina, 0.0f, maxStamina);
            }

            if (!lastLeftControl) 
            {
                controlLeftPressed = Player.controlLeft;
            }
            if (!lastRightControl)
            {
                controlRightPressed = Player.controlRight;
            }
      
            lastLeftControl = Player.controlLeft;
            lastRightControl = Player.controlRight;
        }

        public override void PostUpdate()
        {
            controlLeftPressed = false;
            controlRightPressed = false;
            lTap--;
            rTap--;

        }

        public override void PreUpdateMovement()
        {

            

            float maxSpd = Player.maxRunSpeed;
            float xSpd = float.Abs(Player.velocity.X);

            if (stamina >= minStaminaToRun && rTap > 0 && rTap < doubleTapWindow && controlRightPressed)
            {
                baseSpd = maxSpd;
                stamina -= minStaminaToRun;
                if (float.Sign(Player.velocity.X) < 0 || xSpd < maxSpd)
                {
                    Player.velocity.X = maxSpd;
                }
                sprinting = true;
            } else if (stamina >= minStaminaToRun && lTap > 0 && lTap < doubleTapWindow && controlLeftPressed)
            {
                baseSpd = maxSpd;
                stamina -= minStaminaToRun;
                if (float.Sign(Player.velocity.X) > 0 || xSpd < maxSpd)
                {
                    Player.velocity.X = -maxSpd;
                }
                sprinting = true;
            }
            if (!Player.controlLeft && !Player.controlRight || stamina <= 0.0f || float.Abs(Player.velocity.X) < baseSpd)
            {
                sprinting = false;
            }

            if (controlLeftPressed)
            {
                lTap = doubleTapWindow;
            }
            if (controlRightPressed)
            {
                rTap = doubleTapWindow;
            }
        }
        public override void PostUpdateRunSpeeds()
        {
            if (sprinting)
            {
                stamina--;
                Player.maxRunSpeed *= 2;
            }

        }
    }
}
