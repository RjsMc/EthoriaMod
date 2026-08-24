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
            if (!File.Exists(filePath))
            {
                Main.NewText($"[Exporter] File not found: {filePath}!", 255, 0, 0);
                return false;
            }

            TagCompound root = TagIO.FromFile(filePath);
            return LoadStructure(origin, root);
        }

        public static bool LoadStructure(Point origin, TagCompound root)
        {
            var tiles = root.GetList<TagCompound>("tiles");

            int width = root.GetInt("width");
            int height = root.GetInt("height");

            foreach (var tag in tiles)
            {
                int worldX = origin.X + tag.GetInt("x");
                int worldY = origin.Y + tag.GetInt("y");

                if (!Terraria.WorldGen.InWorld(worldX, worldY)) continue;

                Tile tile = Main.tile[worldX, worldY];
                tile.HasTile = tag.GetBool("hasTile");

                if (tile.HasTile)
                {
                    tile.TileType = (ushort)tag.GetAsShort("type");
                    tile.TileFrameX = tag.GetAsShort("fx");
                    tile.TileFrameY = tag.GetAsShort("fy");

                    if ((tile.TileType == TileID.Containers || tile.TileType == TileID.Containers2) && tile.TileFrameX % 36 == 0 && tile.TileFrameY == 0) Chest.CreateChest(worldX, worldY);
                }

                tile.WallType = (ushort)tag.GetAsShort("wall");
            }

            for (int x = -1; x <= width; x++)
            {
                for (int y = -1; y <= height; y++)
                {
                    int worldX = origin.X + x;
                    int worldY = origin.Y + y;

                    if (!Terraria.WorldGen.InWorld(worldX, worldY)) continue;

                    Tile tile = Main.tile[worldX, worldY];

                    Terraria.WorldGen.SquareWallFrame(worldX, worldY, true);

                    if (tile.HasTile && !Main.tileFrameImportant[tile.TileType]) Terraria.WorldGen.SquareTileFrame(worldX, worldY, true);
                }
            }

            return true;
        }
    }
}
