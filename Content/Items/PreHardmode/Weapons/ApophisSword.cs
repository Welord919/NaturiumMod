using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.General.Projectiles;
using NaturiumMod.Content.Items.PreHardmode.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.PreHardmode.Weapons;

public class ApophisSwrd : ModItem
{
  
    public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Weapons/ApophisSword";

    public override void SetDefaults()
    {
        Item.width = 40;
        Item.height = 40;

        Item.damage = 23; // Enchanted Sword is 23 damage
        Item.DamageType = DamageClass.Melee;
        Item.knockBack = 4.5f;
        Item.crit = 4;

        Item.useTime = 25;
        Item.useAnimation = 25;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.useTurn = true;

        Item.autoReuse = true;
        Item.UseSound = SoundID.Item20; // magical sound

        Item.shoot = ModContent.ProjectileType<ApophisProj>();
        Item.shootSpeed = 8f;

        Item.value = Item.buyPrice(0, 0, 80, 0);
        Item.rare = ItemRarityID.Green;

        Item.noMelee = false;
        Item.noUseGraphic = false;
    }
}
