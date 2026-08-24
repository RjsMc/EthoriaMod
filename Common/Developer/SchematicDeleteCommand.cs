using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace EthoriaMod.Common.Developer
{
    public class SchematicDeleteCommand : ModCommand
    {
        public override CommandType Type => CommandType.Chat;
        public override string Command => "ds";
        public override string Description => "Deletes specified schematic file.";
        public override string Usage => "/ds <filename>";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (args.Length != 1)
            {
                caller.Reply("[Delete] Usage: /ds <filename>", Color.Red);
                return;
            }

            string filename = args[0].ToLower().Trim();
            string fullPath = Path.GetFullPath(Path.Combine(Main.SavePath, "ModSources", "EthoriaMod", "Assets", "Structures", $"{filename}.dat"));

            if (!File.Exists(fullPath))
            {
                caller.Reply($"[Delete] Schematic file '{filename}' does not exist!", Color.Red);
                return;
            }

            try
            {
                File.Delete(fullPath);
                caller.Reply($"[Delete] Successfully deleted schematic '{filename}'!", Color.LimeGreen);
            }
            catch (Exception ex)
            {
                caller.Reply($"[Delete] Failed to delete file: {ex.Message}", Color.Red);
            }
        }
    }
}
