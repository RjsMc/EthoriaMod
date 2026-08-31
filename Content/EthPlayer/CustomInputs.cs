using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steamworks;
using Terraria;
using Terraria.ModLoader;

namespace EthoriaMod.Content.EthPlayer
{
    public class CustomInputs
    {
        public Player owner;

        public bool lastLeftControl = false;
        public bool lastRightControl = false;
        public bool lastLeftClick = false;

        public bool controlLeftPressed = false;
        public bool controlRightPressed = false;
        public bool leftClickPressed = false;

        public CustomInputs(Player owner)
        {
            this.owner = owner;
        }
        public void PreUpdate()
        {
            if (!lastLeftControl)
            {
                controlLeftPressed = owner.controlLeft;
            }
            if (!lastRightControl)
            {
                controlRightPressed = owner.controlRight;
            }
            if (!lastLeftClick)
            {
                leftClickPressed = Main.mouseLeft;
            }
            lastLeftControl = owner.controlLeft;
            lastRightControl = owner.controlRight;
            lastLeftClick = Main.mouseLeft;
        }
        
        public void PostUpdate()
        {
            controlLeftPressed = false;
            controlRightPressed = false;
            leftClickPressed = false;
        }
    }
}
