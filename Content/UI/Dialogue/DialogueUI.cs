using JetBrains.Annotations;
using Microsoft.Xna.Framework;
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
    public class DialogueUI : ModSystem
    {
        private GameTime _lastUpdateUiGameTime;
        internal UserInterface ExampleInterface;

        public class TheUI : UIState
        {
            public override void OnInitialize()
            {
                UIPanel panel = new UIPanel();
                panel.Width.Set(300, 0);
                panel.Height.Set(300, 0);
                Append(panel);

                UIText text = new UIText("Hello world!"); // 1
                panel.Append(text);                       // 2
            }
        }

        internal TheUI MyUI;

        public override void UpdateUI(GameTime gameTime)
        {
            _lastUpdateUiGameTime = gameTime;
            if (ExampleInterface?.CurrentState != null)
            {
                ExampleInterface.Update(gameTime);
            }
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "MyMod: My Interface",
                    delegate
                    {
                        if (_lastUpdateUiGameTime != null && ExampleInterface?.CurrentState != null)
                        {
                            ExampleInterface.Draw(Main.spriteBatch, _lastUpdateUiGameTime);
                        }
                        return true;
                    },
                    InterfaceScaleType.UI));
            }
        }

        public override void Load()
        {
            if (!Main.dedServ)
            {
                ExampleInterface = new UserInterface();

                MyUI = new TheUI();
                MyUI.Activate();
            }
        }

        public override void Unload()
        {
            MyUI = null;
        }

        internal void ShowMyUI()
        {
            ExampleInterface?.SetState(MyUI);
        }

        internal void HideMyUI()
        {
            ExampleInterface?.SetState(null);
        }
    }
}
