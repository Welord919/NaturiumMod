using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Cards.LOB.NoEffect
{
    public abstract class LeftLeg : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/NoEffects/LeftLeg";
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 999;
            Item.value = 250;
            Item.rare = ItemRarityID.Orange;
        }
    }
    public class RightLeg : LeftLeg
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/NoEffects/LeftArm";
    }
    public class LeftArm : LeftLeg
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/NoEffects/RightArm";
    }
    public class RightArm : LeftLeg
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/NoEffects/RightLeg";
    }
    public class Exodia : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/NoEffects/Exodia";
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 999;
            Item.value = 500;
            Item.rare = ItemRarityID.Orange;
        }
    }
}
