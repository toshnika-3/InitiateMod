using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.GameInput;
using InitiateMod;
using InitiateMod.Blocks;
using InitiateMod.StatusEffects;

namespace InitiateMod
{
	public class InitiatePlayer : ModPlayer
	{
		private const int saveVersion = 0;
		public bool razorskiss = false;
        public bool MeganeuraPet = false;
		public bool PrehistoricSwamp = false;
		public override void ResetEffects()
		{
			razorskiss = false;
			MeganeuraPet = false;
		}
		
        public override void UpdateBadLifeRegen()
        {
            if (razorskiss)
            {
                if (player.lifeRegen > 0)
                {
                    player.lifeRegen = 0;
                }
                player.lifeRegenTime = 0;
            }
           
        }
	public override void UpdateBiomes()
        {
           PrehistoricSwamp = (InitiateWorld.PrehistoricSwamp > 0);    
        }
	}
}
