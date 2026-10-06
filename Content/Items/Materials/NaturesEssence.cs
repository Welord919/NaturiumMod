using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Materials;

internal class NaturesEssence : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/Materials/NaturesEssence";
    public override void SetStaticDefaults()
    {
        Main.RegisterItemAnimation(Type, new DrawAnimationVertical(5, 4));
        ItemID.Sets.AnimatesAsSoul[Type] = true; // Makes the item have an animation while in world (not held.). Use in combination with RegisterItemAnimation
        ItemID.Sets.ItemIconPulse[Type] = true; // The item pulses while in the player's inventory
        ItemID.Sets.ItemNoGravity[Type] = true; // Makes the item have no gravity

        Item.ResearchUnlockCount = 25;
    }

    public override void SetDefaults()
    {
        Item refItem = new Item();
        refItem.SetDefaults(ItemID.SoulofSight);

        Item.width = refItem.width;
        Item.height = refItem.height;

        Item.rare = ItemRarityID.Lime;
        Item.value = Item.sellPrice(gold: 1);

        Item.maxStack = 9999;
    }
    public override void PostUpdate()
           => Lighting.AddLight(Item.Center, Color.Yellow.ToVector3() * 0.45f * Main.essScale);
    public override Color? GetAlpha(Color lightColor)
        => Color.Green;

}