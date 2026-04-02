using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.Cards.Fusion;
using NaturiumMod.Content.Items.Cards.LOB.UltraRares;
using NaturiumMod.Content.Items.PreHardmode.IceBarrier;
using NaturiumMod.Content.Items.PreHardmode.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.PreHardmode.Accessories
{
    public class IceBarrierIcon : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/IceBarrier/IceBarrierIcon";

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.accessory = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(silver: 50);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<IceDamagePlayer>().iceMedallionActive = true;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe = RecipeHelper.GetNewRecipe(recipe, [
        new(ModContent.ItemType<WaterEssence>(), 3),
        new(ModContent.ItemType<IceBarrierCore>(), 25),
        new(ModContent.ItemType<CharmBase>(), 1)
            ], TileID.TinkerersWorkbench);
            recipe.Register();
        }
    }
    public class IceDamagePlayer : ModPlayer
    {
        public bool iceMedallionActive;

        public override void ResetEffects()
        {
            iceMedallionActive = false;
        }
    }
    public class IceDamageGlobalItem : GlobalItem
    {
        public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
        {
            if (!player.GetModPlayer<IceDamagePlayer>().iceMedallionActive)
                return;

            if (IceWeaponRegistry.IceItems.Contains(item.type))
            {
                damage *= 1.05f;
            }
        }
    }
    public class IceDamageGlobalProjectile : GlobalProjectile
    {
        public override void ModifyHitNPC(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            Player player = Main.player[proj.owner];

            if (!player.GetModPlayer<IceDamagePlayer>().iceMedallionActive)
                return;

            if (IceWeaponRegistry.IceProjectiles.Contains(proj.type))
            {
                modifiers.SourceDamage *= 1.05f;
            }
        }
    }

}