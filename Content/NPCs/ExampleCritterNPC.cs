using Microsoft.Xna.Framework;
using MonoMod.Cil;
using NaturiumMod.Content.Items.Critter;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace NaturiumMod.Content.NPCs
{
	public class ExampleCritterNPC : ModNPC
	{
		private const int ClonedNPCID = NPCID.Frog; // Easy to change type for your modder convenience

		public override void Load()
		{
			IL_Wiring.HitWireSingle += HookFrogStatue;
		}

		private void HookFrogStatue(ILContext ilContext)
		{
			try
			{
				// Obtain a cursor positioned before the first instruction of the method the cursor is used for navigating and modifying the il
				ILCursor ilCursor = new ILCursor(ilContext);

				// The exact location for this hook is very complex to search for due to the hook instructions not being unique and buried deep in control flow. Switch statements are sometimes compiled to if-else chains, and debug builds litter the code with no-ops and redundant locals.
				// In general you want to search using structure and function rather than numerical constants which may change across different versions or compile settings. Using local variable indices is almost always a bad idea.
				// We can search for
				// switch (*)
				//   case 61:
				//     num115 = 361;

				// In general you'd want to look for a specific switch variable, or perhaps the containing switch (type) { case 105: but the generated IL is really variable and hard to match in this case.
				// We'll just use the fact that there are no other switch statements with case 61

				ILLabel[] targets = null;
				while (ilCursor.TryGotoNext(i => i.MatchSwitch(out targets)))
				{
					// Some optimizing compilers generate a sub so that all the switch cases start at 0:
					// ldc.i4.s 30
					// sub
					// switch
					int offset = 0;
					if (ilCursor.Prev.MatchSub() && ilCursor.Prev.Previous.MatchLdcI4(out offset))
					{
						;
					}

					// Get the label for case 61: if it exists
					int case61Index = 61 - offset;
					if (case61Index < 0 || case61Index >= targets.Length || targets[case61Index] is not ILLabel target)
					{
						continue;
					}

					// Move the cursor to case 61:
					ilCursor.GotoLabel(target);
					// Move the cursor after 361 is pushed onto the stack
					ilCursor.Index++;
					// There are lots of extra checks we could add here to make sure we're at the right spot, such as not encountering any branching instructions

					// Now we add additional code to modify the current value that will be assigned to num115
					ilCursor.EmitDelegate((int originalAssign) => Main.rand.NextBool() ? originalAssign : NPC.type);

					// Hook applied successfully
					return;
				}

				// Couldn't find the right place to insert.
				throw new Exception("Hook location not found, switch(*) { case 61: ...");
			}
			catch
			{
				// If there are any failures with the IL editing, this method will dump the IL to Logs/ILDumps/{Mod Name}/{Method Name}.txt
				MonoModHooks.DumpIL(ModContent.GetInstance<NaturiumMod>(), ilContext);
			}
		}


		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[Type] = Main.npcFrameCount[ClonedNPCID]; // Copy animation frames
			Main.npcCatchable[Type] = true; // This is for certain release situations

			// These three are typical critter values
			NPCID.Sets.CountsAsCritter[Type] = true;
			NPCID.Sets.TakesDamageFromHostilesWithoutBeingFriendly[Type] = true;
			NPCID.Sets.TownCritter[Type] = true;

			// The frog is immune to confused
			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;

			// This is so it appears between the frog and the gold frog
			NPCID.Sets.NormalGoldCritterBestiaryPriority.Insert(NPCID.Sets.NormalGoldCritterBestiaryPriority.IndexOf(ClonedNPCID) + 1, Type);
		}

		public override void SetDefaults()
		{
			NPC.width = 12;
			NPC.height = 10;
			NPC.aiStyle = 7;
			NPC.damage = 0;
			NPC.defense = 0;
			NPC.lifeMax = 5;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
            NPC.catchItem = 2121;
			//Sets the above
			NPC.CloneDefaults(ClonedNPCID);

			NPC.catchItem = ModContent.ItemType<ExampleCritterItem>();
			NPC.lavaImmune = true;
			AIType = ClonedNPCID;
			AnimationType = ClonedNPCID;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld,
				new FlavorTextBestiaryInfoElement("The most adorable goodest spicy child. Do not dare be mean to him!"));
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return SpawnCondition.Underworld.Chance * 0.1f;
		}

		public override void HitEffect(NPC.HitInfo hit)
		{
			if (NPC.life <= 0)
			{
				for (int i = 0; i < 6; i++)
				{
					Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Worm, 2 * hit.HitDirection, -2f);
					if (Main.rand.NextBool(2))
					{
						dust.noGravity = true;
						dust.scale = 1.2f * NPC.scale;
					}
					else
					{
						dust.scale = 0.7f * NPC.scale;
					}
				}
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>($"{Name}_Gore_Head").Type, NPC.scale);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>($"{Name}_Gore_Leg").Type, NPC.scale);
			}
		}

		public override Color? GetAlpha(Color drawColor)
		{
			// GetAlpha gives our Lava Frog a red glow.
			return drawColor with
			{
				R = 255,
				// Both these do the same in this situation, using these methods is useful.
				G = Utils.Clamp<byte>(drawColor.G, 175, 255),
				B = Math.Min(drawColor.B, (byte)75),
				A = 255
			};
		}

		public override bool PreAI()
		{
			// Kills the NPC if it hits water, honey or shimmer
			if (NPC.wet && !Collision.LavaCollision(NPC.position, NPC.width, NPC.height))
			{ // NPC.lavawet not 100% accurate for the frog
			  // These 3 lines instantly kill the npc without showing damage numbers, dropping loot, or playing DeathSound. Use this for instant deaths
				NPC.life = 0;
				NPC.HitEffect();
				NPC.active = false;
				SoundEngine.PlaySound(SoundID.NPCDeath16, NPC.position); // plays a fizzle sound
			}

			return true;
		}

        /*public override void OnCaughtBy(Player player, Item item, bool failed)
		{
			if (failed)
			{
				return;
			}

			Point npcTile = NPC.Center.ToTileCoordinates();

			if (!WorldGen.SolidTile(npcTile.X, npcTile.Y))
			{ // Check if the tile the npc resides the most in is non solid
				Tile tile = Main.tile[npcTile];
				tile.LiquidAmount = tile.LiquidType == LiquidID.Lava ? // Check if the tile has lava in it
					Math.Max((byte)Main.rand.Next(50, 150), tile.LiquidAmount) // If it does, then top up the amount
					: (byte)Main.rand.Next(50, 150); // If it doesn't, then overwrite the amount. Technically this distinction should never be needed bc it will burn but to be safe it's here
				tile.LiquidType = LiquidID.Lava; // Set the liquid type to lava
				WorldGen.SquareTileFrame(npcTile.X, npcTile.Y, true); // Update the surrounding area in the tilemap
			}
		}*/
    }
}