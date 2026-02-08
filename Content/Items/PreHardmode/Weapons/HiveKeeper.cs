using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.PreHardmode.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.PreHardmode.Weapons;

public class HiveKeeper : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Weapons/HiveKeeper";

    public override void SetDefaults()
    {
        Item.DamageType = DamageClass.Melee;
        Item.damage = 25;                 
        Item.knockBack = 5f;            
        Item.crit = 4;                   
        Item.width = 33;
        Item.height = 53;

        Item.useTime = 28;                
        Item.useAnimation = 28;
        Item.useStyle = ItemUseStyleID.Swing;

        Item.autoReuse = true;
        Item.UseSound = SoundID.Item1;

        Item.value = Item.buyPrice(0, 1, 0, 0);
        Item.rare = ItemRarityID.Orange;

        Item.shoot = ProjectileID.Beenade;
        Item.shootSpeed = 10f;

        Item.noMelee = false;            
        Item.noUseGraphic = false;      
    }

    public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (Main.rand.NextFloat() < 0.25f)
        {
            target.AddBuff(BuffID.Confused, 120);
        }

    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe = RecipeHelper.GetNewRecipe(recipe, [
            new(ItemID.BeeKeeper, 1),                
            new(ItemID.Hive, 20),
            new(ItemID.HoneyBlock, 25),
            new(ModContent.ItemType<NaturiumBar>(), 8)
        ], TileID.Anvils);
        recipe.Register();
    }
}
