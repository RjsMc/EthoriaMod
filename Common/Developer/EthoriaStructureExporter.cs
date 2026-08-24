using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Point = Microsoft.Xna.Framework.Point;
using System.IO;
using Terraria.ModLoader.IO;
using Terraria.ID;

namespace EthoriaMod.Common.Developer
{
    public class EthoriaStructureExporter
    {
        public static bool SaveStructure(Point cornerA, Point cornerB, string filePath)
        {
            if (File.Exists(filePath))
            {
                Main.NewText($"[Exporter] File '{Path.GetFileName(filePath)}' already exists!", 255, 0, 0);
                return false;
            }
            int minX = Math.Min(cornerA.X, cornerB.X);
            int minY = Math.Min(cornerA.Y, cornerB.Y);
            int maxX = Math.Max(cornerA.X, cornerB.X);
            int maxY = Math.Max(cornerA.Y, cornerB.Y);

            List<TagCompound> tiles = new List<TagCompound>();

            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    Tile tile = Main.tile[x, y];

                    if (!tile.HasTile && tile.WallType == WallID.None) continue;

                    tiles.Add(new TagCompound
                    {
                        ["x"] = x - minX,
                        ["y"] = y - minY,
                        ["type"] = tile.TileType,
                        ["fx"] = tile.TileFrameX,
                        ["fy"] = tile.TileFrameY,
                        ["wall"] = tile.WallType,
                        ["hasTile"] = tile.HasTile,
                    });
                }
            }

            if (tiles.Count == 0)
            {
                Main.NewText($"[Exporter] Export aborted. Selected area contains no tiles!", 255, 0, 0);
                return false;
            }

            TagCompound root = new TagCompound
            {
                ["width"] = maxX - minX + 1,
                ["height"] = maxY - minY + 1,
                ["tiles"] = tiles
            };

            TagIO.ToFile(root, filePath);
            return true;
        }

        public static bool LoadStructure(Point origin, string filePath)
        {
            TagCompound root = TagIO.FromFile(filePath);
            var tiles = root.GetList<TagCompound>("tiles");

            foreach (var tag in tiles)
            {
                int worldX = origin.X + tag.GetInt("x");
                int worldY = origin.Y + tag.GetInt("y");

                if (!WorldGen.InWorld(worldX, worldY)) continue;

                Tile tile = Main.tile[worldX, worldY];
                tile.HasTile = tag.GetBool("hasTile");

                if (tile.HasTile)
                {
                    tile.TileType = (ushort)tag.GetAsShort("type");
                    tile.TileFrameX = tag.GetAsShort("fx");
                    tile.TileFrameY = tag.GetAsShort("fy");
                }

                tile.WallType = (ushort)tag.GetAsShort("wall");
            }
            return true;
        }
    }
}
