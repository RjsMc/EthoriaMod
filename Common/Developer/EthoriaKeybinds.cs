using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;

namespace EthoriaMod.Common.Developer
{
    public class EthoriaKeybinds : ModSystem
    {
        public static ModKeybind skillTreeHotKey { get; private set; }
  
        public override void Load()
        {
           
            skillTreeHotKey = KeybindLoader.RegisterKeybind(Mod, "OpenSkillTree", "J");
      
        }

        public override void Unload()
        {
            // Clean up the static reference when the mod unloads
            skillTreeHotKey = null;
        }
    }
}
