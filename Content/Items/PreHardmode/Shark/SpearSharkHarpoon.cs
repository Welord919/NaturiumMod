using NaturiumMod.Content.BuffsDebuffs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.PreHardmode.Shark
{
    public class SpearSharkHarpoon : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Shark/SpearSharkHarpoon";

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 18;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 22;
            Item.useTime = 22;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item40;
            Item.useTurn = true;
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 15;
            Item.knockBack = 3f;

            Item.shootSpeed = 14f;
            Item.shoot = ModContent.ProjectileType<SpearSharkHarpoonProj>();

            Item.noMelee = true;
            Item.value = Item.buyPrice(silver: 25);
            Item.rare = ItemRarityID.Blue;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SharkFin, 5)
                .AddIngredient(ItemID.IronBar, 10)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
    public class SpearSharkHarpoonProj : ModProjectile
    {
        public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Shark/SpearSharkHarpoonProj";

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;

            Projectile.aiStyle = ProjAIStyleID.Harpoon;
            AIType = ProjectileID.Harpoon;

            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 3; // pierces 3 enemies
            Projectile.DamageType = DamageClass.Ranged;

            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // Apply your custom bleed debuff
            target.AddBuff(ModContent.BuffType<BleedDebuff>(), 240); // 4 seconds
        }

        public override void AI()
        {
            // Slight water‑trail effect
            if (Main.rand.NextBool(3))
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Water);
                d.velocity *= 0.2f;
                d.scale = 1.1f;
            }
        }
    }

}
