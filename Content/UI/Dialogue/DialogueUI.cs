using EthoriaMod.Content.Dialogue;
using Humanizer;
using JetBrains.Annotations;
using Microsoft.Xna.Framework;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace EthoriaMod.Content.UI.Dialogue
{
    public class DialogueUI : UIState
    {
        public DialogueBox DialogueBox { get; private set; }
        private DialoguePromptContainer PromptContainer;
        private List<DialoguePromptButton> promptButtons = new();
        private DialogueSession session;
        private float spacing = 45f;

        public override void OnInitialize()
        {
            DialogueBox = new DialogueBox();
            Append(DialogueBox);

            PromptContainer = new DialoguePromptContainer(DialogueBox);
            Append(PromptContainer);

            OnLeftClick += (_, _) => { HandleDialogueClick(); };
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (session == null) { return; }

            DialogueNode node = session.CurrentNode;

            if (DialogueBox.IsTextFinished && node.Prompts.Count > 0)
            {
                if (promptButtons.Count == 0)
                {
                    CreatePromptButtons();
                }
            }
        }

        private void HandleDialogueClick()
        {
            if (session == null) { return; }
            ;

            if (!DialogueBox.IsTextFinished)
            {
                DialogueBox.FinishText();
                return;
            }

            DialogueNode node = session.CurrentNode;

            if (node.Prompts.Count > 0)
            {
                return;
            }

            if (session.Advance())
            {
                DialogueBox.ResetWriter();
                ClearPromptButtons();
            }
        }

        private void ClearPromptButtons()
        {
            foreach (DialoguePromptButton button in promptButtons)
            {
                PromptContainer.RemoveChild(button);
            }

            promptButtons.Clear();
            PromptContainer.Height.Set(0f, 0f);
        }

        public void SetSession(DialogueSession session)
        {
            this.session = session;

            DialogueBox.SetSession(session);

            ClearPromptButtons();
        }

        public void ClearSession()
        {
            session = null;
            DialogueBox.SetSession(null);

            ClearPromptButtons();
        }

        private void CreatePromptButtons()
        {
            // Remove previous buttons
            foreach (DialoguePromptButton button in promptButtons)
            {
                PromptContainer.RemoveChild(button);
            }

            promptButtons.Clear();

            // Reset container
            PromptContainer.Width.Set(DialogueBox.BoxWidth, 0f);
            PromptContainer.Height.Set(0f, 0f);

            if (session == null)
                return;

            DialogueNode node = session.CurrentNode;
            int promptCount = node.Prompts.Count;

            if (promptCount == 0)
                return;

            DialoguePromptButton firstButton = new DialoguePromptButton(0, node.Prompts[0].Text);

            float promptHeight = firstButton.PromptHeight;

            float containerHeight = promptCount * (promptHeight + spacing);

            PromptContainer.SetSize(DialogueBox.BoxWidth, containerHeight);

            PromptContainer.Top.Set(-130f - DialogueBox.BoxHeight, 0f);

            for (int i = 0; i < promptCount; i++)
            {
                DialoguePrompt prompt = node.Prompts[i];

                DialoguePromptButton button = i == 0 ? firstButton : new DialoguePromptButton(i, prompt.Text);

                button.OnPromptSelected = HandlePromptSelected;

                button.HAlign = 1f;

                button.Top.Set(i * (button.PromptHeight + spacing), 0f);

                PromptContainer.Append(button);
                promptButtons.Add(button);
            }
        }

        private void HandlePromptSelected(int promptIndex)
        {
            if (session == null) return;

            if (session.SelectPrompt(promptIndex))
            {
                DialogueBox.ResetWriter();
                ClearPromptButtons();
            }
        }
    }
}
