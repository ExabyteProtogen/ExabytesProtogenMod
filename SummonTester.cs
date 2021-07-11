using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace ExabytesProtogenMod
{
	public class AirSupportBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Air Support");
			Description.SetDefault("Backup has arrived!");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (player.ownedProjectileCounts[ModContent.ProjectileType<AirSupportDrone>()] == 0)
			{
				player.DelBuff(buffIndex);
				buffIndex--;
			}
		}
	}
	public class AirSupportCooldown : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Air Support Cooldown");
			Description.SetDefault("Your drone needs time to re-build...");
			canBeCleared = false;
			Main.buffNoSave[Type] = true;
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			
		}
	}
	public class SummonTester : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Example Minion Item");
			Tooltip.SetDefault("Summons an example minion to fight for you");
			ItemID.Sets.GamepadWholeScreenUseRange[item.type] = true; // This lets the player target anywhere on the whole screen while using a controller.
			ItemID.Sets.LockOnIgnoresCollision[item.type] = true;
		}

		public override void SetDefaults()
		{
			item.damage = 10;
			item.knockBack = 3f;
			item.mana = 10;
			item.width = 32;
			item.height = 32;
			item.useTime = 36;
			item.useAnimation = 36;
			item.useStyle = 1;
			item.value = Item.buyPrice(0, 30, 0, 0);
			item.rare = 9;
			item.UseSound = SoundID.Item44;

			// These below are needed for a minion weapon
			item.noMelee = true;
			item.summon = true;
			item.buffType = ModContent.BuffType<AirSupportBuff>();
			// No buffTime because otherwise the item tooltip would say something like "1 minute duration"
			item.shoot = ModContent.ProjectileType<AirSupportDrone>();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			// This is needed so the buff that keeps your minion alive and allows you to despawn it properly applies
			player.AddBuff(item.buffType, 2);

			// Here you can change where the minion is spawned. Most vanilla minions spawn at the cursor position.
			position = Main.MouseWorld;
			return true;
		}
	}
}