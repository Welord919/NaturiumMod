using NaturiumMod.Content.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class HelshadiumHelmet : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Armor/HelshadiumHelmet";

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.buyPrice(silver: 30);
            Item.rare = ItemRarityID.Orange;
            Item.defense = 10;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetCritChance(DamageClass.Melee) += 0.08f;
            player.GetAttackSpeed(DamageClass.Melee) += 0.04f;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<HelshadiumChestplate>()
                && legs.type == ModContent.ItemType<HelshadiumLeggings>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus =
                "+10% melee damage\n" +
                "Immune to Hellfire";
            player.GetDamage(DamageClass.Melee) += 0.10f;
            player.buffImmune[BuffID.OnFire3] = true;

        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Helshadium>(), 10);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
