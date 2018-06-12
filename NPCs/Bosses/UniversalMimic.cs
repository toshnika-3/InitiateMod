using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using InitiateMod.NPCs.Bosses;

namespace InitiateMod.NPCs.Bosses
{
	public class UniversalMimic : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Universal Mimic"); 
			Main.npcFrameCount[npc.type] = 6; // make sure to set this for your modnpcs.
		}

		public override void SetDefaults()
		{
			npc.width = 75;
			npc.height = 75;
			npc.aiStyle = -1; // This npc has a completely unique AI, so we set this to -1.
			npc.damage = 60;
			npc.defense = 50;
			npc.lifeMax = 50000;
			npc.knockBackResist = 0f;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.npcSlots = 50f;
			npc.boss = true;
			npc.lavaImmune = true;
			npc.noTileCollide = false;
			music = MusicID.Boss1;
			npc.value = 0f;
			npc.buffImmune[BuffID.Confused] = true;
		}

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.lifeMax = (int)(npc.lifeMax * 0.625f * bossLifeScale);
			npc.damage = (int)(npc.damage * 0.6f);
		}

	

		const int AI_State_Slot = 0;
		const int AI_Timer_Slot = 1;
		const int AI_Flutter_Time_Slot = 2;
		const int AI_Unused_Slot_3 = 3;

		// npc.localAI will also have 4 float variables available to use. With ModNPC, using just a local class member variable would have the same effect.
		const int Local_AI_Unused_Slot_0 = 0;
		const int Local_AI_Unused_Slot_1 = 1;
		const int Local_AI_Unused_Slot_2 = 2;
		const int Local_AI_Unused_Slot_3 = 3;

		// Here I define some values I will use with the State slot. Using an ai slot as a means to store "state" can simplify things greatly. Think flowchart.
		const int State_En_Garde = 0;
		const int State_Notice = 1;
        	const int State_ShadowBeam = 2;

		// This is a property (https://msdn.microsoft.com/en-us/library/x9fsa0sw.aspx), it is very useful and helps keep out AI code clear of clutter.
		// Without it, every instance of "AI_State" in the AI code below would be "npc.ai[AI_State_Slot]". 
		// Also note that without the "AI_State_Slot" defined above, this would be "npc.ai[0]".
		// This is all to just make beautiful, manageable, and clean code.
		public float AI_State
		{
			get { return npc.ai[AI_State_Slot]; }
			set { npc.ai[AI_State_Slot] = value; }
		}

		public float AI_Timer
		{
			get { return npc.ai[AI_Timer_Slot]; }
			set { npc.ai[AI_Timer_Slot] = value; }
		}

		public float AI_FlutterTime
		{
			get { return npc.ai[AI_Flutter_Time_Slot]; }
			set { npc.ai[AI_Flutter_Time_Slot] = value; }
		}

		

		public override void AI()
		{
			// The npc starts in the asleep state, waiting for a player to enter range
			if (AI_State == State_En_Garde)
			{
				
				npc.TargetClosest(true);
				// Now we check the make sure the target is still valid and within our specified notice range (500)
				if (npc.HasValidTarget && Main.player[npc.target].Distance(npc.Center) < 1000f)
				{
					// Since we have a target in range, we change to the Notice state. (and zero out the Timer for good measure)
					AI_State = State_Notice;
					AI_Timer = 0;
				}
			}
			// In this state, a player has been targeted
			else if (AI_State == State_Notice)
			{
				/// If the targeted player is in attack range (250).
				if (Main.player[npc.target].Distance(npc.Center) < 800f)
				{
	
					AI_Timer++;
					if (AI_Timer >= 20)
					{
						AI_State = State_ShadowBeam;
						AI_Timer = 0;
					}
				}
				else
				{
					npc.TargetClosest(true);
					if (!npc.HasValidTarget || Main.player[npc.target].Distance(npc.Center) > 800f)
					{
						// Out targeted player seems to have left our range, so we'll go back to sleep.
						AI_State = State_En_Garde;
						AI_Timer = 0;
					}
				}
			}
			// In this state, we are in the jump. 
			else if (AI_State == State_ShadowBeam)
			{
				AI_Timer++;
				if (AI_Timer == 1)
				{
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, SpeedX: 5, SpeedY: -5, Type: ProjectileID.ShadowBeamHostile, Damage: 40, KnockBack: 2f, ai0: npc.whoAmI);
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, SpeedX: -5, SpeedY: 5, Type: ProjectileID.ShadowBeamHostile, Damage: 40, KnockBack: 2f, ai0: npc.whoAmI);
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, SpeedX: -5, SpeedY: -5, Type: ProjectileID.ShadowBeamHostile, Damage: 40, KnockBack: 2f, ai0: npc.whoAmI);
		    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, SpeedX: 5, SpeedY: 5, Type: ProjectileID.ShadowBeamHostile, Damage: 40, KnockBack: 2f, ai0: npc.whoAmI);
		    NPC.NewNPC((int)npc.Center.X, (int)npc.position.Y + npc.height, mod.NPCType("Ragnarock"));
				}
				else if (AI_Timer > 40)
				{
					// after .66 seconds, we go to the hover state. // TODO, gravity?
                    AI_State = State_ShadowBeam;
					AI_Timer = 0;
				}
			}

	}

        public override void NPCLoot()
        {

            NPC.NewNPC((int)npc.Center.X, (int)npc.position.Y + npc.height, mod.NPCType("UniversalMimic2"));
           
        }

    }
}