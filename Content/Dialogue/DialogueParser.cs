using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace EthoriaMod.Content.Dialogue
{
    public class DialogueParser
    {
        public static Dialogue Parse(string input)
        {
            Dialogue dialogue = new();

            MatchCollection matches = Regex.Matches(
                input,
                @"<node>(.*?)</node>",
                RegexOptions.Singleline
             );

            /*
                <node>
                Hello = {
                    Speaker: "Test",
                    Text: "YOOOOOO HOW IT GOIIIIING WHAT'S GOOOOOOOOOOOD?!?!?!?!?!",
                    Prompts = [
                        { Text = "uhh..", NextNode = "HelloReply1"},
                        { Text = "YOOOOOOOO", NextNode = "HelloReply2"}
                    ]
                }
                </node>

                <node>
                HelloReply1 = {
                    Speaker: "Test",
                    Text: "i hate you stfu",
                    Prompts = [
                        { Text = "oh wow ok", NextNode = "End"},
                        { Text = ":(", NextNode = "End"}
                    ]
                }</node>
             */

            foreach (Match match in matches)
            {
                string nodeText = match.Groups[1].Value; // Give me the first thing that I specifically captured. Groups[1] so it drops <node>

                /*
                This for loop basically finds 2 matches for each <node>

                nodeText on first run returns 
                Hello = {
                    Speaker: "Test",
                    Text: "YOOOOOO"
                }
                 */

                Match idMatch = Regex.Match( // Matches Node = {, but idMatch.Groups[1].Value is Hello. Groups[0] would capture everything.
                    nodeText,
                    @"^\s*([A-Za-z0-9_]+)\s*=\s*\{"
                );

                if (!idMatch.Success) continue;

                Match speakerMatch = Regex.Match(
                    nodeText,
                    @"Speaker:\s*""(.*?)"""
                );

                Match textMatch = Regex.Match(
                    nodeText,
                    @"Text:\s*""(.*?)"""
                );

                Match promptsMatch = Regex.Match(
                    nodeText,
                    @"Prompts\s*=\s*\[(.*?)\]",
                    RegexOptions.Singleline
                );

                MatchCollection promptMatches = Regex.Matches(
                    promptsMatch.Groups[1].Value,
                    @"\{\s*Text\s*=\s*""(.*?)""\s*,\s*NextNode\s*=\s*""(.*?)""\s*\}" // REGEX captures 2 groups. So Group[1] = uhh.. Groups[2] = HelloReply1
                );

                Match nextNodeMatch = Regex.Match(
                    nodeText,
                    @"NextNode\s*=\s*""(.*?)"""
                );

                Match actionMatch = Regex.Match(
                    nodeText,
                    @"Action\s*=\s*""(.*?)"""
                );

                string speaker = speakerMatch.Success ? speakerMatch.Groups[1].Value : "REPORT ME I BROKE (speaker)";
                string text = textMatch.Success ? textMatch.Groups[1].Value : "REPORT ME I BROKE (text)";
                string id = idMatch.Groups[1].Value;

                DialogueNode node = new()
                {
                    Id = id,
                    Speaker = speaker,
                    Text = text,
                    NextNode = nextNodeMatch.Success ? nextNodeMatch.Groups[1].Value : null,
                    Action = actionMatch.Success ? actionMatch.Groups[1].Value : null
                };


                foreach (Match promptMatch in promptMatches)
                {
                    DialoguePrompt prompt = new()
                    {
                        Text = promptMatch.Groups[1].Value,
                        NextNode = promptMatch.Groups[2].Value
                    };

                    node.Prompts.Add(prompt);
                }

                dialogue.Nodes.Add(node.Id, node);
            }

            return dialogue;
        }
    }
}
