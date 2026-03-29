using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using NaturiumMod.Content.BuffsDebuffs;

namespace NaturiumMod.Content.Items.PreHardmode.Shark
{
    public class HammerShark : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Shark/HammerShark";

        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 50;

            Item.damage = 20;
            Item.knockBack = 7f;
            Item.useTime = 32;
            Item.useAnimation = 32;
            Item.UseSound = SoundID.Item1;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.DamageType = DamageClass.Melee;

            Item.autoReuse = true;
            Item.value = Item.buyPrice(silver: 90);
            Item.rare = ItemRarityID.Blue;
            Item.useTurn = true;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<BleedDebuff>(), 240); // 4 seconds

            // Small shockwave effect
            for (int i = 0; i < 10; i++)
            {
                Dust d = Dust.NewDustDirect(target.position, target.width, target.height, DustID.Water);
                d.velocity *= 1.2f;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SharkFin, 3)
                .AddIngredient(ItemID.IronBar, 30)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

}
