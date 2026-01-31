using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Generation;

public readonly struct ChestInfo(Chest chest, int dropItemID)
{
    public readonly Chest Chest = chest;
    public readonly int DropItemID = dropItemID;
    public readonly ChestType? Type = Enum.IsDefined(typeof(ChestType), dropItemID)
        ? (ChestType)dropItemID : null;

    public bool IsType (ChestType type) =>
        DropItemID == (int)type;

    public void TryAddItem<T>(int minimum, int maximum, float chance) where T : ModItem
    {
        if (Main.rand.NextFloat() >= chance)
        {
            return;
        }

        int stack = Main.rand.Next(minimum, maximum + 1);
        int itemType = ModContent.ItemType<T>();

        for (int i = 0; i < Chest.maxItems; i++)
        {
            if (Chest.item[i] is null)
            {
                Chest.item[i] = new Item();
            }

            if (Chest.item[i].type == ItemID.None)
            {
                Chest.item[i].SetDefaults(itemType);
                Chest.item[i].stack = stack;
                return;
            }
        }
    }
}