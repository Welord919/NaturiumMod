using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.Cards.Fusion;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Cards.Crafted
{
    public class StopDefense : BaseCardCrafted
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/Crafted/StopDefense";
        public override string CardAttribute => "Spell";
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.buffType = ModContent.BuffType<StopDefenseBuff>();
            Item.buffTime = 60 * 30;
        }

        public override bool CanUseItem(Player player)
        {
            return !player.HasBuff(Item.buffType);
        }

        public override bool? UseItem(Player player)
        {
            player.AddBuff(ModContent.BuffType<SummoningSickness>(), 180);
            return true;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe = RecipeHelper.GetNewRecipe(recipe, [
            new(ItemID.ObsidianShield, 1),
            new(ModContent.ItemType<SpellEssence>(), 5)
            ], ModContent.TileType<FusionAltarTile>());
            recipe.Register();
        }
    }
    public class StopDefenseBuff : ModBuff
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/Crafted/StopDefenseBuff";
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetArmorPenetration(DamageClass.Generic) += 8;
        }
    }
}
