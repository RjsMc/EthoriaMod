using EthoriaMod.Content.Dialogue.NPCDialogueHandlers;
using EthoriaMod.Content.UI.Dialogue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

#nullable enable
namespace EthoriaMod.Content.Dialogue
{
    public class DialogueManager
    {
        public static void Load()
        {
            TestNPCDialogue.Load();
        }

        public static void EndDialogue()
        {
            ModContent.GetInstance<DialogueUISystem>().HideDialogue();
        }
    }
}
