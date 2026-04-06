using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.Cards.Fusion;
using NaturiumMod.Content.Items.Cards.LOB.SuperRares;
using NaturiumMod.Content.Items.Cards.LOB.SuperShortPrint;
using NaturiumMod.Content.Items.Cards.LOB.UltraRares;
using NaturiumMod.Content.Items.Cards.NPCDrop;
using NaturiumMod.Content.Items.PreHardmode.Consumables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Cards.PSA
{
    public abstract class BaseSimpleGradedCard<TOriginal> : BaseGradedCard where TOriginal : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            ItemTags.AddTagToItem(Type, "Card");
        }
        public abstract string CardName { get; }

        public override string Texture => $"NaturiumMod/Assets/Items/Cards/PSA/{CardName}Graded";

        public override int OriginalCardType => ModContent.ItemType<TOriginal>();

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<PSACase>());
            recipe.AddIngredient(ModContent.ItemType<TOriginal>());
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
    public class GradedPlaguespreaderCard : BaseSimpleGradedCard<PlaguespreaderCard>
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.value = Item.buyPrice(silver: 30);
        }
        public override string CardName => "Plaguespreader";
    }

    public class GradedSpiritReaperCard : BaseSimpleGradedCard<SpiritReaperCard>
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.value = Item.buyPrice(silver: 30);
        }
        public override string CardName => "SpiritReaper";
    }

    public class GradedCharubinCard : BaseSimpleGradedCard<Charubin>
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.value = Item.buyPrice(silver: 50);
        }
        public override string CardName => "Charubin";
    }

    public class GradedDragonessCard : BaseSimpleGradedCard<Dragoness>
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.value = Item.buyPrice(silver: 50);
        }
        public override string CardName => "Dragoness";
    }

    public class GradedFlameGhostCard : BaseSimpleGradedCard<FlameGhost>
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.value = Item.buyPrice(silver: 55);
        }
        public override string CardName => "FlameGhost";
    }

    public class GradedFlowerWolfCard : BaseSimpleGradedCard<FlowerWolf>
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.value = Item.buyPrice(silver: 50);
        }
        public override string CardName => "FlowerWolf";
    }

    public class GradedFusionistCard : BaseSimpleGradedCard<Fusionist>
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.value = Item.buyPrice(silver: 50);
        }
        public override string CardName => "Fusionist";
    }

    public class GradedGaiaChampionCard : BaseSimpleGradedCard<GaiaChampion>
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.value = Item.buyPrice(silver: 50);
        }
        public override string CardName => "GaiaChampion";
    }

    public class GradedKarbonalaCard : BaseSimpleGradedCard<Karbonala>
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.value = Item.buyPrice(silver: 50);
        }
        public override string CardName => "Karbonala";
    }

    public class GradedMetalDragonCard : BaseSimpleGradedCard<MetalDragon>
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.value = Item.buyPrice(silver: 55);
        }
        public override string CardName => "MetalDragon";
    }
    public class GradedBEWD : BaseSimpleGradedCard<BEWD>
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.value = Item.buyPrice(silver: 25);
        }
        public override string CardName => "BEWD";
    }

    public class GradedDarkMagician : BaseSimpleGradedCard<DarkMagician>
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.value = Item.buyPrice(silver: 25);
        }
        public override string CardName => "DarkMagician";
    }

    public class GradedExodia : BaseSimpleGradedCard<Exodia>
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.value = Item.buyPrice(silver: 20);
        }
        public override string CardName => "Exodia";
    }

    public class GradedLeftArm : BaseSimpleGradedCard<LeftArm>
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.value = Item.buyPrice(silver: 15);
        }
        public override string CardName => "LeftArm";
    }

    public class GradedRightArm : BaseSimpleGradedCard<RightArm>
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.value = Item.buyPrice(silver: 15);
        }
        public override string CardName => "RightArm";
    }

    public class GradedLeftLeg : BaseSimpleGradedCard<LeftLeg>
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.value = Item.buyPrice(silver: 15);
        }
        public override string CardName => "LeftLeg";
    }

    public class GradedRightLeg : BaseSimpleGradedCard<RightLeg>
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.value = Item.buyPrice(silver: 15);
        }
        public override string CardName => "RightLeg";
    }

    public class GradedREBD : BaseSimpleGradedCard<REBD>
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.value = Item.buyPrice(silver: 25);
        }
        public override string CardName => "REBD";
    }

    public class GradedPotofGreed : BaseSimpleGradedCard<PotofGreed>
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.value = Item.buyPrice(silver: 25);
        }
        public override string CardName => "PotofGreed";
    }

    public class GradedMonsterReborn : BaseSimpleGradedCard<MonsterReborn>
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.value = Item.buyPrice(silver: 25);
        }
        public override string CardName => "MonsterReborn";
    }
    public class GradedDarkfireDragon : BaseSimpleGradedCard<DarkfireDragon>
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.value = Item.buyPrice(silver: 55);
        }
        public override string CardName => "DarkfireDragon";
    }

    public class GradedFlameSwordsman : BaseSimpleGradedCard<FlameSwordsman>
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.value = Item.buyPrice(silver: 55);
        }
        public override string CardName => "FlameSwordsman";
    }
    public class GradedGaia : BaseSimpleGradedCard<Gaia>
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.value = Item.buyPrice(silver: 15);
        }
        public override string CardName => "Gaia";
    }

}
