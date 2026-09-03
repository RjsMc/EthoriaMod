using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EthoriaMod.Content.Dialogue.NPCDialogueHandlers
{
    public static class TestNPC2Dialogue
    {
        public static Dialogue Dialogue { get; private set; } = null!;

        public static void Load()
        {
            Dialogue = DialogueLoader.Load("Mods.EthoriaMod.TestNPC2");
        }
    }
}
