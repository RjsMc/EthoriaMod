using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace EthoriaMod.Content.EthoriaPlayer
{
    public class EthoriaPlayer : ModPlayer
    {
        public float stamina = 0.0f;


        public override void PreUpdateMovement()
        {
            Player.velocity = new Vector2(0, 0);
        }
    }
}
