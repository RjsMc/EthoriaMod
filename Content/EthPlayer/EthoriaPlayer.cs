using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Threading.Tasks;
using EthoriaMod.Content.UI.SkTree;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace EthoriaMod.Content.EthPlayer
{
    public class EthoriaPlayer : ModPlayer
    {

        public int level = 1;
        public int currentExp = 0;
        public int expToSyphon = 0;

        public float stamina = 0.0f;
        public float maxStamina = 300.0f;
        private float minStaminaToRun = 10.0f;

        public bool sprinting = false;
        public float baseSpd = 0.0f;

        public CustomInputs customInputs;

        private int lTap = 0;
        private int rTap = 0;

        private int doubleTapWindow = 15;


        public SkillTree skillTree = new SkillTree();

        public override void SaveData(TagCompound tag)
        {
            tag["level"] = level;
            tag["currentExp"] = currentExp;
            tag["expToSyphon"] = expToSyphon;
            tag["skillTree"] = skillTree;
        }

        public override void LoadData(TagCompound tag)
        {
            level = tag.GetInt("level");
            currentExp = tag.GetInt("currentExp");
            expToSyphon = tag.GetInt("expToSyphon");
            skillTree = tag.Get<SkillTree>("skillTree");
            
            customInputs = new CustomInputs(Player);
            this.syphonAllExp();
        }
        
        public override void PreUpdate()
        {

            if (!sprinting)
            {
                stamina++;
                stamina = float.Clamp(stamina, 0.0f, maxStamina);
            }

            customInputs.PreUpdate();
            
            syphonExp();
        }

        public override void PostUpdate()
        {
            lTap--;
            rTap--;

        }
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
           
            if (PlayerInput.Triggers.JustPressed.MouseLeft)
            {
                // This logic will run exactly once per click
                Main.NewText("Left mouse button was clicked once!");
            }
        }
        public override void PreUpdateMovement()
        {

            

            float maxSpd = Player.maxRunSpeed;
            float xSpd = float.Abs(Player.velocity.X);

            if (stamina >= minStaminaToRun && rTap > 0 && rTap < doubleTapWindow && customInputs.controlRightPressed)
            {
                baseSpd = maxSpd;
                stamina -= minStaminaToRun;
                if (float.Sign(Player.velocity.X) < 0 || xSpd < maxSpd)
                {
                    Player.velocity.X = maxSpd;
                }
                sprinting = true;
            } else if (stamina >= minStaminaToRun && lTap > 0 && lTap < doubleTapWindow && customInputs.controlLeftPressed)
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

            if (customInputs.controlLeftPressed)
            {
                lTap = doubleTapWindow;
            }
            if (customInputs.controlRightPressed)
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

        public int expToLevelUp(int level = -1)
        {
            if (level == -1) 
            {
                level = this.level;
            }

            return (2 * level - 1) * 30;
        }

        private void syphonAllExp()
        {

            currentExp += expToSyphon;
            expToSyphon = 0;

            while (currentExp >= this.expToLevelUp())
            {
                int remainder = currentExp - this.expToLevelUp();
                level++;
                currentExp = remainder;
                Main.NewText("Level Up: " + level.ToString());
            }
        }

        private void syphonExp()
        {
            if (expToSyphon > 0)
            {
                int syphonStep = int.Min(expToSyphon, this.expToLevelUp() / 60);
                syphonStep = int.Max(syphonStep, 1);
                expToSyphon -= syphonStep;
                currentExp += syphonStep;
            }
            while (currentExp >= this.expToLevelUp())
            {
                int remainder = currentExp - this.expToLevelUp();
                level++;
                currentExp = remainder;
                Main.NewText("Level Up: " + level.ToString());
            }
        }

        public void gainExp(int ammount)
        {
            this.expToSyphon += ammount;
        }
    }
}
