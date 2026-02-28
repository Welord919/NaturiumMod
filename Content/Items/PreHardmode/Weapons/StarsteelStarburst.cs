using Microsoft.Xna.Framework;
using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.PreHardmode.Materials;
using NaturiumMod.Content.ModPlayers;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.PreHardmode.Weapons;

public class StarsteelStarburst : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Weapons/StarsteelStarburst";

    public override void SetDefaults()
    {
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.useAnimation = 12;
        Item.useTime = 12;
        Item.autoReuse = false;
        Item.damage = 50;
        Item.DamageType = DamageClass.Ranged;
        Item.knockBack = 6.5f;
        Item.crit = 6;
        Item.shootSpeed = 14f;
        Item.noMelee = true;
        Item.width = 44;
        Item.height = 14;
        Item.UseSound = SoundID.Item40;
        Item.value = Item.buyPrice(0, 1, 0, 0);
        Item.rare = ItemRarityID.Green;
        Item.shoot = ProjectileID.MeteorShot;
        Item.useAmmo = AmmoID.Bullet;
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        // Only convert the basic musket ball projectile (ProjectileID.Bullet == musket ball)
        if (type == ProjectileID.Bullet)
        {
            int meteorType = ProjectileID.MeteorShot;
            float spread = MathHelper.ToRadians(4f);
            Vector2 newVel = velocity.RotatedByRandom(spread);

            Projectile.NewProjectile(source, position, newVel, meteorType, damage, knockback, player.whoAmI);
            return false; // we spawned the meteor, prevent default spawn
        }

        // Otherwise spawn the original projectile so other ammo keeps its behavior
        Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
        return false;
    }



    public override bool CanUseItem(Player player)
    {
        return true; // prevent default Shoot() usage
    }

    public override void HoldItem(Player player)
    {
        var modPlayer = player.GetModPlayer<StarsteelGunPlayer>();

        // If player is holding use button
        if (player.controlUseItem)
        {
            // If currently in cooldown, do nothing (no sound, no projectile)
            if (modPlayer.starburstCooldown > 0)
                return;

            // fireTimer controls delay between individual shots in the burst
            if (modPlayer.fireTimer > 0)
                return;

            // Only the local player should initiate projectile creation to avoid double-spawn
            if (player.whoAmI == Main.myPlayer)
            {
                Vector2 muzzlePosition = player.Center + Vector2.Normalize(Main.MouseWorld - player.Center) * 20f;
                Vector2 direction = Vector2.Normalize(Main.MouseWorld - player.Center) * Item.shootSpeed;
                direction = direction.RotatedByRandom(MathHelper.ToRadians(4));

                Projectile.NewProjectile(
                    player.GetSource_ItemUse(Item),
                    muzzlePosition,
                    direction,
                    ProjectileID.StarCannonStar,
                    Item.damage,
                    Item.knockBack,
                    player.whoAmI
                );

                SoundEngine.PlaySound(Item.UseSound, player.position);
            }

            modPlayer.burstShots++;
            modPlayer.fireTimer = Item.useTime; // wait this many ticks before next shot

            if (modPlayer.burstShots >= 5)
            {
                modPlayer.starburstCooldown = 120; // 2 seconds
                modPlayer.burstShots = 0;
            }
        }
        else
        {
            // Not holding: reset the per-shot timer so next hold starts immediately
            modPlayer.fireTimer = 0;
        }
    }


    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe.AddIngredient(ModContent.ItemType<Starsteel>(), 25);
        recipe.AddIngredient(ItemID.IllegalGunParts, 1);
        recipe.AddIngredient(ItemID.StarCannon, 1);
        recipe.AddTile(TileID.Anvils);
        recipe.Register();
    }
}

