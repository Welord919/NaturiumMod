using Microsoft.Xna.Framework;
using NaturiumMod.Content.Items.PreHardmode.ApophisItems;
using NaturiumMod.Content.Items.PreHardmode.Cards;
using NaturiumMod.Content.Items.PreHardmode.Cards.SuperRares;
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

namespace NaturiumMod.Content.NPCs
{
    [AutoloadHead]
    public class MysteriousGrandpa : ModNPC
    {
        private bool questCompleted = false;
        private bool questAccepted = false;
        public override string Texture => "NaturiumMod/Assets/NPCs/MysteriousGrandpa";
        public const string ShopName = "Shop";
        public int NumberOfTimesTalkedTo = 0;

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

        public override bool CanTownNPCSpawn(int numTownNPCs)
        {
            return true; // Always spawn for testing
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                //BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Town,
                new FlavorTextBestiaryInfoElement("A mysterious traveler who sells strange cards.")
            });
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = "Shop";

            if (!questCompleted)
                button2 = questAccepted ? "Turn In" : "Quest";
            else
                button2 = "";
        }


        public override void OnChatButtonClicked(bool firstButton, ref string shop)
        {
            if (firstButton)
            {
                shop = ShopName;
                return;
            }

            // SECOND BUTTON (QUEST)
            if (!questAccepted)
            {
                questAccepted = true;
                Main.npcChatText = "Bring me 3 Celtic Guardians, and I shall reward you.";
                return;
            }

            // TURN-IN LOGIC
            if (questAccepted && !questCompleted)
            {
                Player player = Main.LocalPlayer;

                int count = 0;

                // Count Celtic Guardians
                foreach (var item in player.inventory)
                {
                    if (item.type == ModContent.ItemType<CelticGuardian>())
                        count += item.stack;
                }

                if (count < 3)
                {
                    Main.npcChatText = "You don't have enough Celtic Guardians yet.";
                    return;
                }

                // Remove 3 Celtic Guardians
                int remaining = 3;
                for (int i = 0; i < player.inventory.Length && remaining > 0; i++)
                {
                    if (player.inventory[i].type == ModContent.ItemType<CelticGuardian>())
                    {
                        int take = System.Math.Min(player.inventory[i].stack, remaining);
                        player.inventory[i].stack -= take;
                        remaining -= take;

                        if (player.inventory[i].stack <= 0)
                            player.inventory[i].TurnToAir();
                    }
                }

                // Reward
                SoundEngine.PlaySound(SoundID.Item37);
                player.QuickSpawnItem(NPC.GetSource_GiftOrReward(), ModContent.ItemType<PackLOB>());

                questCompleted = true;
                Main.npcChatText = "Well done, child. Here is your reward.";
            }
        }
        public override void SaveData(TagCompound tag)
        {
            tag["talks"] = NumberOfTimesTalkedTo;
            tag["questAccepted"] = questAccepted;
            tag["questCompleted"] = questCompleted;
        }

        public override void LoadData(TagCompound tag)
        {
            NumberOfTimesTalkedTo = tag.GetInt("talks");
            questAccepted = tag.GetBool("questAccepted");
            questCompleted = tag.GetBool("questCompleted");
        }
        public override string GetChat()
        {
            WeightedRandom<string> chat = new WeightedRandom<string>();

            chat.Add("Your heart contains a treasure that no money can buy.");
            chat.Add("Ever opened a booster pack?");
            chat.Add("Games are my life!");
            chat.Add("I travel far to find rare cards.");
            chat.Add("Some cards are rarer than others.", 5.0);
            chat.Add("Have you ever seen Apdnarg Otum?", 0.1);

            NumberOfTimesTalkedTo++;
            if (NumberOfTimesTalkedTo >= 10)
            {
                chat.Add("You like pulling packs don't you");
            }

            if (!questAccepted)
                return "I sense destiny in your future... care to hear a request?";

            if (questAccepted && !questCompleted)
                return "Bring me 3 Celtic Guardians. They hold ancient power.";

            if (questCompleted)
                return "You have done well. The spirits smile upon you.";

            return "Hello there.";
        }


        public override void AddShops()
        {
            var npcShop = new NPCShop(Type, ShopName)
                .Add<PackLOB>()
                .Add(ItemID.AcornAxe);

            npcShop.Register();
        }

        public override void ModifyActiveShop(string shopName, Item[] items)
        {
            // No shimmer logic — keep simple
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

        public override List<string> SetNPCNameList()
        {
            return new List<string>()
            {
                "Mysterious",
                "Grandpa",
                "Card Collector"
            };
        }

    }
}