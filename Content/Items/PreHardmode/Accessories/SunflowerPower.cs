using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.PreHardmode.Consumables;
using NaturiumMod.Content.Items.PreHardmode.Materials;
using NaturiumMod.Content.Items.PreHardmode.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace NaturiumMod.Content.Items.PreHardmode.Accessories;

public class SunflowerPower : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Accessories/SunflowerPower2";
    public override void SetDefaults()
    {
        Item.Size = new(20, 26);
        Item.rare = ItemRarityID.Orange;
        Item.value = Item.buyPrice(0, 1, 0, 0);
        Item.maxStack = 1;
        Item.accessory = true;
    }

    public override void UpdateInventory(Player player)
    {
        // Sunflower buff (movement speed + reduced spawns)
        player.AddBuff(BuffID.Sunflower, 2);

        // Extra custom movement speed
        player.moveSpeed += 0.10f;       
        player.runAcceleration *= 1.03f; 
    }
}

