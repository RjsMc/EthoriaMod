using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EthoriaMod.Content.NPCs.TestEnemy
{
    internal class TestEnemy : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 6;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Velocity = 1f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.Zombie);
            NPC.width = 38;
            NPC.height = 38;
            NPC.damage = 1;
            NPC.defense = 100;
            NPC.lifeMax = 2;
            NPC.scale = 1f;
            NPC.HitSound = SoundID.NPCHit13;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 99999f;
            NPC.knockBackResist = 1;
            NPC.aiStyle = NPCAIStyleID.Fighter;

            AIType = NPCID.Zombie;
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter += 0.15f;
            NPC.frameCounter %= Main.npcFrameCount[NPC.type];
            int frameIndex = (int)NPC.frameCounter;

            NPC.frame.Height = 38;

            NPC.frame.Y = frameIndex * (38 + 2);
        }
    }
}
