using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

public class LeviathansBlade : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/PostHardmode/Weapons/AndwraithsBlade";

    public override void SetDefaults()
    {
        Item.width = 70;
        Item.height = 70;

        Item.damage = 78;
        Item.DamageType = DamageClass.Melee;
        Item.knockBack = 8.5f;
        Item.crit = 6;

        Item.useTime = 28;
        Item.useAnimation = 28;
        Item.useStyle = ItemUseStyleID.Swing;

        Item.autoReuse = true;
        Item.UseSound = SoundID.Item1;

        Item.value = Item.buyPrice(0, 8, 0, 0);
        Item.rare = ItemRarityID.LightPurple;

        Item.noUseGraphic = false;
        Item.noMelee = false;
    }
    public override void ModifyItemScale(Player player, ref float scale)
    {
        scale = 2f;
    }
    public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
    {
        // Bonus damage to enemies below the player
        if (target.Center.Y > player.Center.Y)
        {
            int bonus = (int)(damageDone * 0.20f);
            NPC.HitInfo bonusHit = new NPC.HitInfo()
            {
                Damage = bonus,
                Knockback = 0f,
                HitDirection = player.direction,
                Crit = false
            };

            target.StrikeNPC(bonusHit);

        }

        // Ground shockwave (true melee hitbox)
        float shockRange = 80f;
        int shockDamage = (int)(Item.damage * 0.7f);

        foreach (NPC npc in Main.npc)
        {
            if (npc.active && !npc.friendly && npc.whoAmI != target.whoAmI)
            {
                bool onGround = Math.Abs(npc.Center.Y - target.Center.Y) < 20f;
                if (onGround && Vector2.Distance(npc.Center, target.Center) <= shockRange)
                {
                    NPC.HitInfo shockhit = new NPC.HitInfo()
                    {
                        Damage = shockDamage,
                        Knockback = 0f,
                        HitDirection = player.direction,
                        Crit = false
                    };

                    npc.StrikeNPC(shockhit);
                }
            }
        }

        // Dust effect
        for (int i = 0; i < 15; i++)
        {
            Dust d = Dust.NewDustDirect(target.position, target.width, target.height, DustID.Water);
            d.noGravity = true;
            d.scale = 1.4f;
        }
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe.AddIngredient(ModContent.ItemType<AndwraithsBlade>());
        recipe.AddIngredient(ItemID.SoulofMight, 10);
        recipe.AddIngredient(ItemID.SoulofSight, 10);
        recipe.AddIngredient(ItemID.SoulofFright, 10);
        recipe.AddIngredient(ItemID.HallowedBar, 15);
        recipe.AddTile(TileID.MythrilAnvil);
        recipe.Register();
    }
}
