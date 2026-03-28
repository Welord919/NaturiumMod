using NaturiumMod.Content.Items.Cards.Fusion;
using NaturiumMod.Content.Items.Cards.LOB.UltraRares;
using NaturiumMod.Content.Items.Cards.NPCDrop;
using NaturiumMod.Content.Items.Cards.PSA;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static NaturiumMod.Content.NPCs.MysteriousGrandpa;

namespace NaturiumMod.Content.NPCs;

public static class MGQuests
{
    public static string GetChat()
    {
        return CardQuestWorld.questStage switch
        {
            0 => "I sense destiny in your future... care to hear a request?",
            1 => "Bring me 5 Warrior Cards. Easy!",
            2 => "Bring me a Super Rare card. Show me your luck.",
            3 => "Bring me 15 Fire cards. Surely you can do this.",
            4 => "I have game you the Fusion Altar. Place it down and craft a Flame Swordsman. You need Essence Extractors for this and are crafted with Natuirum",
            5 => "Bring me 10 Dragon cards. A bit rarer than either Warriors or Fires.",
            6 => "Defeat 50 enemies using only card attacks.",
            7 => "Bring me a Plaguespreader card. I hear a Millennium item might be needed.",
            8 => "Bring me 3 Blue-Eyes White Dragons.",
            9 => "You have done well. Now bring me a Darkfire Dragon.",
            10 => "Get me a PSA 10 rated card.",
            11 => "Looks very good, and don't worry I wont take it from you. Now defeat a total of 200 enemies with card attacks.",
            12 => "You have completed all my tasks for now. Will you begin anew?",
            _ => "I have nothing more for you right now."
        };
    }

    public static bool TryComplete(Player player)
    {
        switch (CardQuestWorld.questStage)
        {
            case 1:
                if (MGHelpers.CountTaggedCards(player, "Warrior") >= 5)
                {
                    MGHelpers.ConsumeTaggedCards(player, "Warrior", 5);
                    MGHelpers.RewardPack(player);
                    player.QuickSpawnItem(player.GetSource_GiftOrReward(), ModContent.ItemType<FusionAltar>());
                    CardQuestWorld.questStage = 2;
                    return true;
                }
                break;

            case 2:
                if (player.inventory.Any(i => i.rare == ItemRarityID.Orange))
                {
                    for (int i = 0; i < player.inventory.Length; i++)
                    {
                        if (player.inventory[i].rare == ItemRarityID.Orange)
                        {
                            player.inventory[i].stack--;
                            if (player.inventory[i].stack <= 0)
                                player.inventory[i].TurnToAir();
                            break;
                        }
                    }

                    MGHelpers.RewardPack(player);
                    CardQuestWorld.questStage = 3;
                    return true;
                }
                break;

            case 3:
                if (MGHelpers.CountTaggedCards(player, "Fire") >= 15)
                {
                    MGHelpers.ConsumeTaggedCards(player, "Fire", 15);
                    MGHelpers.RewardPack(player);
                    MGHelpers.RewardPack(player);
                    player.QuickSpawnItem(player.GetSource_GiftOrReward(), ModContent.ItemType<FusionAltar>());
                    CardQuestWorld.questStage = 4;
                    return true;
                }
                break;

            case 4:
                if (player.HasItem(ModContent.ItemType<FlameSwordsman>()))
                {
                    MGHelpers.RewardPack(player);
                    MGHelpers.RewardPack(player);
                    CardQuestWorld.questStage = 5;
                    return true;
                }
                break;

            case 5:
                if (MGHelpers.CountTaggedCards(player, "Dragon") >= 10)
                {
                    MGHelpers.ConsumeTaggedCards(player, "Dragon", 10);
                    MGHelpers.RewardPack(player);
                    MGHelpers.RewardPack(player);
                    CardQuestWorld.questStage = 6;
                    return true;
                }
                break;

            case 6:
                if (CardQuestWorld.totalCardDamageKills >= 50)
                {
                    MGHelpers.RewardPack(player);
                    MGHelpers.RewardPack(player);
                    CardQuestWorld.questStage = 7;
                    return true;
                }
                break;

            case 7:
                if (player.HasItem(ModContent.ItemType<PlaguespreaderCard>()))
                {
                    MGHelpers.ConsumeItem(player, ModContent.ItemType<PlaguespreaderCard>(), 1);
                    player.QuickSpawnItem(player.GetSource_GiftOrReward(), ModContent.ItemType<PSACase>());
                    MGHelpers.RewardPack(player);
                    MGHelpers.RewardPack(player);
                    CardQuestWorld.questStage = 8;
                    return true;
                }
                break;

            case 8:
                if (player.CountItem(ModContent.ItemType<BEWD>()) >= 3)
                {
                    MGHelpers.ConsumeItem(player, ModContent.ItemType<BEWD>(), 3);
                    MGHelpers.RewardPack(player);
                    MGHelpers.RewardPack(player);
                    MGHelpers.RewardPack(player);
                    CardQuestWorld.questStage = 9;
                    return true;
                }
                break;

            case 9:
                if (player.HasItem(ModContent.ItemType<DarkfireDragon>()))
                {
                    MGHelpers.RewardPack(player);
                    MGHelpers.RewardPack(player);
                    MGHelpers.RewardPack(player);
                    CardQuestWorld.questStage = 10;
                    return true;
                }
                break;

            case 10:
                if (MGHelpers.HasAnyPSA10Card(player))
                {
                    MGHelpers.RewardPack(player);
                    MGHelpers.RewardPack(player);
                    MGHelpers.RewardPack(player);
                    CardQuestWorld.questStage = 11;
                    return true;
                }
                break;

            case 11:
                if (CardQuestWorld.totalCardDamageKills >= 200)
                {
                    MGHelpers.RewardPack(player);
                    MGHelpers.RewardPack(player);
                    MGHelpers.RewardPack(player);
                    MGHelpers.RewardPack(player);
                    MGHelpers.RewardPack(player);

                    player.QuickSpawnItem(player.GetSource_GiftOrReward(), ModContent.ItemType<QuestResetToken>());
                    CardQuestWorld.questStage = 12;
                    return true;
                }
                break;
        }

        return false;
    }
}