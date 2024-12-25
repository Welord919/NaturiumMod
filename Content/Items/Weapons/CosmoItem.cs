﻿using Microsoft.Xna.Framework;
using NaturiumMod.Content.Items.Materials;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Weapons;

public class CosmoItem : ModItem
{
    public override void SetStaticDefaults()
    {
        ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller
        ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
        ItemID.Sets.StaffMinionSlotsRequired[Type] = 1f; // The default value is 1, but other values are supported. See the docs for more guidance. 
    }

    public override void SetDefaults()
    {
        Item.damage = 11;
        Item.knockBack = 2f;
        Item.mana = 10; // mana cost
        Item.width = 32;
        Item.height = 32;
        Item.useTime = 36;
        Item.useAnimation = 36;
        Item.useStyle = ItemUseStyleID.Swing; // how the player's arm moves when using the item
        Item.value = Item.sellPrice(gold: 30);
        Item.rare = ItemRarityID.Cyan;
        Item.UseSound = SoundID.Item44; // What sound should play when using the item

        // These below are needed for a minion weapon
        Item.noMelee = true; // this item doesn't do any melee damage
        Item.DamageType = DamageClass.Summon;
        Item.buffType = ModContent.BuffType<Buffs.CosmoMinionBuff>();
        Item.shoot = ModContent.ProjectileType<Projectiles.CosmoMinionProj>();
    }

    public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
    {
        // Here you can change where the minion is spawned. Most vanilla minions spawn at the cursor position
        position = Main.MouseWorld;
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        // This is needed so the buff that keeps your minion alive and allows you to despawn it properly applies
        player.AddBuff(Item.buffType, 2);

        // Minions have to be spawned manually, then have originalDamage assigned to the damage of the summon item
        var projectile = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, Main.myPlayer);
        projectile.originalDamage = Item.damage;

        // Since we spawned the projectile manually already, we do not need the game to spawn it for ourselves anymore, so return false
        return false;
    }

    // Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe = RecipeUtils.GetNewRecipe(recipe, [(ModContent.ItemType<BarkionsBark>(), 25), (ModContent.ItemType<NaturiumOre>(), 5), (ItemID.Vine, 5), (ItemID.Stinger, 3)], TileID.Anvils);
        recipe.Register();
    }
}
