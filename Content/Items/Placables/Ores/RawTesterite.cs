using EthoriaMod.Content.Tiles.Ores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;

namespace EthoriaMod.Content.Items.Placables.Ores
{
    internal class RawTesterite : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Placables";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
            ItemID.Sets.SortingPriorityMaterials[Type] = 99; // Luminite
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Testerite>());
            Item.Size = new(12);
            Item.value = 5000;
            Item.rare = ItemRarityID.Orange;
        }
    }
}
