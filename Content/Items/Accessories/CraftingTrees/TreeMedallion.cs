namespace NaturiumMod.Content.Items.Accessories
{
    using global::NaturiumMod.Content.Helpers;
    using global::NaturiumMod.Content.Items.Cards.Fusion;
    using global::NaturiumMod.Content.Items.Materials;
    using global::NaturiumMod.Content.Items.Tools;
    using global::NaturiumMod.Content.Tiles.Furniture;
    using Terraria;
    using Terraria.ID;
    using Terraria.ModLoader;

    namespace NaturiumMod.Content.Items.Accessories
    {
        //[AutoloadEquip(EquipType.Neck)]
        public class TreeMedallion : ModItem
        {
            public override string Texture => "NaturiumMod/Assets/Items/Accessories/TreeMedallion";

            public override void SetDefaults()
            {
                Item.width = 28;
                Item.height = 28;
                Item.accessory = true;
                Item.rare = ItemRarityID.LightRed;
                Item.value = Item.buyPrice(gold: 5);
            }

            public override void UpdateAccessory(Player player, bool hideVisual)
            {
                // Weapon boost medallions
                var boost = player.GetModPlayer<WeaponBoostPlayer>();
                boost.activeBoosts["Barkion"] = true;
                boost.activeBoosts["Exterio"] = true;
                boost.activeBoosts["Leodrake"] = true;
                boost.activeBoosts["Nibiru"] = true;

                // Ice Barrier medallion effects
                player.GetModPlayer<IceDamagePlayer>().iceMedallionActive = true;
                player.GetModPlayer<FrostburnMinionPlayer>().frostburnMinions = true;
                player.GetModPlayer<MinionInfoPlayer>().minionDisplayEquipped = true;
            }
            public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
            {
                // All old medallion types
                int barkion = ModContent.ItemType<BarkionsMedallion>();
                int exterios = ModContent.ItemType<ExteriosMedallion>();
                int ice = ModContent.ItemType<IceBarrierMedallion>();
                int leodrake = ModContent.ItemType<LeodrakesMedallion>();
                int nibiru = ModContent.ItemType<NibiruMedallion>();

                // If the incoming item is the  Medallion,
                // block equipping if ANY old medallion is already equipped.
                if (incomingItem.type == Type)
                {
                    return equippedItem.type != barkion &&
                           equippedItem.type != exterios &&
                           equippedItem.type != ice &&
                           equippedItem.type != leodrake &&
                           equippedItem.type != nibiru;
                }

                // If the incoming item IS one of the old medallions,
                // block equipping if already equipped.
                if (incomingItem.type == barkion ||
                    incomingItem.type == exterios ||
                    incomingItem.type == ice ||
                    incomingItem.type == leodrake ||
                    incomingItem.type == nibiru)
                {
                    return equippedItem.type != Type;
                }

                return true;
            }

            public override void AddRecipes()
            {
                CreateRecipe()
                    .AddIngredient(ModContent.ItemType<BarkionsMedallion>())
                    .AddIngredient(ModContent.ItemType<ExteriosMedallion>())
                    .AddIngredient(ModContent.ItemType<IceBarrierMedallion>())
                    .AddIngredient(ModContent.ItemType<LeodrakesMedallion>())
                    .AddIngredient(ModContent.ItemType<NibiruMedallion>())
                    .AddIngredient(ModContent.ItemType<InfusedNaturiumBar>(), 25)
                    .AddIngredient(ModContent.ItemType<EarthEssence>(), 15)
                    .AddTile(TileID.TinkerersWorkbench)
                    .Register();
            }
        }
    }

}
