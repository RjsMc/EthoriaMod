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
using EthoriaMod.Content.Dialogue;

namespace EthoriaMod.Content.UI.Dialogue
{
    public class DialogueUI : UIState
    {
        public DialogueBox DialogueBox { get; private set; }
        public override void OnInitialize()
        {
            DialogueBox = new DialogueBox();
            Append(DialogueBox);
        }

        public void SetSession(DialogueSession session)
        {
            DialogueBox.SetSession(session);
        }

        //private GameTime _lastUpdateUiGameTime;
        //internal UserInterface ExampleInterface;

        //public class TheUI : UIState
        //{
        //    private UIText text;
        //    private UIClickableButton _button;
        //    public override void OnInitialize()
        //    {
        //        UIPanel panel = new UIPanel();
        //        panel.Width.Set(300, 0);
        //        panel.Height.Set(300, 0);
        //        panel.HAlign = panel.VAlign = 0.5f;
        //        Append(panel);

        //        UIText textHeader = new UIText("My UI Header");
        //        textHeader.HAlign = 0.5f;
        //        textHeader.Top.Set(15, 0);
        //        panel.Append(textHeader);

        //        _button = new UIClickableButton("Click me!", OnButtonClick);
        //        _button.Width.Set(100, 0);
        //        _button.Height.Set(50, 0);
        //        _button.HAlign = 0.5f;
        //        _button.Top.Set(25, 0);
        //        panel.Append(_button);
        //    }

        //    private void OnButtonClick(UIMouseEvent evt, UIElement listeningElement)
        //    {
        //        _button.Text = "I was clicked";
        //    }
        //}

        //internal TheUI MyUI;

        //public override void UpdateUI(GameTime gameTime)
        //{
        //    _lastUpdateUiGameTime = gameTime;
        //    if (ExampleInterface?.CurrentState != null)
        //    {
        //        ExampleInterface.Update(gameTime);
        //    }
        //}

        //public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        //{
        //    int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
        //    if (mouseTextIndex != -1)
        //    {
        //        layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
        //            "MyMod: My Interface",
        //            delegate
        //            {
        //                if (_lastUpdateUiGameTime != null && ExampleInterface?.CurrentState != null)
        //                {
        //                    ExampleInterface.Draw(Main.spriteBatch, _lastUpdateUiGameTime);
        //                }
        //                return true;
        //            },
        //            InterfaceScaleType.UI));
        //    }
        //}

        //public override void Load()
        //{
        //    if (!Main.dedServ)
        //    {
        //        ExampleInterface = new UserInterface();

        //        MyUI = new TheUI();
        //        MyUI.Activate();
        //    }
        //}

        //public override void Unload()
        //{
        //    MyUI = null;
        //}

        //internal void ShowMyUI(int open)
        //{
        //    switch (open)
        //    {
        //        case 1:
        //            ExampleInterface?.SetState(MyUI);
        //            break;
        //    }
        //}


        //internal void HideMyUI()
        //{
        //    ExampleInterface?.SetState(null);
        //}
    }
}
