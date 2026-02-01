using NaturiumMod.Content.Items.PostHardmode.Materials;
using System;
using System.Collections.Generic;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace NaturiumMod.Content.NPCs
{
    internal class NaturesEssenceDrop : GlobalNPC

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

            // Drop chance, 1/10
            if (Main.rand.NextBool(1))
            {
                Item.NewItem(
                    npc.GetSource_Loot(),
                    npc.Hitbox,
                    ModContent.ItemType<NaturesEssence>()
                );
            }
        }

    }
}
