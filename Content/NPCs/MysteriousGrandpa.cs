using Microsoft.Xna.Framework;
using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.Cards;
using NaturiumMod.Content.Items.Cards.Fusion;
using NaturiumMod.Content.Items.Cards.LOB;
using NaturiumMod.Content.Items.Cards.LOB.ShortPrint;
using NaturiumMod.Content.Items.Cards.LOB.SuperRares;
using NaturiumMod.Content.Items.Cards.LOB.UltraRares;
using NaturiumMod.Content.Items.Cards.NPCDrop;
using NaturiumMod.Content.Items.Cards.PSA;
using NaturiumMod.Content.Items.PreHardmode.ApophisItems;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;
using static NaturiumMod.Content.NPCs.MGHelpers;
using static NaturiumMod.Content.NPCs.MGQuests;

namespace NaturiumMod.Content.NPCs
{
    [AutoloadHead]
    public class MysteriousGrandpa : ModNPC
    {
        public override string Texture => "NaturiumMod/Assets/NPCs/MysteriousGrandpa";
        public const string ShopName = "Shop";

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 25;

            NPCID.Sets.ExtraFramesCount[Type] = 9;
            NPCID.Sets.AttackFrameCount[Type] = 4;
            NPCID.Sets.DangerDetectRange[Type] = 700;
            NPCID.Sets.AttackType[Type] = 0;
            NPCID.Sets.AttackTime[Type] = 90;
            NPCID.Sets.AttackAverageChance[Type] = 30;
            NPCID.Sets.HatOffsetY[Type] = 4;
            NPCID.Sets.ShimmerTownTransform[Type] = true;

            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new()
            {
                Velocity = 1f,
                Direction = 1
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
        }

        public override void SetDefaults()
        {
            NPC.townNPC = true;
            NPC.friendly = true;
            NPC.width = 18;
            NPC.height = 40;
            NPC.aiStyle = NPCAIStyleID.Passive;
            NPC.damage = 10;
            NPC.defense = 15;
            NPC.lifeMax = 250;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0.5f;

            AnimationType = NPCID.Guide;
        }

        public override bool CanTownNPCSpawn(int numTownNPCs) => true;

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = "Shop";
            button2 = "Quest";
        }

        public override void OnChatButtonClicked(bool firstButton, ref string shop)
        {
            Player player = Main.LocalPlayer;

            // If first button is the SHOP button
            if (firstButton)
            {
                shop = ShopName;
                return;
            }

            // Starting the quest
            if (CardQuestWorld.questStage == 0)
            {
                CardQuestWorld.questStage = 1;
                Main.npcChatText = MGQuests.GetChat();
                return;
            }

            // Attempt to complete the quest
            if (TryCompleteQuest(player))
            {
                // SUCCESS — show the next quest text
                Main.npcChatText = MGQuests.GetChat();
                return; // <-- THIS WAS MISSING
            }

            // FAILURE — show failure text
            Main.npcChatText = "You haven't met the conditions yet.";
        }

        public class QuestResetToken : ModItem
        {
            public override string Texture => "NaturiumMod/Assets/Items/Cards/QuestResetToken";
            public override void SetDefaults()
            {
                Item.width = 20;
                Item.height = 20;
                Item.maxStack = 99;
                Item.useStyle = ItemUseStyleID.HoldUp;
                Item.useTime = 20;
                Item.useAnimation = 20;
                Item.consumable = true;
                Item.rare = ItemRarityID.Blue;
                Item.value = Item.buyPrice(silver: 50);
            }

            public override bool? UseItem(Player player)
            {
                // Reset quest stage
                CardQuestWorld.questStage = 0;

                // Feedback
                if (Main.netMode != NetmodeID.Server)
                    Main.NewText("Grandpa's questline has been reset.", 200, 200, 255);

                SoundEngine.PlaySound(SoundID.MenuOpen, player.Center);

                // Sync for multiplayer
                if (Main.netMode == NetmodeID.MultiplayerClient)
                    NetMessage.SendData(MessageID.WorldData);

                return true;
            }
        }
        // ============================================================
        // QUEST LOGIC (REPLACED WITH REGISTRY)
        // ============================================================

        public override string GetChat()
        {
            return MGQuests.GetChat();
        }

        private bool TryCompleteQuest(Player player)
        {
            return MGQuests.TryComplete(player);
        }

        public override void AddShops()
        {
            NPCShop shop = new NPCShop(Type);

            // Always available
            shop.Add(ModContent.ItemType<PackLOB_Common>());

            // Dynamic unlocks based on questStage
            shop.Add(
                ModContent.ItemType<EssenceExtractor>(),
                new Condition("Naturium.Quest5", () => CardQuestWorld.questStage >= 5)
            );

            shop.Add(
                ModContent.ItemType<Polymerization>(),
                new Condition("Naturium.Quest7", () => CardQuestWorld.questStage >= 7)
            );

            shop.Add(
                ModContent.ItemType<PSACase>(),
                new Condition("Naturium.Quest8", () => CardQuestWorld.questStage >= 8)
            );

            shop.Add(
                ModContent.ItemType<PackLOB_Rare>(),
                new Condition("Naturium.Quest10", () => CardQuestWorld.questStage >= 10)
            );

            shop.Add(
                ModContent.ItemType<QuestResetToken>(),
                new Condition("Naturium.Quest12", () => CardQuestWorld.questStage >= 12)
            );

            shop.Register();
        }

        public class KillTracker : ModPlayer
        {
            private void TryGiveKillReward()
            {
                if (CardQuestWorld.totalCardDamageKills % 100 == 0)
                {
                    if (Main.netMode != NetmodeID.Server)
                        Main.NewText("Bonus reward!", 150, 255, 150);

                    RewardPack(Player);
                }
            }

            public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
            {
                if (proj.DamageType == ModContent.GetInstance<CardDamage>() &&
                    !target.friendly &&
                    target.lifeMax > 5 &&
                    target.life - damageDone <= 0)
                {
                    CardQuestWorld.totalCardDamageKills++;
                    TryReduceSickness(Player, proj);

                    if (CardQuestWorld.totalCardDamageKills % 50 == 0)
                    {
                        if (Main.netMode != NetmodeID.Server)
                            Main.NewText($"Milestone reached: {CardQuestWorld.totalCardDamageKills} enemies slain with CardDamage!", 255, 200, 50);
                    }

                    TryGiveKillReward();
                }
            }

            public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
            {
                if (item.DamageType == ModContent.GetInstance<CardDamage>() &&
                    !target.friendly &&
                    target.lifeMax > 5 &&
                    target.life - damageDone <= 0)
                {
                    CardQuestWorld.totalCardDamageKills++;
                    TryReduceSickness(Player, null);

                    if (CardQuestWorld.totalCardDamageKills % 50 == 0)
                    {
                        if (Main.netMode != NetmodeID.Server)
                            Main.NewText($"Milestone reached: {CardQuestWorld.totalCardDamageKills} enemies slain with CardDamage!", 255, 200, 50);
                    }

                    TryGiveKillReward();
                }
            }
            //For REBD SS reducing
            private void TryReduceSickness(Player player, Projectile proj)
            {
                bool isREBD = proj != null && proj.GetGlobalProjectile<REBDTag>().isREBD;
                bool hasCharm = player.GetModPlayer<REBDRingPlayer>().rebdRingActive;

                if (!isREBD && !hasCharm)
                    return;

                int buffID = ModContent.BuffType<SummoningSickness>();
                int index = player.FindBuffIndex(buffID);

                if (index == -1)
                    return;

                int current = player.buffTime[index];
                int reduced = (int)(current * 0.90f);

                player.buffTime[index] = Math.Max(1, reduced);
            }

        }
        // ============================================================
        // COMBAT
        // ============================================================

        

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PackLOB_Rare>(), 1));
        }

        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = 20;
            knockback = 4f;
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = 30;
            randExtraCooldown = 30;
        }

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            projType = ModContent.ProjectileType<ApophisProj>();
            attackDelay = 1;
        }

        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 12f;
            randomOffset = 2f;
        }

        public override List<string> SetNPCNameList() =>
        new()
        {
        "Soloman",
        "Hiriluk",
        "Hakuro",
        "Roshi",
        "Edward",
        "Rayleigh",
        "Sorahiko"
        };

    }

    // ============================================================
    // WORLD QUEST SYSTEM
    // ============================================================

    public class CardQuestWorld : ModSystem
    {
        public static int questStage = 0;
        public static bool hasPSA10 = false;
        public static int totalCardDamageKills = 0;

        public override void SaveWorldData(TagCompound tag)
        {
            tag["questStage"] = questStage;
            tag["totalCardDamageKills"] = totalCardDamageKills;
        }

        public override void LoadWorldData(TagCompound tag)
        {
            questStage = tag.GetInt("questStage");
            totalCardDamageKills = tag.GetInt("totalCardDamageKills");
        }
    }

}