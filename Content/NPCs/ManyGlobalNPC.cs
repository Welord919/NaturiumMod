using Microsoft.Xna.Framework;
using NaturiumMod.Content.Items.General.Placeable;
using NaturiumMod.Content.Items.PostHardmode.Materials;
using NaturiumMod.Content.Items.PreHardmode.Accessories;
using NaturiumMod.Content.Items.PreHardmode.ApophisItems;
using NaturiumMod.Content.Items.PreHardmode.Materials;
using System;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.NPCs;

public class ManyGlobalNPC : GlobalNPC
{
    public override void ModifyShop(NPCShop shop)
    {
        int[] itemsToAdd = GetItemsToAdd(shop.NpcType);
        foreach (int item in itemsToAdd)
        {
            shop.Add(item);
        }
    }

    private static int[] GetItemsToAdd(int npcID)
    {
        return npcID switch
        {
            NPCID.Dryad =>
            [
                ModContent.ItemType<BarkionsBark>(),
                ModContent.ItemType<BarkionsMedallion>(),
                ModContent.ItemType<CameliaSeeds>(),
                ItemID.Vine
            ],
            NPCID.Demolitionist =>
            [
                ItemID.Diamond,
                ItemID.Ruby,
                ItemID.Emerald,
                ItemID.Sapphire,
                ItemID.Topaz,
                ItemID.Amethyst
            ],
            _ => []
        };
    }
    public class EoCDrops : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type != NPCID.EyeofCthulhu)
                return;

            // Classic only
            if (!Main.expertMode)
            {
                npcLoot.Add(ItemDropRule.Common(
                    ModContent.ItemType<NaturiumOre>(),
                    1,
                    18,
                    36
                ));
            }
        }
    }
    public class NaturesEssenceDrop : GlobalNPC

    {
        public override void OnKill(NPC npc)
        {
            Player player = Main.player[npc.lastInteraction];

            if (!Main.hardMode)
                return;

            if (!player.ZoneJungle || !player.ZoneRockLayerHeight)
                return;

            // Exclude statue-spawned NPCs
            if (npc.SpawnedFromStatue)
                return;

            // Exclude critters and town NPCs
            if (npc.friendly || npc.lifeMax <= 5)
                return;

            // Drop chance, 1/20
            if (Main.rand.NextBool(20))
            {
                Item.NewItem(
                    npc.GetSource_Loot(),
                    npc.Hitbox,
                    ModContent.ItemType<NaturesEssence>()
                );
            }
        }

    }
    public class AntiGravityGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public bool hadAntiGravity;

        public override void ResetEffects(NPC npc)
        {
            // Only reset gravity if THIS NPC had the debuff last tick
            if (hadAntiGravity)
            {
                npc.noGravity = false;
                hadAntiGravity = false;
            }
        }
    }
    public class DecayGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public bool hasDecay;

        public override void PostAI(NPC npc)
        {
            if (npc.HasBuff(ModContent.BuffType<DecayDebuff>()))
            {
                hasDecay = true;
            }
        }

        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (hasDecay)
            {
                int dps = 6;
                npc.lifeRegen -= dps;

                if (damage < dps / 2)
                    damage = dps / 2;
            }
        }

        public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
        {
            if (hasDecay)
            {
                modifiers.Defense.Flat -= 4;
            }
        }

        public override void ResetEffects(NPC npc)
        {
            if (!npc.HasBuff(ModContent.BuffType<DecayDebuff>()))
            {
                hasDecay = false;
            }
        }

        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            if (hasDecay)
            {
                // Purple plague dust
                if (Main.rand.NextBool(4))
                {
                    int dust = Dust.NewDust(
                        npc.position,
                        npc.width,
                        npc.height,
                        DustID.Poisoned, // base dust type
                        0f, 0f,
                        150,
                        new Color(180, 0, 255), // purple tint
                        1.3f
                    );

                    Main.dust[dust].noGravity = true;
                }

                // Slight purple tint on the NPC
                drawColor = Color.Lerp(drawColor, new Color(180, 0, 255), 0.35f);
            }
        }
    }



}
