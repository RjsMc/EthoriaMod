using Ionic.Zlib;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

#nullable enable
namespace EthoriaMod.Content.Dialogue
{
    public class DialogueLoader
    {
        public static Dialogue Load(string category)
        {
            Dialogue dialogue = new();

            string startNodeKey = $"{category}.StartNode";
            dialogue.StartNode = Language.GetTextValue(startNodeKey);

            HashSet<string> nodeIds = new();

            string nodePattern =
                $"^{Regex.Escape(category)}\\.Nodes\\.([A-Za-z0-9_]+)\\.";

            LocalizedText[] entries =
                LanguageManager.Instance.FindAll(new Regex(nodePattern)); // Searches all localization entires and returns the ones whose keys mach the pattern

            foreach (LocalizedText entry in entries)
            {
                Match match = Regex.Match(entry.Key, nodePattern);

                if (match.Success)
                {
                    nodeIds.Add(match.Groups[1].Value);
                }
            }

            foreach (string nodeId in nodeIds)
            {
                string speakerKey = $"{category}.Nodes.{nodeId}.Speaker";
                string textKey = $"{category}.Nodes.{nodeId}.Text";
                string nextNodeKey = $"{category}.Nodes.{nodeId}.NextNode";
                string actionKey = $"{category}.Nodes.{nodeId}.Action";
                string? speaker = Language.Exists(speakerKey) ? Language.GetTextValue(speakerKey) : null;
                string? text = Language.Exists(textKey) ? Language.GetTextValue(textKey) : null;
                string? nextNode = Language.Exists(nextNodeKey) ? Language.GetTextValue(nextNodeKey) : null;
                string? action  = Language.Exists(actionKey) ? Language.GetTextValue(actionKey) : null;
                DialogueNode node = new()
                {
                    Id = nodeId,
                    Speaker = speaker,
                    Text = text,
                    NextNode = nextNode,
                    Action = action,
                };

                // Load prompts if any
                for (int i = 0; ; i++) // Im not putting a condition because we'll break anyway
                {
                    string promptTextKey = $"{category}.Nodes.{nodeId}.Prompts.{i}.Text";
                    string promptNextNodeKey = $"{category}.Nodes.{nodeId}.Prompts.{i}.NextNode";

                    if (!Language.Exists(promptTextKey)) break; // Assume no prompts or out of prompts

                    string promptText = Language.GetTextValue(promptTextKey);
                    string promptNextNode = Language.GetTextValue(promptNextNodeKey);

                    DialoguePrompt prompt = new()
                    {
                        Text = promptText,
                        NextNode = promptNextNode
                    };

                    node.Prompts.Add(prompt);
                }

                dialogue.Nodes.Add(nodeId, node);
            }

            // Validation
            if (dialogue.StartNode == null)
            {
                throw new Exception($"Dialogue '{category}' does not have a StartNode.");
            }

            if (!dialogue.Nodes.ContainsKey(dialogue.StartNode))
            {
                throw new Exception($"Dialogue '{category}' has invalid StartNode '{dialogue.StartNode}'.");
            }

            foreach (DialogueNode node in dialogue.Nodes.Values)
            {
                if (node.NextNode != null && !dialogue.Nodes.ContainsKey(node.NextNode))
                {
                    throw new Exception($"Dialogue '{category}' node '{node.Id}' points to non-existent node '{node.NextNode}'.");
                }

                foreach (DialoguePrompt prompt in node.Prompts)
                {
                    if (prompt.NextNode != null && !dialogue.Nodes.ContainsKey(prompt.NextNode))
                    {
                        throw new Exception($"Dialogue '{category}' node '{node.Id}' has a prompt that points to non-existent node '{prompt.NextNode}'.");
                    }
                }
            }

            return dialogue;
        }
    }
}
