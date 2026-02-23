using NaturiumMod.Content.Generation.Ores;
using NaturiumMod.Content.Items.PreHardmode.Accessories.CharmPieces;
using NaturiumMod.Content.Items.PreHardmode.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod
{
    public class Shimmer : ModSystem
    {
        public override void PostSetupContent()
        {
            RegisterShimmerTransforms();
        }

        private void RegisterShimmerTransforms()
        {
            ItemID.Sets.ShimmerTransformToItem[
                ModContent.ItemType<CharmPieceAngler>()
            ] = ModContent.ItemType<CharmPieceArtisan>();

            ItemID.Sets.ShimmerTransformToItem[
                ModContent.ItemType<CharmPieceArtisan>()
            ] = ModContent.ItemType<CharmPieceGuardian>();

            ItemID.Sets.ShimmerTransformToItem[
                ModContent.ItemType<CharmPieceGuardian>()
            ] = ModContent.ItemType<CharmPieceMystic>();

            ItemID.Sets.ShimmerTransformToItem[
                ModContent.ItemType<CharmPieceMystic>()
            ] = ModContent.ItemType<CharmPieceWarrior>();

            ItemID.Sets.ShimmerTransformToItem[
                ModContent.ItemType<CharmPieceWarrior>()
            ] = ModContent.ItemType<CharmPieceAngler>();
        }


        public override void Unload()
        {
            // Clean up static references here if needed
        }
    }
}
