using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EthoriaMod.Content.EthoriaPlayer
{
    public class EthoriaPlayer : ModPlayer
    {
        public float stamina = 0.0f;
        public bool sprinting = false;

        private bool lastLeftControl = false;
        private bool lastRightControl = false;
        private bool controlLeftPressed = false;
        private bool controlRightPressed = false;

        private int lTap = 0;
        private int rTap = 0;

        private int doubleTapWindow = 15;

        public override void PreUpdate()
        {
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

            if (rTap > 0 && rTap < doubleTapWindow && controlRightPressed)
            {
                if (float.Sign(Player.velocity.X) < 0 || xSpd < maxSpd)
                {
                    Player.velocity.X = maxSpd;
                }
                sprinting = true;
            } else if (lTap > 0 && lTap < doubleTapWindow && controlLeftPressed)
            {
                if (float.Sign(Player.velocity.X) > 0 || xSpd < maxSpd)
                {
                    Player.velocity.X = -maxSpd;
                }
                sprinting = true;
            }

            if (!Player.controlLeft && !Player.controlRight)
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
                Player.maxRunSpeed *= 2;
            }

        }
    }
}
