using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Point = Microsoft.Xna.Framework.Point;

namespace EthoriaMod.Common.Developer
{
    public class SchematicImportCommand : ModCommand
    {
        public override CommandType Type => CommandType.Chat;
        public override string Command => "importstructure";
        public override string Description => "Imports specified schematic file.";
        public override string Usage => "/importstructure <filename>";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (args.Length != 1)
            {
                caller.Reply("[Importer] Usage: /importstructure <filename>", Color.Red);
                return;
            }

            string filename = args[0].ToLower().Trim();
            string fullPath = Path.GetFullPath(Path.Combine(Main.SavePath, "ModSources", "EthoriaMod", "Assets", "Structures", $"{filename}.dat"));

            if (!File.Exists(fullPath))
            {
                caller.Reply($"[Importer] Could not find file at path:\n{fullPath}", Color.Red);
                return;
            }

            Point mouseTile = Main.MouseWorld.ToTileCoordinates();

            caller.Reply($"[Importer] Attempting to place {filename} at ({mouseTile.X}, {mouseTile.Y})...", Color.Yellow);

            EthoriaStructureExporter.LoadStructure(mouseTile, fullPath);

            caller.Reply($"[Importer] Structure placed successfully at ({mouseTile.X}, {mouseTile.Y})!", Color.LimeGreen);
        }
    }
}
