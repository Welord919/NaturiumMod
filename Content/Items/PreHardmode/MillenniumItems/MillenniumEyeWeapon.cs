using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NaturiumMod.Content.Items.Cards.LOB;
using NaturiumMod.Content.Items.Cards.LOB.Commons;
using NaturiumMod.Content.Items.Cards.NPCDrop;
using NaturiumMod.Content.Items.Weapons.Melee;
using NaturiumMod.Content.NPCs.SpiritReaper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.PreHardmode.MillenniumItems
{
    public class MillenniumEyeWeapon : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Millennium/MillenniumEye";

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.noMelee = true;
            Item.channel = true;

            Item.mana = 2;
            Item.damage = 18;
            Item.knockBack = 2f;
            Item.DamageType = DamageClass.Magic;

            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.buyPrice(gold: 5);

            Item.shoot = ModContent.ProjectileType<MillenniumEyeHoldout>();
            Item.shootSpeed = 0f;
        }

        public override bool CanUseItem(Player player)
        {
            // Just channel the holdout
            return true;
        }

    }

    public class MillenniumEyeHoldout : ModProjectile
    {
        public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Millennium/MillenniumEye";

        private int coneCooldown;

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 60; // keep alive while channeling
            Projectile.DamageType = DamageClass.Magic;
        }
        private int manaTimer = 0;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (!player.channel || player.dead || !player.active)
            {
                Projectile.Kill();
                return;
            }

            // Keep alive
            Projectile.timeLeft = 60;

            // Stick to player and face mouse
            Vector2 toMouse = Main.MouseWorld - player.Center;
            if (toMouse.LengthSquared() < 0.001f)
                toMouse = Vector2.UnitX;

            toMouse.Normalize();
            Projectile.Center = player.Center + toMouse * 24f;
            Projectile.rotation = toMouse.ToRotation();

            player.direction = toMouse.X > 0 ? 1 : -1;
            player.heldProj = Projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;

            Lighting.AddLight(Projectile.Center, 0.6f, 0.5f, 0.9f);

            // -----------------------------
            // ⭐ MANA DRAIN (every 20 ticks)
            // -----------------------------
            manaTimer++;
            if (manaTimer >= 20)
            {
                manaTimer = 0;

                int manaCost = 2;

                if (player.statMana >= manaCost)
                {
                    player.statMana -= manaCost;
                    player.manaRegenDelay = 60; // prevent regen while channeling
                }
                else
                {
                    Projectile.Kill();
                    return;
                }
            }

            // Cooldown logic (3 attacks per second)
            if (coneCooldown > 0)
                coneCooldown--;

            if (coneCooldown == 0)
            {
                DoConeAttack(player);
                SpawnConeDust();
                coneCooldown = 20;
            }
        }
        private void DoConeAttack(Player player)
        {
            int damage = Projectile.damage;

            Vector2 dir = Projectile.rotation.ToRotationVector2();
            float maxDistance = 400f;
            float minDot = 0.75f;

            CardDropPlayer cardPlayer = player.GetModPlayer<CardDropPlayer>();

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (!npc.active || npc.friendly || npc.dontTakeDamage || npc.life <= 0)
                    continue;

                Vector2 toNPC = npc.Center - Projectile.Center;
                float dist = toNPC.Length();
                if (dist > maxDistance)
                    continue;

                Vector2 toNPCNorm = toNPC / dist;
                float dot = Vector2.Dot(toNPCNorm, dir);
                if (dot < minDot)
                    continue;

                int hitDir = toNPC.X > 0 ? 1 : -1;
                npc.SimpleStrikeNPC(damage, hitDir, false);

                // Mark NPC as hit by the Eye
                npc.GetGlobalNPC<MillenniumEyeGlobalNPC>().hitByMillenniumEye = true;


                if (CardRegistry.TryGetCardNPC(npc.type, out int cardNPCType))
                {
                    if (Main.rand.NextFloat() < 0.25f)
                    {
                        int newIndex = NPC.NewNPC(
                            npc.GetSource_FromAI(),
                            (int)npc.position.X,
                            (int)npc.position.Y,
                            cardNPCType
                        );

                        NPC newNPC = Main.npc[newIndex];
                        newNPC.life = npc.life;
                        newNPC.lifeMax = npc.lifeMax;
                        newNPC.direction = npc.direction;

                        npc.active = false;
                    }
                }
                else
                {
                    cardPlayer.CardDropBoost += 0.05f;
                }
            }
        }

        private void SpawnConeDust()
        {
            float coneLength = 400f;
            float coneAngle = MathHelper.ToRadians(40f);

            Vector2 origin = Projectile.Center;
            Vector2 dir = Projectile.rotation.ToRotationVector2();

            Vector2 leftEdge = dir.RotatedBy(-coneAngle) * coneLength;
            Vector2 rightEdge = dir.RotatedBy(coneAngle) * coneLength;

            // Dust along edges
            for (int i = 0; i < 8; i++)
            {
                float t = i / 8f;

                Vector2 leftPos = origin + leftEdge * t;
                Vector2 rightPos = origin + rightEdge * t;

                Dust.NewDustPerfect(leftPos, DustID.PurpleTorch, dir * 2f, 150, Color.Yellow, 1.2f);
                Dust.NewDustPerfect(rightPos, DustID.PurpleTorch, dir * 2f, 150, Color.Yellow, 1.2f);
            }
        }
    }
    public static class CardRegistry
    {
        private static readonly Dictionary<int, int> cardMap = new();

        public static void Unload()
        {
            cardMap.Clear();
        }

        public static bool TryGetCardNPC(int npcType, out int cardNPCType)
        {
            return cardMap.TryGetValue(npcType, out cardNPCType);
        }
    }
    public class CardDropPlayer : ModPlayer
    {
        public float CardDropBoost;

        public override void ResetEffects()
        {
            CardDropBoost = 0f;
        }
    }
    public class MillenniumEyeGlobalNPC : GlobalNPC
    {
        public bool hitByMillenniumEye;

        public override bool InstancePerEntity => true;

        public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            if (projectile.type == ModContent.ProjectileType<MillenniumEyeHoldout>())
            {
                hitByMillenniumEye = true;
            }
        }
    }
    public class CardDropGlobalNPC : GlobalNPC
    {
        public override void OnKill(NPC npc)
        {
            Player player = Main.player[npc.lastInteraction];

            if (npc.GetGlobalNPC<MillenniumEyeGlobalNPC>().hitByMillenniumEye)
            {
                //Skull Servant Card
                if (npc.GetGlobalNPC<MillenniumEyeGlobalNPC>().hitByMillenniumEye)
                {
                    if (npc.type == NPCID.Skeleton ||
                        npc.type == NPCID.HeadacheSkeleton ||
                        npc.type == NPCID.UndeadMiner ||
                        npc.type == NPCID.SkeletonArcher ||
                        npc.type == NPCID.AngryBones ||
                        npc.type == NPCID.AngryBonesBig ||
                        npc.type == NPCID.AngryBonesBigHelmet ||
                        npc.type == NPCID.AngryBonesBigMuscle ||
                        npc.type == NPCID.DungeonSlime ||
                        npc.type == NPCID.CursedSkull ||
                        npc.type == NPCID.DarkCaster ||
                        npc.type == NPCID.ArmoredSkeleton)
                    {
                        if (Main.rand.NextFloat() < 0.2f)
                        {
                            Item.NewItem(npc.GetSource_Loot(), npc.getRect(), ModContent.ItemType<SkullServant>());
                        }
                    }
                }
                //Plaguespreader Card
                if (npc.type == ModContent.NPCType<Plaguespreader>())
                {
                    if (Main.rand.NextFloat() < 0.25f)
                    {
                        Item.NewItem(npc.GetSource_Loot(), npc.getRect(), ModContent.ItemType<PlaguespreaderCard>());
                    }
                }
                //Spirit Reaper Card
                if (npc.type == ModContent.NPCType<SpiritReaper>())
                {
                    if (Main.rand.NextFloat() < 0.25f)
                    {
                        Item.NewItem(npc.GetSource_Loot(), npc.getRect(), ModContent.ItemType<SpiritReaperCard>());
                    }
                }
                var cardPlayer = player.GetModPlayer<CardDropPlayer>();
                float baseChance = 0.04f;
                float finalChance = baseChance + cardPlayer.CardDropBoost;

                if (Main.rand.NextFloat() < finalChance)
                {
                    Item.NewItem(npc.GetSource_Loot(), npc.getRect(), ModContent.ItemType<PackLOB_Common>());
                }

            }
        }
    }

}
