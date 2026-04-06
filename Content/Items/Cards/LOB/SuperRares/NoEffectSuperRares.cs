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
    public abstract class ExodiaPiece : BaseCardSuper
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/NoEffects/Exodia";
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.value = 1000;
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
