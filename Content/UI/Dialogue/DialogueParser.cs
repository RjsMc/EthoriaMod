using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Color = Microsoft.Xna.Framework.Color;

namespace EthoriaMod.Content.UI.Dialogue
{
    public static class DialogueTextParser
    {
        public static List<TextSnippet> Parse(string text)
        {
            List<TextSnippet> snippets = new();

            TextSettings currentSettings = new();

            Stack<float> delayStack = new();
            Stack<bool> boldStack = new();
            Stack<bool> italicizedStack = new();
            Stack<Color> colorStack = new();
            Stack<float> sizeStack = new();

            delayStack.Push(currentSettings.Delay);
            boldStack.Push(currentSettings.Bold);
            italicizedStack.Push(currentSettings.Italicized);
            colorStack.Push(currentSettings.Color);
            sizeStack.Push(currentSettings.Size);

            int textStart = 0;

            while (textStart < text.Length)
            {
                int tagStart = text.IndexOf('<', textStart);

                // No more tags, so the rest is normal text
                if (tagStart == -1)
                {
                    AddSnippet(
                        snippets,
                        text.Substring(textStart),
                        currentSettings
                    );

                    break;
                }

                // Add the text before the tag
                if (tagStart > textStart)
                {
                    AddSnippet(
                        snippets,
                        text.Substring(textStart, tagStart - textStart),
                        currentSettings
                    );
                }

                int tagEnd = text.IndexOf('>', tagStart);

                if (tagEnd == -1)
                {
                    throw new Exception("Dialogue text contains an unclosed tag.");
                }

                string tag = text.Substring(
                    tagStart + 1,
                    tagEnd - tagStart - 1
                );

                ParseTag(
                    tag,
                    currentSettings,
                    delayStack
                );

                textStart = tagEnd + 1;
            }

            return snippets;
        }

        private static void ParseTag(
            string tag,
            TextSettings currentSettings,
            Stack<float> delayStack)
        {
            // Opening delay tag: <d:2>
            if (tag.StartsWith("d:"))
            {
                string value = tag.Substring(2);

                if (!float.TryParse(value, out float delay))
                {
                    throw new Exception($"Invalid dialogue delay: {value}");
                }

                delayStack.Push(delay);
                currentSettings.Delay = delay;

                return;
            }

            // Closing delay tag: </d>
            if (tag == "/d")
            {
                if (delayStack.Count <= 1)
                {
                    throw new Exception("Dialogue contains </d> without a matching <d:...>.");
                }

                delayStack.Pop();
                currentSettings.Delay = delayStack.Peek();

                return;
            }

            throw new Exception($"Unknown dialogue tag: <{tag}>");
        }

        private static void AddSnippet(
            List<TextSnippet> snippets,
            string text,
            TextSettings settings)
        {
            if (string.IsNullOrEmpty(text))
                return;

            snippets.Add(
                new TextSnippet(
                    text,
                    settings.Clone()
                )
            );
        }
    }
}