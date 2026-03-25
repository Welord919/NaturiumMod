using Microsoft.Xna.Framework;
using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.Cards;
using NaturiumMod.Content.Items.Cards.Fusion;
using NaturiumMod.Content.Items.Cards.LOB;
using NaturiumMod.Content.Items.Cards.LOB.ShortPrint;
using NaturiumMod.Content.Items.Cards.LOB.SuperRares;
using NaturiumMod.Content.Items.Cards.LOB.UltraRares;
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

            if (firstButton)
            {
                shop = ShopName;
                return;
            }

            // QUEST BUTTON
            if (TryCompleteQuest(player))
                return;

            // Start Quest 1 if none active
            if (CardQuestWorld.questStage == 0)
            {
                CardQuestWorld.questStage = 1;
                Main.npcChatText = "Bring me 5 Warrior Cards. Easy!.";
                return;
            }

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

        public override string GetChat()
        {
            return CardQuestWorld.questStage switch
            {
                0 => "I sense destiny in your future... care to hear a request?",
                1 => "Bring me 5 Warrior Cards. Easy!",
                2 => "Bring me a Super Rare card. Show me your luck.",
                3 => "Bring me 15 Fire cards. Surely you can do this.",
                4 => "Now that you have the Fusion Altar, you must craft Fusion Extractors, you can use Naturium to make them. They extract essence from the cards while on the table. With essences and Polymerization, you can craft powerful Fusion cards. Craft a Flame Swordsman.",
                5 => "Bring me 10 Dragon cards. A bit rarer than either Warriors or Fires.",
                6 => "Defeat 50 enemies using only card attacks.",
                7 => "Bring me 3 Blue-Eyes White Dragons.",
                8 => "You have done well. Now bring me a Darkfire Dragon..",
                9 => "Defeat 200 enemies with card attacks.",
                10 => "You have completed all my tasks for now. Will you begin anew?",
                11 => "You have completed all my tasks for now. Will you begin anew?"
            };
        }

        // ============================================================
        // QUEST LOGIC
        // ============================================================

        private bool TryCompleteQuest(Player player)
        {
            switch (CardQuestWorld.questStage)
            {
                // QUEST 1 — Bring 5 Warrior Cards
                case 1:
                    if (CountTaggedCards(player, "Warrior") >= 5)
                    {
                        ConsumeTaggedCards(player, "Warrior", 5);
                        RewardPack(player);
                        player.QuickSpawnItem(player.GetSource_GiftOrReward(), ModContent.ItemType<FusionAltar>());
                        CardQuestWorld.questStage = 2;
                        Main.npcChatText = "Now pull a Super Rare Card";
                        return true;
                    }
                    break;

                // QUEST 2 — Bring 1 Super Rare card
                case 2:
                    if (HasAnySuperRareCard(player))
                    {
                        ConsumeFirstSuperRareCard(player);
                        RewardPack(player);
                        CardQuestWorld.questStage = 3;
                        Main.npcChatText = "A Super Rare? Impressive. Now bring me 15 Fire cards.";
                        return true;
                    }
                    break;

                // QUEST 3 — Bring 15 Fire cards
                case 3:
                    if (CountTaggedCards(player, "Fire") >= 15)
                    {
                        ConsumeTaggedCards(player, "Fire", 15);
                        for (int i = 0; i < 2; i++)
                            RewardPack(player);
                        player.QuickSpawnItem(player.GetSource_GiftOrReward(), ModContent.ItemType<FusionAltar>());
                        CardQuestWorld.questStage = 4;
                        Main.npcChatText = "Ho ho, for that I will give you a Fusion Altar. Place it down and come back to me.";
                        return true;
                    }
                    break;

                // QUEST 4 — Craft a Flame Swordsman
                case 4:
                    if (player.HasItem(ModContent.ItemType<FlameSwordsman>()))
                    {
                        for (int i = 0; i < 2; i++)
                            RewardPack(player);

                        CardQuestWorld.questStage = 5;
                        Main.npcChatText = "Excellent work crafting a Flame Swordsman. Now bring me 10 Dragon cards.";
                        return true;
                    }
                    break;

                // QUEST 5 — Bring 10 Dragon cards
                case 5:
                    if (CountTaggedCards(player, "Dragon") >= 10)
                    {
                        ConsumeTaggedCards(player, "Dragon", 10);
                        for (int i = 0; i < 2; i++)
                            RewardPack(player);
                        CardQuestWorld.questStage = 6;
                        Main.npcChatText = "Now defeat 50 enemies with Card attacks.";
                        return true;
                    }
                    break;

                // QUEST 6 — Kill 50 enemies with CardDamage
                case 6:
                    if (CardQuestWorld.totalCardDamageKills >= 50)
                    {
                        for (int i = 0; i < 2; i++)
                            RewardPack(player);
                        CardQuestWorld.questStage = 7;
                        Main.npcChatText = "Impressive. Now bring me 3 Blue-Eyes White Dragons.";
                        return true;
                    }
                    break;

                // QUEST 7 — Bring 3 BEWD
                case 7:
                    if (CountItem(player, ModContent.ItemType<BEWD>()) >= 3)
                    {
                        ConsumeItem(player, ModContent.ItemType<BEWD>(), 3);
                        for (int i = 0; i < 3; i++)
                            RewardPack(player);
                        CardQuestWorld.questStage = 8;
                        Main.npcChatText = "Three Blue-Eyes... I remember a unfriendly young man who liked those. Now bring me a Darkfire Dragon.";
                        return true;
                    }
                    break;
                // QUEST 8 — Bring a Darkfire Dragon
                case 8:
                    if (player.HasItem(ModContent.ItemType<DarkfireDragon>()))
                    {
                        for (int i = 0; i < 3; i++)
                            RewardPack(player);
                        CardQuestWorld.questStage = 9;
                        Main.npcChatText = "A Darkfire Dragon? Magnificent. Now show your strength by killing 200 enemies.";
                        return true;
                    }
                    break;


                // QUEST 9 — Kill 200 enemies with CardDamage
                case 9:
                    if (CardQuestWorld.totalCardDamageKills >= 200)
                    {
                        for (int i = 0; i < 4; i++)
                            RewardPack(player);
                        CardQuestWorld.questStage = 10;
                        Main.npcChatText = "Your strength is undeniable.";
                        return true;
                    }
                    break;

                
                // QUEST 10 — Final quest, give QuestResetToken
                case 10:
                    // Final reward
                    player.QuickSpawnItem(
                        player.GetSource_GiftOrReward(),
                        ModContent.ItemType<QuestResetToken>());

                    for (int i = 0; i < 5; i++)
                        RewardPack(player);
                    CardQuestWorld.questStage = 11; // or loop back to 0 if you prefer

                    Main.npcChatText = "You have completed all my tasks for now. Take this token — it will allow you to begin anew when you wish.";
                    return true;
            }

            return false;
        }


        // ============================================================
        // HELPERS
        // ============================================================

        public static void RewardPack(Player player)
        {
            var weighted = new List<(int type, int weight)>
    {
        (ModContent.ItemType<PackLOB_Common>(), 45),
        (ModContent.ItemType<PackLOB_Rare>(),   25),
        (ModContent.ItemType<PackLOB_Super>(),  20),
        (ModContent.ItemType<PackLOB_Ultra>(),  10)
    };

            int totalWeight = weighted.Sum(w => w.weight);
            int roll = Main.rand.Next(totalWeight);

            foreach (var entry in weighted)
            {
                if (roll < entry.weight)
                {
                    player.QuickSpawnItem(
                        player.GetSource_GiftOrReward(),
                        entry.type
                    );

                    SoundEngine.PlaySound(SoundID.Item37);
                    return;
                }

                roll -= entry.weight;
            }
        }
        private int CountItem(Player player, int type)
        {
            int count = 0;
            foreach (var item in player.inventory)
                if (item.type == type)
                    count += item.stack;
            return count;
        }

        private void ConsumeItem(Player player, int type, int amount)
        {
            for (int i = 0; i < player.inventory.Length && amount > 0; i++)
            {
                if (player.inventory[i].type == type)
                {
                    int take = System.Math.Min(player.inventory[i].stack, amount);
                    player.inventory[i].stack -= take;
                    amount -= take;
                    if (player.inventory[i].stack <= 0)
                        player.inventory[i].TurnToAir();
                }
            }
        }

        private bool HasAnySuperRareCard(Player player)
        {
            foreach (var item in player.inventory)
                if (item.rare == ItemRarityID.Orange)
                    return true;
            return false;
        }

        private void ConsumeFirstSuperRareCard(Player player)
        {
            for (int i = 0; i < player.inventory.Length; i++)
            {
                if (player.inventory[i].rare == ItemRarityID.Orange)
                {
                    player.inventory[i].stack--;
                    if (player.inventory[i].stack <= 0)
                        player.inventory[i].TurnToAir();
                    return;
                }
            }
        }

        private int CountTaggedCards(Player player, string tag)
        {
            int count = 0;
            foreach (var item in player.inventory)
            {
                if (WeaponTag.ItemTags.TryGetValue(item.type, out var tags) &&
                    tags.Contains(tag))
                {
                    count += item.stack;
                }
            }
            return count;
        }

        private void ConsumeTaggedCards(Player player, string tag, int amount)
        {
            for (int i = 0; i < player.inventory.Length && amount > 0; i++)
            {
                Item item = player.inventory[i];
                if (WeaponTag.ItemTags.TryGetValue(item.type, out var tags) &&
                    tags.Contains(tag))
                {
                    int take = System.Math.Min(item.stack, amount);
                    item.stack -= take;
                    amount -= take;
                    if (item.stack <= 0)
                        item.TurnToAir();
                }
            }
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
        // SHOP + COMBAT
        // ============================================================

        public override void AddShops()
        {
            NPCShop shop = new NPCShop(Type);

            int stage = CardQuestWorld.questStage;

            // Always available
            shop.Add(ModContent.ItemType<PackLOB_Common>());

           
            // After Quest 4 (Flame Swordsman)
            if (stage >= 4)
                shop.Add(ModContent.ItemType<EssenceExtractor>());

            // After Quest 5 (10 Dragons)
            if (stage >= 5)
                shop.Add(ModContent.ItemType<FusionAltar>());

            // After Quest 6)
            if (stage >= 6)
                shop.Add(ModContent.ItemType<Polymerization>());

            // After Quest 8 (Darkfire Dragon)
            if (stage >= 8)
                shop.Add(ModContent.ItemType<PackLOB_Rare>());

            // After Quest 10 (final)
            if (stage >= 10)
                shop.Add(ModContent.ItemType<QuestResetToken>());

            shop.Register();
        }

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

        // ⭐ NEW: world‑level kill counter
        public static int totalCardDamageKills = 0;

        public override void OnWorldLoad()
        {
            questStage = 0;
            totalCardDamageKills = 0;
        }

        public override void OnWorldUnload()
        {
            questStage = 0;
            totalCardDamageKills = 0;
        }

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