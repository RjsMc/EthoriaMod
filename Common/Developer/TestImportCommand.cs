using Humanizer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.IO;
using Terraria.ModLoader;

namespace EthoriaMod.Common.Developer
{
    public class TestImportCommand : ModCommand
    {
        public static string schematicfile = "";
        public static string schematicpath = "";
        public override CommandType Type => CommandType.Chat;
        public override string Command => "testpath";
        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (args.Length < 1) {
                Main.NewText("Please input a schematic name.", 255, 0, 0);
                return;
            }

            if (args.Length > 1)
            {
                Main.NewText("Too many arguments!", 255, 0, 0);
                return;
            }

            schematicfile = args[0].ToLower();
            schematicpath = $"EthoriaMod/Assets/Structures/{schematicfile}.bin";
            Main.NewText($"Searching for path... {schematicpath}", 255, 255, 0);
            
            if (ModContent.FileExists(schematicpath))
            {
                byte[] fileData = ModContent.GetFileBytes(schematicpath);
                long byteCount = fileData.Length;
                string readableSize = byteCount.Bytes().Humanize();
                Main.NewText($"File Found! Size: {byteCount}, Bytes: ({readableSize})", 0, 255, 0);
                return;
            }
            else
            {
                Main.NewText("File not found!", 255, 0, 0);
                return;
            }
        }
    }
}
