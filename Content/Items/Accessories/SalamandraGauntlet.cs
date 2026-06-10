using NaturiumMod.Content.Items.Cards.Fusion.FusionCards;
using NaturiumMod.Content.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

//[AutoloadEquip(EquipType.HandsOn)]
public class SalamandrasGauntlet : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/Accessories/SalamandraGauntlet";

    public override void SetDefaults()
    {
        Item.width = 26;
        Item.height = 30;
        Item.accessory = true;
        Item.rare = ItemRarityID.Yellow;
        Item.value = Item.buyPrice(gold: 80);
        Item.defense = 8;
    }
    public override bool CanEquipAccessory(Player player, int slot, bool modded)
    {
        for (int i = 3; i < 10 + player.extraAccessorySlots; i++)
        {
            Item acc = player.armor[i];

            if (acc.type == ItemID.FireGauntlet ||
                acc.type == ItemID.BerserkerGlove)
            {
                return false;
            }
        }

        return base.CanEquipAccessory(player, slot, modded);
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        var fs = player.GetModPlayer<FlameSwordsmanPlayer>();
        fs.salamandraGauntletEquipped = true;

        // Base melee boosts
        player.GetDamage(DamageClass.Melee) += 0.10f;
        player.GetAttackSpeed(DamageClass.Melee) += 0.25f;
        player.GetCritChance(DamageClass.Melee) += 5;
        player.autoReuseGlove = true;
        player.aggro += 400;
        player.meleeScaleGlove = true;


        // Below 50% HP buff
        if (player.statLife < player.statLifeMax2 * 0.5f)
        {
            player.endurance += 0.10f;
            player.GetDamage(DamageClass.Melee) += 0.10f;
            player.GetAttackSpeed(DamageClass.Melee) += 0.15f;
            player.GetCritChance(DamageClass.Melee) += 5;
        }
    }


    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe.AddIngredient(ItemID.FireGauntlet);
        recipe.AddIngredient(ItemID.BerserkerGlove);
        recipe.AddIngredient(ModContent.ItemType<FlameSwordsman>(), 5);
        recipe.AddIngredient(ItemID.SoulofLight, 10);
        recipe.AddIngredient(ItemID.SoulofNight, 10);
        recipe.AddIngredient(ModContent.ItemType<NaturesEssence>(), 10);
        recipe.AddTile(TileID.TinkerersWorkbench);
        recipe.Register();
    }
    public class SalamandraEquipBlocker : GlobalItem
    {
        public override bool CanEquipAccessory(Item item, Player player, int slot, bool modded)
        {
            // If the item being equipped is Fire Gauntlet or Berserker Glove
            if (item.type == ItemID.FireGauntlet || item.type == ItemID.BerserkerGlove)
            {
                // Check if Salamandra's Gauntlet is already equipped
                for (int i = 3; i < 10 + player.extraAccessorySlots; i++)
                {
                    if (player.armor[i].type == ModContent.ItemType<SalamandrasGauntlet>())
                    {
                        return false;
                    }
                }
            }

            return base.CanEquipAccessory(item, player, slot, modded);
        }
    }
}