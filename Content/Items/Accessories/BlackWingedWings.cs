namespace NaturiumMod.Content.Items.Accessories
{
    using Microsoft.Xna.Framework;
    using Terraria;
    using Terraria.DataStructures;
    using Terraria.ID;
    using Terraria.ModLoader;

    [AutoloadEquip(EquipType.Wings)]
    public class BlackWingedWings : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Accessories/Black-WingedWings";

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 28;
            Item.accessory = true;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.buyPrice(0, 4, 50, 0);
            ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(150, 6.75f, 1.75f);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var modPlayer = player.GetModPlayer<BlackWingedPlayer>();
            modPlayer.blackWingedWings = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SoulofFlight, 10)
                .AddIngredient(ItemID.SoulofNight, 8)
                .AddIngredient(ItemID.GiantHarpyFeather, 1)
                .AddIngredient(ItemID.DarkShard, 1)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }

    public class BlackWingedPlayer : ModPlayer
    {
        public bool blackWingedWings;
        private int slashCooldown = 0; // 10 seconds = 600 ticks

        public override void ResetEffects()
        {
            blackWingedWings = false;
        }

        public override void PostUpdate()
        {
            if (slashCooldown > 0)
                slashCooldown--;

            // Fire once every 10 seconds when below 50% HP
            if (blackWingedWings &&
                Player.statLife < Player.statLifeMax2 * 0.50f &&
                slashCooldown <= 0)
            {
                slashCooldown = 600; // 10 seconds

                Projectile.NewProjectile(
                    Player.GetSource_Accessory(Player.HeldItem),
                    Player.Center,
                    new Vector2(0, -6f), // upward slash projectile
                    ModContent.ProjectileType<BlackGaleSlash>(),
                    20,
                    2f,
                    Player.whoAmI
                );
            }
        }
    }

    public class BlackGaleSlash : ModProjectile
    {
        public override string Texture => "NaturiumMod/Assets/Projectiles/BlackGaleSlash";

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.penetrate = 2;
            Projectile.timeLeft = 60;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Ranged;
        }

        public override void AI()
        {
            Projectile.rotation += 0.4f;

            NPC target = null;
            float dist = 400f;

            foreach (NPC npc in Main.npc)
            {
                if (npc.CanBeChasedBy() &&
                    Vector2.Distance(npc.Center, Projectile.Center) < dist)
                {
                    dist = Vector2.Distance(npc.Center, Projectile.Center);
                    target = npc;
                }
            }

            if (target != null)
            {
                Vector2 desired = Projectile.DirectionTo(target.Center) * 10f;
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, desired, 0.08f);
            }
        }
    }
}
