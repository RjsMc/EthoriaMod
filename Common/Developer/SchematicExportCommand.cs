using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Terraria;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using Point = Microsoft.Xna.Framework.Point;
using EthoriaMod.Content.Items.DeveloperTools.EthoriaExporter;
using System.IO;

namespace EthoriaMod.Common.Developer
{
    public class SchematicExportCommand : ModCommand
    {
        public override CommandType Type => CommandType.Chat;
        public override string Command => "exportstructure";
        public override string Description => "Exports selected region set by the EthoriaExporter.";
        public override string Usage => "/exportstructure <filename>";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (args.Length < 1) 
            { 
                caller.Reply("[Exporter] Please specify a filename! Usage: /exportstructure <filename>", Color.Red);
                return;
            }
            if (args.Length > 1)
            {
                caller.Reply("[Exporter] Too many arguments! Usage: /exportstructure <filename>", Color.Red);
                return;
            }

            string filename = args[0].ToLower();

            Point a = EthoriaExporter.CornerA;
            Point b = EthoriaExporter.CornerB;

            if (a == Point.Zero || b == Point.Zero)
            {
                caller.Reply("[Exporter] Both corners must be specified! Use the EthoriaExporter developer item.", Color.Red);
                return;
            }

            string directoryPath = Path.Combine(Main.SavePath, "ModSources", "EthoriaMod", "Assets", "Structures");
            if (!Directory.Exists(directoryPath))
            {
                caller.Reply("[Exporter] Directory not found. Created a new one.", Color.Yellow);
                Directory.CreateDirectory(directoryPath);
            }

            string fullPath = Path.Combine(directoryPath, $"{filename}.dat");

            try
            {
                caller.Reply("[Exporter] Exporting...", Color.Yellow);
                bool success = EthoriaStructureExporter.SaveStructure(a, b, fullPath);
                if (success)
                {
                    caller.Reply($"[Exporter] Export successful! File saved to: {fullPath}", Color.LimeGreen);
                }
                else
                {
                    caller.Reply("[Exporter] Export aborted!", Color.Orange);
                }
            }
            catch (Exception ex)
            {
                caller.Reply($"[Exporter] Error while exporting: {ex.Message}", Color.Red);
            }
        }

    }
}
