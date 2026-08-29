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

namespace EthoriaMod.Content.UI
{
    public class UIManager : ModSystem
    {

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
           
            
            layers.Insert(0, new LegacyGameInterfaceLayer("Test", delegate() { StaminaBarUI.Draw(Main.spriteBatch, Main.LocalPlayer); return true; }, InterfaceScaleType.None));

            layers.Insert(0, new LegacyGameInterfaceLayer("Test", delegate () { ExpBarUI.Draw(Main.spriteBatch, Main.LocalPlayer); return true; }, InterfaceScaleType.None));

            layers.Insert(0, new LegacyGameInterfaceLayer("Test", delegate () { SkillTreeUI.Draw(Main.spriteBatch, Main.LocalPlayer); return true; }, InterfaceScaleType.None));

        }
    }
}
