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
using Terraria.ModLoader.Utilities;

namespace NaturiumMod.Content.Items.PreHardmode.ApophisItems;
    public class TabletoftheKings : ModItem
    {
    public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Apophis/TabletoftheKings";
    public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.accessory = true;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.sellPrice(gold: 5);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<TabletPlayer>().apophisBoost = true;
            player.maxMinions += 1;
            player.pickSpeed -= 0.25f;
    }
    }
    
