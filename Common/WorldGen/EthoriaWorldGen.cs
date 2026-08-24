using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;
using Point = Microsoft.Xna.Framework.Point;

using Microsoft.Xna.Framework;
using EthoriaMod.Content.Items.DeveloperTools.EthoriaExporter;
using EthoriaMod.Common.Developer;

namespace EthoriaMod.Common.WorldGen
{
    internal class EthoriaWorldGen : ModSystem
    {
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {

            int passIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Micro Biomes"));

            if (passIndex != -1)
            {
                tasks.Insert(passIndex + 1, new PassLegacy("Ethoria: Mass Structures", MassStructurePlacement));
            }
        }

        private void MassStructurePlacement(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "DOING SOME MAGIC SILLY!!!!!!!!!!!!!!!!!!!!!";

            string assetPath = "Assets/Structures/surprise.dat";

            if (!Mod.FileExists(assetPath))
            {
                Mod.Logger.Warn($"[Ethoria WorldGen] Could not find embedded file at: {assetPath}");
                return;
            }

            TagCompound schematicData;
            using (Stream stream = Mod.GetFileStream(assetPath))
            {
                schematicData = TagIO.FromStream(stream);
            }

            int structuresPlaced = 0;
            int maxAttempts = 50000;
            int attempts = 0;

            while (structuresPlaced < 4000 && attempts < maxAttempts)
            {
                attempts++;

                int randomX = Terraria.WorldGen.genRand.Next(50, Main.maxTilesX - 50);
                int randomY = Terraria.WorldGen.genRand.Next(50, Main.maxTilesY - 200);

                Point spawnPoint = new Point(randomX, randomY);

                if (EthoriaStructureExporter.LoadStructure(spawnPoint, schematicData))
                {
                    structuresPlaced++;
                }

                progress.Set((float)structuresPlaced / 2000f);
            }

            Mod.Logger.Info($"[Ethoria WorldGen] Successfully placed {structuresPlaced} structures!");
        }
    }
}