using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Point = Microsoft.Xna.Framework.Point;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace EthoriaMod.Content.Items.DeveloperTools.EthoriaExporter
{
    public class EthoriaExporter : ModItem
    {
        public static Point CornerA = Point.Zero;
        public static Point CornerB = Point.Zero;

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.rare = ItemRarityID.Red;
        }

        public override bool AltFunctionUse(Player player) => true;

        public override bool? UseItem(Player player)
        {
            Point mouseTile = Main.MouseWorld.ToTileCoordinates();
            if (player.altFunctionUse == 2)
            {
                CornerB = mouseTile;
                Main.NewText($"[Exporter] Corner B set to: ({CornerB.X}, {CornerB.Y})", 0, 255, 0);
            } 
            else
            {
                CornerA = mouseTile;
                Main.NewText($"[Exporter] Corner A set to: ({CornerA.X}, {CornerA.Y})", 0, 255, 0);
            }

            return true;
        }
    }
}
