using EthoriaMod.Content.Dialogue;
using EthoriaMod.Content.Dialogue.NPCDialogueHandlers;
using EthoriaMod.Content.UI.Dialogue;
using System;
using Terraria;
using Terraria.ModLoader;

namespace EthoriaMod.Common.Developer
{
    internal class DialogueTestCommand2 : ModCommand
    {
        public override CommandType Type => CommandType.Chat;
        public override string Command => "dt2";
        public override string Description => "Test the dialogue system";
        public override string Usage => "/dt2";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            // Load the dialogue
            TestNPC2Dialogue.Load();

            Dialogue dialogue = TestNPC2Dialogue.Dialogue;

            // Create a session
            DialogueSession session = new(dialogue);

            // Show the dialogue UI using this session
            ModContent.GetInstance<DialogueUISystem>().ShowDialogue(session);
        }
    }
}