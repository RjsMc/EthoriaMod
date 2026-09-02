using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EthoriaMod.Content.UI.SkTree;
using Microsoft.VisualStudio.Setup.Configuration;

namespace EthoriaMod.Content.UI
{
    public class UIManager : ModSystem
    {
        
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            if (!Main.dedServ)
            {

                int mouseTextIndex = layers.FindIndex(
                    layer => layer.Name.Equals("Vanilla: Mouse Text")
                );

               

                if (mouseTextIndex != -1)
                {


                    layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer("Mouse Str", delegate () { MouseStrUI.Draw(Main.spriteBatch, Main.LocalPlayer); return true; }, InterfaceScaleType.None));

                    layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer("Stamina Bar", delegate () { StaminaBarUI.Draw(Main.spriteBatch, Main.LocalPlayer); return true; }, InterfaceScaleType.None));

                    layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer("Exp Bar", delegate () { ExpBarUI.Draw(Main.spriteBatch, Main.LocalPlayer); return true; }, InterfaceScaleType.None));

                    layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer("Skill Tree", delegate () { SkillTreeUI.Draw(Main.spriteBatch, Main.LocalPlayer); return true; }, InterfaceScaleType.None));



                }
                
            }
        }
    }
}
