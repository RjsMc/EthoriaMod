using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EthoriaMod.Content.Dialogue.NPCDialogueHandlers
{
    public static class DialogueTest
    {
        public static Dialogue CreateTestDialogue()
        {
            Dialogue dialogue = new Dialogue();

            dialogue.StartNode = "Hello";

            DialogueNode hello = new DialogueNode
            {
                Id = "Hello",
                Speaker = "Test",
                Text = "YOOOOOO HOW IT GOIIIIING?!?!"
            };

            hello.Prompts.Add(new DialoguePrompt
            {
                Text = "uhh..",
                NextNode = "HelloReply1"
            });

            hello.Prompts.Add(new DialoguePrompt
            {
                Text = "YOOOOOOOO",
                NextNode = "HelloReply2"
            });

            DialogueNode helloReply1 = new DialogueNode
            {
                Id = "HelloReply1",
                Speaker = "Test",
                Text = "i hate you stfu"
            };

            DialogueNode helloReply2 = new DialogueNode
            {
                Id = "HelloReply2",
                Speaker = "Test",
                Text = "HAVE A GOOD ONE MAAAAAN"
            };

            dialogue.Nodes.Add(hello.Id, hello);
            dialogue.Nodes.Add(helloReply1.Id, helloReply1);
            dialogue.Nodes.Add(helloReply2.Id, helloReply2);

            return dialogue;
        }
    }
}
