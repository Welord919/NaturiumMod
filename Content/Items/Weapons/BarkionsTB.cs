using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModdingGang.Content.Items.Materials;

namespace ModdingGang.Content.Items.Weapons
{
    public class BarkionsTB : ModItem
    {
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Melee;
            Item.damage = 23;
            Item.width = 40;
            Item.height = 50;
            Item.useTime = 33;
            Item.useAnimation = 30;
            Item.knockBack = 4.5f;
            Item.crit = 8;
            Item.UseSound = SoundID.Item1;
            Item.useStyle = ItemUseStyleID.Swing;

            Item.value = Item.buyPrice(gold: 3);
            Item.rare = ItemRarityID.Green;
            Item.autoReuse = true;

            Item.shootSpeed = 12f;
            Item.shoot = ProjectileID.ChlorophyteOrb;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            // Inflict the OnFire debuff for 1 second onto any NPC/Monster that this hits.
            // 60 frames = 1 second
            target.AddBuff(BuffID.Poisoned, 240);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<NaturiumBar>(), 15);
            recipe.AddIngredient(ItemID.BladeofGrass, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}