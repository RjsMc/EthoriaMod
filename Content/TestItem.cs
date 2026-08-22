using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EthoriaMod.Content
{
    internal class TestItem : ModItem
    {
        public override void SetDefaults()
        {

            Item.useStyle = ItemUseStyleID.Swing;

            Item.useAnimation = 12;
            Item.useTime = 12;
            Item.autoReuse = true;

            Item.width = 32;
            Item.height = 32;
            Item.scale = 3;

            Item.damage = 1000;
            Item.value = Item.sellPrice(100);
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            float customScale = scale * 2.0f;

            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            spriteBatch.Draw(texture, position, frame, drawColor, 0f, origin, customScale, SpriteEffects.None, 0f);

            return false;
        }

    }
}
