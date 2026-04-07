using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.Cards;
using NaturiumMod.Content.Items.Cards.Fusion;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Cards.LOB.Commons
{
    public class Hinotama : BaseCardCommon
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/NoEffects/Hinotama";

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.damage = 50;
            Item.knockBack = 8f;
            Item.shoot = ModContent.ProjectileType<HinotamaFireball>();
            Item.shootSpeed = 12f;
            Item.UseSound = SoundID.Item20;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.HasBuff(ModContent.BuffType<SummoningSickness>())) return false;
            return true;
        }

        public override bool? UseItem(Player player)
        {
            player.AddBuff(ModContent.BuffType<SummoningSickness>(), 30);

            if (Main.myPlayer == player.whoAmI)
            {
                Vector2 dir = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.UnitX);
                int p = Projectile.NewProjectile(
                    player.GetSource_ItemUse(Item),
                    player.Center,
                    dir * Item.shootSpeed,
                    Item.shoot,
                    Item.damage,
                    Item.knockBack,
                    player.whoAmI
                );
                if (p >= 0) Main.projectile[p].originalDamage = Item.damage;
            }

            return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo src,
            Vector2 pos, Vector2 vel, int type, int dmg, float kb) => false;
    }
    public class HinotamaSpell : BaseCardRare
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/HinotamaSpell";

        private int usesLeft = 4;
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.consumable = false;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.damage = 25;
            Item.knockBack = 3f;
            Item.shoot = ModContent.ProjectileType<HinotamaFireball>();
            Item.shootSpeed = 10f;
            Item.UseSound = SoundID.Item20;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(4);
            recipe = RecipeHelper.GetNewRecipe(recipe, [
                new(ModContent.ItemType<Hinotama>(), 4),
                new(ModContent.ItemType<FireEssence>(), 4),
                new(ModContent.ItemType<SpellEssence>(), 5),
            ], ModContent.TileType<FusionAltarTile>());
            recipe.Register();
        }
        public override bool CanUseItem(Player player)
        {
            if (usesLeft <= 0) return false;
            if (player.HasBuff(ModContent.BuffType<SummoningSickness>())) return false;
            return true;
        }

        public override bool? UseItem(Player player)
        {
            usesLeft--;
            if (usesLeft <= 0)
            {
                Item.stack--;
                usesLeft = 4;
            }

            player.AddBuff(ModContent.BuffType<SummoningSickness>(), 30);

            if (Main.myPlayer == player.whoAmI)
            {
                Vector2 dir = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.UnitX);
                int p = Projectile.NewProjectile(
                    player.GetSource_ItemUse(Item),
                    player.Center,
                    dir * Item.shootSpeed,
                    Item.shoot,
                    Item.damage,
                    Item.knockBack,
                    player.whoAmI
                );
                if (p >= 0) Main.projectile[p].originalDamage = Item.damage;
            }

            return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo src,
            Vector2 pos, Vector2 vel, int type, int dmg, float kb) => false;
    }
    public class HinotamaFireball : ModProjectile
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/HinotamaFireball";

        public override void SetDefaults()
        {
            Projectile.width = 23;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 2;
            Projectile.timeLeft = 80;
            Projectile.DamageType = ModContent.GetInstance<CardDamage>();
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
        }

        public override void AI()
        {
            float offset = MathHelper.ToRadians(220f);
            Projectile.rotation = Projectile.velocity.ToRotation() + offset;

            // 🔥 Fireball dust trail
            for (int i = 0; i < 2; i++)
            {
                int d = Dust.NewDust(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.Torch,
                    Projectile.velocity.X * -0.2f,
                    Projectile.velocity.Y * -0.2f,
                    150,
                    default,
                    1.2f
                );
                Main.dust[d].noGravity = true;
            }
        }

        // 🔥 Apply On Fire! debuff
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 180); // 3 seconds
        }

        // ✨ Glowmask draw
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Texture2D glow = ModContent.Request<Texture2D>("NaturiumMod/Assets/Items/Cards/LOB/HinotamaFireball_Glow").Value;

            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            Vector2 origin = tex.Size() * 0.5f;

            // Base sprite
            Main.EntitySpriteDraw(
                tex,
                drawPos,
                null,
                lightColor,
                Projectile.rotation,
                origin,
                1f,
                SpriteEffects.None,
                0
            );

            // Glowmask
            Main.EntitySpriteDraw(
                glow,
                drawPos,
                null,
                Color.White,
                Projectile.rotation,
                origin,
                1f,
                SpriteEffects.None,
                0
            );

            return false;
        }
    }

}
