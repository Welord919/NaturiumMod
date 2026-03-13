using NaturiumMod.Content.Helpers;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.PreHardmode.Cards;
[AutoloadEquip(EquipType.Body)]

public class KaibasCloak : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Cards/KaibasCloak";
    public override void SetDefaults()
    {
        Item.width = 18;
        Item.height = 14;
        Item.accessory = true;
        Item.rare = ItemRarityID.LightPurple;
        Item.value = Item.buyPrice(gold: 50);
    }
    public override void Load()
    {
        if (Main.netMode == NetmodeID.Server)
        {
            return;
        }
        EquipLoader.AddEquipTexture(Mod, $"{Texture}_{EquipType.Legs}", EquipType.Legs, this);
    }
    public override void SetStaticDefaults()
    {
        ArmorIDs.Body.Sets.HidesHands[Item.bodySlot] = false;
    }
    public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
    {
        robes = true;
        equipSlot = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Legs);
    }
    public override void UpdateEquip(Player player)

    {
        player.GetModPlayer<KaibaPlayer>().KaibasCloakEquipped = true;
        var boost = player.GetModPlayer<WeaponBoostPlayer>();
        boost.activeBoosts["Dragon"] = true;
        
    }
}
public class KaibaPlayer : ModPlayer
{
    public bool KaibasCloakEquipped;

    public override void ResetEffects()
    {
        KaibasCloakEquipped = false;
    }
}


