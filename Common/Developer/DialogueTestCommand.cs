using EthoriaMod.Content.Dialogue;
using EthoriaMod.Content.Dialogue.NPCDialogueHandlers;
using EthoriaMod.Content.UI.Dialogue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace EthoriaMod.Common.Developer
{
    internal class DialogueTestCommand : ModCommand // Command to test starting node of dialogue
    {
        public override CommandType Type => CommandType.Chat;
        public override string Command => "dialoguetest";
        public override string Description => "Test the starting node for dialogue";
        public override string Usage => "/dialoguetest";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            TestNPCDialogue.Load();

            Dialogue dialogue = TestNPCDialogue.Dialogue;

            caller.Reply($"Start Node: {dialogue.StartNode}");

            if (dialogue.StartNode == null)
            {
                caller.Reply("StartNode is null!");
                return;
            }

            if (!dialogue.Nodes.TryGetValue(dialogue.StartNode, out DialogueNode node))
            {
                caller.Reply($"Could not find node '{dialogue.StartNode}'!");
                return;
            }

            caller.Reply($"Speaker: {node.Speaker}");
            caller.Reply($"Text: {node.Text}");
            caller.Reply($"Prompts: {node.Prompts.Count}");

            for (int i = 0; i < node.Prompts.Count; i++)
            {
                DialoguePrompt prompt = node.Prompts[i];

                caller.Reply(
                    $"{i}: {prompt.Text} -> {prompt.NextNode}"
                );
            }

            ModContent.GetInstance<DialogueUISystem>().ShowDialogue();
        }
    }
}
