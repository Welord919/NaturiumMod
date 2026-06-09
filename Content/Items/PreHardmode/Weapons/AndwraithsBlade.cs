using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

public class AndwraithsBlade : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/PostHardmode/Weapons/AndwraithsBlade";

    public override void SetDefaults()
    {
        Item.width = 60;
        Item.height = 60;

        Item.damage = 32;
        Item.DamageType = DamageClass.Melee;
        Item.knockBack = 6.5f;
        Item.crit = 4;

        Item.useTime = 30;
        Item.useAnimation = 30;
        Item.useStyle = ItemUseStyleID.Swing;

        Item.autoReuse = true;
        Item.UseSound = SoundID.Item1;

        Item.value = Item.buyPrice(0, 0, 90, 0);
        Item.rare = ItemRarityID.Orange;

        Item.noUseGraphic = false;
        Item.noMelee = false;
    }
    public override void ModifyItemScale(Player player, ref float scale)
    {
        scale = 1.3f;
    }
    public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
    {
        // True melee shockwave (no projectile)
        int shockDamage = (int)(Item.damage * 0.5f);
        float radius = 48f;

        foreach (NPC npc in Main.npc)
        {
            if (npc.active && !npc.friendly && npc.whoAmI != target.whoAmI)
            {
                if (Vector2.Distance(npc.Center, target.Center) <= radius)
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
        for (int i = 0; i < 10; i++)
        {
            Dust d = Dust.NewDustDirect(target.position, target.width, target.height, DustID.Bone);
            d.noGravity = true;
            d.scale = 1.2f;
        }
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe.AddIngredient(ItemID.BladeofGrass);
        recipe.AddIngredient(ItemID.FieryGreatsword);
        recipe.AddIngredient(ItemID.Bone, 10);
        recipe.AddIngredient(ItemID.DemoniteBar, 15);
        recipe.AddTile(TileID.Anvils);
        recipe.Register();
    }
}
