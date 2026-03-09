using Microsoft.Xna.Framework.Graphics;
using NaturiumMod.Content.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;


namespace NaturiumMod.Content.Items.PreHardmode.Accessories;
public class LeodrakesMane : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Weapons/LeodrakesYoyo";
    public override void SetDefaults()
    {
        Item.width = 28;
        Item.height = 28;
        Item.accessory = true;
        Item.rare = ItemRarityID.Orange;
        Item.value = Item.buyPrice(gold: 3);
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetModPlayer<LeodrakesManePlayer>().leodrakeManeEquipped = true;
    }
}