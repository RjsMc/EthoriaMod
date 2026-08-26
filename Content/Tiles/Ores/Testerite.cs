using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.Skies;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace EthoriaMod.Content.Tiles.Ores
{
    public class Testerite : ModTile
    {
        public class RawTesterite : ModItem
        {
            public override void SetStaticDefaults()
            {
                Item.ResearchUnlockCount = 100;
                ItemID.Sets.SortingPriorityMaterials[Type] = 58; // 58 is for ores
            }

            public override void SetDefaults()
            {
                Item.DefaultToPlaceableTile(ModContent.TileType<Testerite>());
                Item.Size = new(12);
                Item.value = 5000;
            }
        }
        class TesteriteDust : ModDust
        {
            public override void OnSpawn(Dust dust)
            {
                UpdateType = DustID.Copper;
            }
        }
        public override void SetStaticDefaults()
        {
            TileID.Sets.Ore[Type] = true;
            TileID.Sets.FriendlyFairyCanLureTo[Type] = true;
            Main.tileSpelunker[Type] = true;
            Main.tileOreFinderPriority[Type] = 410;
            Main.tileShine[Type] = 975; // AmbientSky shiny dust
            Main.tileShine2[Type] = true; // Glow a bit
            Main.tileMergeDirt[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;

            LocalizedText name = CreateMapEntryName(); // Help create a key for map entry (hovering over on the map)
            AddMapEntry(new Microsoft.Xna.Framework.Color(0, 255, 0), name);

            DustType = ModContent.DustType<TesteriteDust>();
            VanillaFallbackOnModDeletion = TileID.Platinum; // Replace instance if mod is not loaded, turn into platinum

            HitSound = SoundID.NPCDeath10;
            MineResist = 1f;

            MinPick = 55; // Gold Pickaxe Level
        }
    }

    public class TesteriteSystem : ModSystem
    {
        public static LocalizedText TesteritePassMessage { get; private set; }

        public override void SetStaticDefaults()
        {
            TesteritePassMessage = Mod.GetLocalization($"WorldGen.{nameof(TesteritePassMessage)}");
        }

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {
            int index = tasks.FindIndex(pass => pass.Name.Equals("Shinies"));
            if (index != -1)
            {
                tasks.Insert(index + 1, new TesteritePass("Generating Testerite", 237.4298f));
            }
        }
    }

    public class TesteritePass : GenPass
    {
        public TesteritePass(string name, float loadWeight) : base(name, loadWeight)
        {

        }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            for (int k = 0; k < (int)(Main.maxTilesX * Main.maxTilesY * 0.001); k++) // Loop a certain an amount of times of the percentage of the world in tiles (instead of a set number, so it can run more on bigger worlds)
            {
                int x = WorldGen.genRand.Next(0, Main.maxTilesX); // Only use genRand to ensure it works with terrarias seed system
                int y = WorldGen.genRand.Next((int)GenVars.worldSurfaceLow, Main.maxTilesY); // Higher Y, lower we are. (int)GenVars.worldSurface ignores from above surface.

                Tile tile = Framing.GetTileSafely(x, y);
                if (tile.HasTile && tile.TileType == TileID.SnowBlock) // Only generate in the snow biome. You can delete this line to generate throughout the entire world
                {
                    WorldGen.TileRunner(x, y, WorldGen.genRand.Next(5, 12), WorldGen.genRand.Next(5, 10), ModContent.TileType<Testerite>());
                }
            }
        }
    }
}
