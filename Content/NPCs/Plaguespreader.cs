using NaturiumMod.Content.Items.General.Placeable;
using NaturiumMod.Content.Items.PreHardmode.ApophisItems;
using NaturiumMod.Content.Items.PreHardmode.Materials;
using NaturiumMod.Content.Items.Weapons.Melee;
using Terraria;
using Terraria.Enums;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

internal class Plaguespreader : ModNPC
{
    public override string Texture => "NaturiumMod/Assets/NPCs/Plaguespreader";

    public override void SetStaticDefaults()
    {
        Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.Zombie];

        NPCID.Sets.NPCBestiaryDrawModifiers value = new()
        {
            Velocity = 1f
        };
        NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
    }

    public override void SetDefaults()
    {
        NPC.CloneDefaults(NPCID.Zombie);

        NPC.damage = 18;
        NPC.defense = 4;
        NPC.lifeMax = 85;
        NPC.value = 200f;
        NPC.knockBackResist = 0.3f;

        AIType = NPCID.Zombie;
        AnimationType = NPCID.Zombie;

        Banner = NPC.type;
        BannerItem = ModContent.ItemType<PlaguespreaderBanner>();
    }
    public class PlaguespreaderBanner : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/General/Placeable/PlaguespreaderBanner";
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<EnemyBanner>(), (int)EnemyBanner.StyleID.Plaguespreader);
            Item.width = 10;
            Item.height = 24;
            Item.SetShopValues(ItemRarityColor.Blue1, Item.buyPrice(silver: 10));
        }
    }
    public override void ModifyNPCLoot(NPCLoot npcLoot)
    {
        var zombieDrops = Main.ItemDropsDB.GetRulesForNPCID(NPCID.Zombie, false);
        foreach (var rule in zombieDrops)
            npcLoot.Add(rule);

        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PlagueChunk>(), 1, 4, 7));
        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PlaguespreaderArm>(), 20, 1, 1));
    }

    public override float SpawnChance(NPCSpawnInfo spawnInfo)
    {
        bool inDesert = spawnInfo.Player.ZoneDesert;
        bool inCorrupt = spawnInfo.Player.ZoneCorrupt;
        bool inCrimson = spawnInfo.Player.ZoneCrimson;

        if (inDesert && (inCorrupt || inCrimson))
            return 0.5f;

        return 0f;
    }
    public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
    {
        bestiaryEntry.Info.AddRange([
            BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.CorruptDesert,
            BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.CorruptUndergroundDesert,
			new FlavorTextBestiaryInfoElement("Once an ordinary zombie, now infected by a plague that spreads to any nearby creature")
        ]);
    }

    public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
    {
        target.AddBuff(ModContent.BuffType<DecayDebuff>(), 180);
    }
}