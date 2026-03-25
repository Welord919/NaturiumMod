using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.PreHardmode.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Cards.Fusion
{
    public class CharubinMace : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/Fusion/CharubinMace";

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 25;       
            Item.useTime = 25;           
            Item.knockBack = 7f;         
            Item.damage = 20;             
            Item.crit = 8;               
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.channel = true;
            Item.shoot = ModContent.ProjectileType<CharubinMaceProj>();
            Item.shootSpeed = 14f; 
            Item.rare = ItemRarityID.Green;
            Item.value = Item.buyPrice(silver: 75);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe = RecipeHelper.GetNewRecipe(recipe, [
            new(ModContent.ItemType<Charubin>(), 1),
        new(ModContent.ItemType<NaturiumBar>(), 15),
        new(ItemID.FlamingMace, 1),
        new(ModContent.ItemType<EarthEssence>(), 25)
            ], TileID.Anvils);
            recipe.Register();
        }
    }

    public class CharubinMaceProj : ModProjectile
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/Fusion/CharubinMaceProj";

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;
            Projectile.aiStyle = ProjAIStyleID.Flail; // REAL flail physics
            Projectile.tileCollide = true;
            Projectile.noDropItem = true;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // 10% chance to apply egg shell
            if (Main.rand.NextFloat() < 0.10f)
            {
                int spawnedIndex = Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    target.Center,
                    Vector2.Zero,
                    ModContent.ProjectileType<EggShellStickProj>(),
                    0, // contact damage = 0 so it won't hurt on spawn
                    Projectile.knockBack,
                    Projectile.owner
                );

                if (spawnedIndex >= 0 && spawnedIndex < Main.maxProjectiles)
                {
                    Projectile spawned = Main.projectile[spawnedIndex];

                    // Store intended explosion damage in localAI[0]
                    spawned.localAI[0] = damageDone;

                    // Store the NPC index in ai[0] so the eggshell knows which NPC to stick to immediately
                    spawned.ai[0] = target.whoAmI;

                    // Ensure multiplayer sync
                    spawned.netUpdate = true;
                }
            }
        }


        // Draw chain
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D chainTex = ModContent.Request<Texture2D>("Terraria/Images/Projectile_13").Value;

            Vector2 position = Projectile.Center;
            Vector2 mountedCenter = Main.player[Projectile.owner].MountedCenter;
            Vector2 distToProj = mountedCenter - position;
            float projRotation = distToProj.ToRotation() - MathHelper.PiOver2;
            float distance = distToProj.Length();

            while (distance > 20f)
            {
                distToProj.Normalize();
                distToProj *= 20f;
                position += distToProj;
                distToProj = mountedCenter - position;
                distance = distToProj.Length();

                Main.EntitySpriteDraw(chainTex, position - Main.screenPosition,
                    null, lightColor, projRotation, chainTex.Size() / 2f, 1f, SpriteEffects.None, 0);
            }

            return true;
        }
    }

    public class EggShellStickProj : ModProjectile
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/Fusion/EggShellStickProj";

        private NPC stuckTarget;
        private int stuckTimer = 0;

        // localAI[0] stores the intended explosion damage (set by the spawner)
        // ai[0] may contain the target NPC index when spawned directly onto an NPC

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true; // keep friendly so it doesn't hurt players
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 180; // safety
        }

        public override void AI()
        {
            // If ai[0] was set by the spawner, try to resolve the NPC and become stuck immediately
            if (stuckTarget == null && Projectile.ai[0] >= 0f)
            {
                int npcIndex = (int)Projectile.ai[0];
                if (npcIndex >= 0 && npcIndex < Main.maxNPCs)
                {
                    NPC maybe = Main.npc[npcIndex];
                    if (maybe != null && maybe.active)
                    {
                        stuckTarget = maybe;

                        // Make sure projectile is in stuck state
                        Projectile.velocity = Vector2.Zero;
                        Projectile.penetrate = -1;
                        Projectile.tileCollide = false;

                        // Clear ai[0] so we don't re-run this block
                        Projectile.ai[0] = -1f;

                        Projectile.netUpdate = true;
                    }
                }
            }

            if (stuckTarget != null && stuckTarget.active)
            {
                stuckTimer++;

                // Stick to the enemy's center
                Projectile.Center = stuckTarget.Center;
                Projectile.netUpdate = true; // keep server/client in sync

                // Spin faster over time (visual only)
                Projectile.rotation += 0.4f + stuckTimer * 0.01f;

                // No periodic contact damage while stuck (per your request)

                // After 2 seconds (120 frames), explode
                if (stuckTimer >= 120)
                {
                    Explode();
                }
            }
            else
            {
                // Normal flight spin
                Projectile.rotation += 0.3f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // If the projectile actually hits an NPC in flight, stick normally
            if (stuckTarget == null)
            {
                stuckTarget = target;
                Projectile.velocity = Vector2.Zero;
                Projectile.penetrate = -1;
                Projectile.tileCollide = false;
                Projectile.netUpdate = true;
            }
        }

        private void Explode()
        {
            // Use stored damage from localAI[0] if present, otherwise fallback to Projectile.damage
            int explosionDamage = (int)Projectile.localAI[0];
            if (explosionDamage <= 0)
                explosionDamage = Projectile.damage;

            // Spawn the explosion server-side only to avoid double-spawning in multiplayer
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    Projectile.Center,
                    Vector2.Zero,
                    ProjectileID.GrenadeIII, // or your custom explosion projectile
                    explosionDamage,
                    4f,
                    Projectile.owner
                );
            }

            Projectile.Kill();
        }
    }

}