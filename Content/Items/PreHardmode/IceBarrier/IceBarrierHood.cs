using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.PreHardmode.IceBarrier
{
    [AutoloadEquip(EquipType.Head)]
    public class IceBarrierHood : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/IceBarrier/IceBarrierHood";

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.buyPrice(silver: 30);
            Item.rare = ItemRarityID.Blue;
            Item.defense = 5;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Magic) += 0.04f;
            player.statManaMax2 += 20;

        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<IceBarrierRobe>()
                && legs.type == ModContent.ItemType<IceBarrierLeggings>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "+10% magic & summon damage\n" +
                              "Immune to Frostburn";

            player.GetDamage(DamageClass.Magic) += 0.10f;
            player.GetDamage(DamageClass.Summon) += 0.10f;

            // IMMUNITY TO FROSTBURN
            player.buffImmune[BuffID.Frostburn] = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<IceBarrierCore>(), 10);
            recipe.AddIngredient(ItemID.FlinxFur, 5);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }

    }
}