using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.PreHardmode.Accessories;
using NaturiumMod.Content.Items.PreHardmode.Materials;
using StructureHelper.Content.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Cards.Fusion
{ 
    public class FusionistRing : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/Fusion/FusionistRing";
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.accessory = true;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.buyPrice(gold: 5);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var modPlayer = player.GetModPlayer<FusionistRingPlayer>();
            var boost = player.GetModPlayer<WeaponBoostPlayer>();
            boost.activeBoosts["Fusion"] = true;
            player.blockRange += 1;
            player.moveSpeed += 0.10f;
            player.runAcceleration *= 1.03f;
        }
        public override void AddRecipes()
        {
            CreateRecipe(15)
                .AddIngredient(ModContent.ItemType<Fusionist>(), 1)
                .AddIngredient(ModContent.ItemType<SunflowerPower>(), 1)
                .AddIngredient(ModContent.ItemType<NaturiumBar>(), 8)
                .AddTile(ModContent.TileType<FusionAltarTile>())
                .Register();
        }
    }
}
public class FusionistRingPlayer : ModPlayer
{   
    public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
    {
        // Only apply to CardDamage projectiles
        if (proj.DamageType == ModContent.GetInstance<CardDamage>())
        {
            modifiers.SourceDamage *= 1.05f; // +10% damage
        }
    }
    public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
    {
        // Only apply to CardDamage items
        if (item.DamageType == ModContent.GetInstance<CardDamage>())
        {
            damage *= 1.05f; // +10% damage
        }
    }
}
    
