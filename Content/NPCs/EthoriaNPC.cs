using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EthoriaMod.Content.EthPlayer;
using Terraria;
using Terraria.ModLoader;

namespace EthoriaMod.Content.NPCs
{
    public class EthoriaNPC : GlobalNPC
    {
        public override void OnKill(NPC npc)
        {
            int playerIndex = npc.lastInteraction;
            // Verify the killer is a valid active player
            if (playerIndex != 255)
            {
                Player thePlayer = Main.player[playerIndex];
                thePlayer.GetModPlayer<EthoriaPlayer>().gainExp(1000);
            }
        }
    }
}
