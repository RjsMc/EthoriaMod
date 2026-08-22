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


            Item.damage = 1000;
            Item.value = Item.sellPrice(100);
        }

    }
}
