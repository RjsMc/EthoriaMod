using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace EthoriaMod.Content.UI.Dialogue
{
    public class TextSettings
    {
        public float Delay { set; get; } = 0.2f;
        public bool Bold { set; get; } = false;
        public bool Italicized { set; get; } = false;
        public Color Color { set; get; } = Color.White;
        public float Size { set; get; } = 1f;

        public TextSettings Clone()
        {
            return new TextSettings
            {
                Delay = Delay,
                Bold = Bold,
                Italicized = Italicized,
                Color = Color,
                Size = Size
            };
        }
    }
}
