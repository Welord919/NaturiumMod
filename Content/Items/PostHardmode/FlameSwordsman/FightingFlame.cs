using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NaturiumMod.Content.Items.Cards.Fusion.FusionCards;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

public class BladeofFightingFlame : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/PostHardmode/FlameSwordsman/FlameSwordsmanBlade";

    public override void SetDefaults()
    {
        Item.DamageType = DamageClass.Melee;
        Item.damage = 50;
        Item.width = 60;
        Item.height = 120;
        Item.useTime = 33;
        Item.useAnimation = 33;
        Item.knockBack = 8f;
        Item.crit = 10;
        Item.UseSound = SoundID.Item20; // flame swing
        Item.useStyle = ItemUseStyleID.Swing;

        Item.value = Item.buyPrice(0, 25, 0, 0);
        Item.rare = ItemRarityID.Red;
        Item.autoReuse = true;

        Item.shootSpeed = 12f;
        Item.shoot = ModContent.ProjectileType<SalamandraFireProj>();
    }

    public override void HoldItem(Player player)
    {
        var fs = player.GetModPlayer<FlameSwordsmanPlayer>();
        fs.flameSwordsmanBladeEquipped = true;

        // Fire aura
        if (Main.rand.NextBool(4))
        {
            Dust d = Dust.NewDustDirect(player.Center, 4, 4, DustID.Torch);
            d.velocity *= 0.5f;
            d.scale = 1.2f;
            d.noGravity = true;
        }
    }

    public override void UseAnimation(Player player)
    {
        // Flame burst on swing
        for (int i = 0; i < 20; i++)
        {
            Dust d = Dust.NewDustDirect(player.itemLocation, 10, 10, DustID.Lava);
            d.velocity = Main.rand.NextVector2Circular(4f, 4f);
            d.scale = 1.5f;
            d.noGravity = true;
        }
    }

    public override void UseStyle(Player player, Rectangle heldItemFrame)
    {
        player.itemRotation -= MathHelper.ToRadians(20f);
    }

    public override Vector2? HoldoutOffset()
    {
        return new Vector2(-20, 0);
    }

    public override void ModifyItemScale(Player player, ref float scale)
    {
        scale = 2f;
    }

    public override void ModifyHitNPC(Player player, NPC target, ref NPC.HitModifiers modifiers)
    {
        var fs = player.GetModPlayer<FlameSwordsmanPlayer>();

        if (player.statLife < player.statLifeMax2 * 0.5f)
            modifiers.SourceDamage *= 1.20f;

        if (fs.salamandraGauntletEquipped)
            modifiers.SourceDamage *= 1.10f;
    }

    public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
    {
        var fs = player.GetModPlayer<FlameSwordsmanPlayer>();

        target.AddBuff(BuffID.OnFire3, 180);
        fs.AddFlameToken();
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe.AddIngredient(ItemID.TerraBlade);
        recipe.AddIngredient(ModContent.ItemType<FlameSwordsman>());
        recipe.AddIngredient(ItemID.HallowedBar, 15);
        recipe.AddIngredient(ItemID.SoulofFright, 10);
        recipe.AddTile(TileID.MythrilAnvil);
        recipe.Register();
    }
}
