using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.PreHardmode.IceBarrier
{
    public class IceBarrierCore : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/IceBarrier/IceBarrierCore";

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;

            Item.maxStack = 999;
            Item.value = Item.buyPrice(silver: 1);
            Item.rare = ItemRarityID.Blue;

            Item.material = true;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(5);
            recipe.AddIngredient(ItemID.IceBlock, 25);
            recipe.AddIngredient(ItemID.SnowBlock, 25);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }

    }

    public class IceBarrierCoreDrops : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            // Pre-Hardmode Snow enemies
            if (npc.type == NPCID.IceBat ||
                npc.type == NPCID.SnowFlinx ||
                npc.type == NPCID.IceSlime ||
                npc.type == NPCID.ZombieEskimo)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<IceBarrierCore>(), 10));
                // 1 in 10 chance
            }

            // Hardmode Snow enemies
            if (npc.type == NPCID.IceElemental ||
                npc.type == NPCID.IcyMerman ||
                npc.type == NPCID.IceTortoise)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<IceBarrierCore>(), 3));
                // 1 in 3 chance
            }
        }
    }
}