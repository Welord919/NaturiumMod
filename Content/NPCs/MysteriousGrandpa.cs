using Microsoft.Xna.Framework;
using NaturiumMod.Content.Items.PreHardmode.ApophisItems;
using NaturiumMod.Content.Helpers;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;
using NaturiumMod.Content.Items.Cards.LOB;
using NaturiumMod.Content.Items.Cards.LOB.Commons;
using NaturiumMod.Content.Items.Cards.LOB.UltraRares;

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
                Main.npcChatText = "Bring me 3 Celtic Guardians, and I shall reward you.";
                return;
            }

            Main.npcChatText = "You haven't met the conditions yet.";
        }

        public override string GetChat()
        {
            return CardQuestWorld.questStage switch
            {
                0 => "I sense destiny in your future... care to hear a request?",
                1 => "Bring me 3 Celtic Guardians. They hold ancient power.",
                2 => "Bring me a Super Rare card. Show me your luck.",
                3 => "Bring me 5 Warrior cards. Strength comes in numbers.",
                4 => "Bring me a Warrior, a Spellcaster, and a Dragon card.",
                5 => "Bring me 3 Dragon cards. The dragons stir.",
                6 => "Defeat 50 enemies using only card attacks.",
                7 => "Bring me 3 Blue-Eyes White Dragons.",
                _ => "You have done well. The spirits smile upon you."
            };
        }

        // ============================================================
        // QUEST LOGIC
        // ============================================================

        private bool TryCompleteQuest(Player player)
        {
            switch (CardQuestWorld.questStage)
            {
                // QUEST 1 — Bring 3 Celtic Guardians
                case 1:
                    if (CountItem(player, ModContent.ItemType<CelticGuardian>()) >= 3)
                    {
                        ConsumeItem(player, ModContent.ItemType<CelticGuardian>(), 3);
                        RewardPack(player);
                        CardQuestWorld.questStage = 2;
                        Main.npcChatText = "Well done. Now bring me a Super Rare card.";
                        return true;
                    }
                    break;

                // QUEST 3 — Bring 1 Super Rare card
                case 2:
                    if (HasAnySuperRareCard(player))
                    {
                        ConsumeFirstSuperRareCard(player);
                        RewardPack(player);
                        CardQuestWorld.questStage = 3;
                        Main.npcChatText = "A Super Rare? Impressive. Now bring me 5 Warrior cards.";
                        return true;
                    }
                    break;

                // QUEST 4 — Bring 5 Warrior cards
                case 3:
                    if (CountTaggedCards(player, "Warrior") >= 5)
                    {
                        ConsumeTaggedCards(player, "Warrior", 5);
                        RewardPack(player);
                        CardQuestWorld.questStage = 4;
                        Main.npcChatText = "Very Brave. Now bring a Warrior, Spellcaster, and a Dragon.";
                        return true;
                    }
                    break;

                // QUEST 6 — Warrior + Spellcaster + Dragon
                case 4:
                    if (HasTaggedCard(player, "Warrior") &&
                        HasTaggedCard(player, "Spellcaster") &&
                        HasTaggedCard(player, "Dragon"))
                    {
                        ConsumeTaggedCards(player, "Warrior", 1);
                        ConsumeTaggedCards(player, "Spellcaster", 1);
                        ConsumeTaggedCards(player, "Dragon", 1);
                        RewardPack(player);
                        CardQuestWorld.questStage = 5;
                        Main.npcChatText = "A balanced deck. Now bring me 3 Dragons.";
                        return true;
                    }
                    break;

                // QUEST 7 — Bring 3 Dragon cards
                case 5:
                    if (CountTaggedCards(player, "Dragon") >= 3)
                    {
                        ConsumeTaggedCards(player, "Dragon", 3);
                        RewardPack(player);
                        CardQuestWorld.questStage = 6;
                        CardQuestWorld.cardKillsWithCardDamage = 0;
                        Main.npcChatText = "The dragons awaken. Now defeat 50 enemies with card attacks.";
                        return true;
                    }
                    break;

                // QUEST 8 — Kill 50 enemies with CardDamage
                case 6:
                    if (CardQuestWorld.cardKillsWithCardDamage >= 50)
                    {
                        RewardPack(player);
                        CardQuestWorld.questStage = 7;
                        Main.npcChatText = "Impressive. Now bring me 3 Blue-Eyes White Dragons.";
                        return true;
                    }
                    break;

                // QUEST 10 — Bring 3 BEWD
                case 7:
                    if (CountItem(player, ModContent.ItemType<BEWD>()) >= 3)
                    {
                        ConsumeItem(player, ModContent.ItemType<BEWD>(), 3);
                        RewardPack(player);
                        CardQuestWorld.questStage = 8;
                        Main.npcChatText = "Three Blue-Eyes... now THAT is power.";
                        return true;
                    }
                    break;
            }

            return false;
        }

        // ============================================================
        // HELPERS
        // ============================================================

        private void RewardPack(Player player)
        {
            player.QuickSpawnItem(NPC.GetSource_GiftOrReward(), ModContent.ItemType<PackLOB>());
            SoundEngine.PlaySound(SoundID.Item37);
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
                if (item.rare == ItemRarityID.LightRed)
                    return true;
            return false;
        }

        private void ConsumeFirstSuperRareCard(Player player)
        {
            for (int i = 0; i < player.inventory.Length; i++)
            {
                if (player.inventory[i].rare == ItemRarityID.LightRed)
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

        private bool HasTaggedCard(Player player, string tag)
        {
            return CountTaggedCards(player, tag) > 0;
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

        // ============================================================
        // SHOP + COMBAT
        // ============================================================

        public override void AddShops()
        {
            var npcShop = new NPCShop(Type, ShopName)
                .Add<PackLOB>()
                .Add(ItemID.AcornAxe);

            npcShop.Register();
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PackLOB>(), 1));
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
            new() { "Mysterious", "Grandpa", "Card Collector" };
    }

    // ============================================================
    // WORLD QUEST SYSTEM (LIKE ANGLER)
    // ============================================================

    public class CardQuestWorld : ModSystem
    {
        public static int questStage = 0;
        public static int cardKillsWithCardDamage = 0;

        public override void OnWorldLoad()
        {
            questStage = 0;
            cardKillsWithCardDamage = 0;
        }

        public override void OnWorldUnload()
        {
            questStage = 0;
            cardKillsWithCardDamage = 0;
        }

        public override void SaveWorldData(TagCompound tag)
        {
            tag["questStage"] = questStage;
            tag["cardKills"] = cardKillsWithCardDamage;
        }

        public override void LoadWorldData(TagCompound tag)
        {
            questStage = tag.GetInt("questStage");
            cardKillsWithCardDamage = tag.GetInt("cardKills");
        }
    }

    // ============================================================
    // GLOBAL KILL TRACKER
    // ============================================================

    
}