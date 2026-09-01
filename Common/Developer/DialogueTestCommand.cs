using EthoriaMod.Content.Dialogue;
using EthoriaMod.Content.Dialogue.NPCDialogueHandlers;
using System;
using Terraria;
using Terraria.ModLoader;

namespace EthoriaMod.Common.Developer
{
    internal class DialogueTestCommand : ModCommand
    {
        public override CommandType Type => CommandType.Chat;
        public override string Command => "dialoguetest";
        public override string Description => "Test the dialogue system";
        public override string Usage => "/dialoguetest";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            // Load the dialogue
            TestNPCDialogue.Load();

            Dialogue dialogue = TestNPCDialogue.Dialogue;

            caller.Reply("=== Dialogue Test ===");

            // Create a session
            DialogueSession session = new(dialogue);

            // Show starting node
            PrintCurrentNode(caller, session);

            // Select prompt 0
            caller.Reply("");
            caller.Reply("Selecting prompt 0...");

            if (session.SelectPrompt(0))
            {
                PrintCurrentNode(caller, session);
            }
            else
            {
                caller.Reply("Failed to select prompt 0.");
            }

            // Select prompt 0 again
            caller.Reply("");
            caller.Reply("Selecting prompt 0 again...");

            if (session.SelectPrompt(0))
            {
                PrintCurrentNode(caller, session);
            }
            else
            {
                caller.Reply("Failed to select prompt 0.");
            }

            caller.Reply("");
            caller.Reply("=== Test Complete ===");
        }

        private static void PrintCurrentNode(
            CommandCaller caller,
            DialogueSession session)
        {
            DialogueNode node = session.CurrentNode;

            caller.Reply($"Node: {node.Id}");
            caller.Reply($"Speaker: {node.Speaker ?? "(none)"}");
            caller.Reply($"Text: {node.Text ?? "(none)"}");
            caller.Reply($"NextNode: {node.NextNode ?? "(none)"}");
            caller.Reply($"Action: {node.Action ?? "(none)"}");
            caller.Reply($"Prompts: {node.Prompts.Count}");

            for (int i = 0; i < node.Prompts.Count; i++)
            {
                DialoguePrompt prompt = node.Prompts[i];

                caller.Reply(
                    $"  [{i}] {prompt.Text} -> {prompt.NextNode}"
                );
            }
        }
    }
}

