using EthoriaMod.Content.Dialogue;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace EthoriaMod.Content.UI.Dialogue
{
    public class DialogueUISystem : ModSystem
    {
        internal UserInterface DialogueInterface;
        internal DialogueUI DialogueUI;

        public override void Load()
        {
            if (!Main.dedServ)
            {
                DialogueInterface = new UserInterface();
                DialogueUI = new DialogueUI();
                DialogueUI.Activate();
            }
        }

        public override void UpdateUI(GameTime gameTime)
        {
            DialogueInterface?.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(
            List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(
                layer => layer.Name.Equals("Vanilla: Mouse Text")
            );

            if (mouseTextIndex != -1)
            {
                layers.Insert(
                    mouseTextIndex,
                    new LegacyGameInterfaceLayer(
                        "EthoriaMod: Dialogue",
                        delegate
                        {
                            DialogueInterface.Draw(Main.spriteBatch, new GameTime());

                            return true;
                        },
                        InterfaceScaleType.UI
                    )
                );
            }
        }

        public void ShowDialogue(DialogueSession session)
        {
            DialogueUI.SetSession( session );
            DialogueInterface?.SetState(DialogueUI);
        }

        public void HideDialogue()
        {
            DialogueInterface?.SetState(null);
        }
    }
}
