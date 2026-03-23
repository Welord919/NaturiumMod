using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Cards.LOB.SuperRares
{
    public abstract class ExodiaPiece : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/NoEffects/Exodia";
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 999;
            Item.value = 250;
            Item.rare = ItemRarityID.Orange;
        }
    }
    public class RightLeg : ExodiaPiece
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/NoEffects/RightLeg";
    }
    public class LeftLeg : ExodiaPiece
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/NoEffects/LeftLeg";
    }
    public class LeftArm : ExodiaPiece
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/NoEffects/LeftArm";
    }
    public class RightArm : ExodiaPiece
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/NoEffects/RightArm";
    }
    
}
