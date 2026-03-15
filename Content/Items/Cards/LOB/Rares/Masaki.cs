using Microsoft.Xna.Framework;
using NaturiumMod.Content.Helpers;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Cards.LOB.Commons
{
    public class Masaki : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/Masaki";

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.UseSound = SoundID.Item4;
            Item.consumable = true;
            Item.maxStack = 999;
            Item.rare = ItemRarityID.Green;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.value = Item.buyPrice(silver: 25);
        }

        public override bool? UseItem(Player player)
        {
            player.AddBuff(ModContent.BuffType<PetiteDragonBuff>(), 60 * 30);
            return true;
        }
    }
}