//make sure to add "modReferences = MrPlagueRaces" into your mod's Build.txt
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using MrPlagueRaces.Common.Races;

//this is a custom race file. It contains the code that makes up the race
namespace ExabytesProtogenMod.Common.Races.Protogens
{
	public class Protogen : Race
	{
		public override string RaceSelectIcon => ($"ExabytesProtogenMod/Common/UI/RaceDisplay/ProtogenSelect");
		public override string RaceDisplayMaleIcon => ($"ExabytesProtogenMod/Common/UI/RaceDisplay/ProtogenDisplayMale");
		public override string RaceDisplayFemaleIcon => ($"ExabytesProtogenMod/Common/UI/RaceDisplay/ProtogenDisplayFemale");
		public override string RaceDisplayName => "Protogens";
		public override string RaceLore1 => "Protogens are " + "\na curious cybernetic " + "\nrace that travel " + "\nthe stars.";
		public override string RaceLore2 => "Protogens are gifted tinkerers, " + "\nalways improving their craft.";
		public override string RaceAbilityName => "Air Support";
		public override string RaceAbilityDescription1 => "Press [c/34EB93:Racial Ability Hotkey] to summon an air support drone!";
		public override string RaceAbilityDescription2 => "Air support is unavailable untill EoC is defeated.";
		public override string RaceAbilityDescription3 => "Air support improves based on bosses killed.";
		public override string RaceAbilityDescription4 => "(EoC, Queen Bee, WoF, All Mech Bosses, Golem, Cultist, then Moonlord)";
		public override string RaceAbilityDescription5 => "";
		public override string RaceAbilityDescription6 => "";
		public override string RaceAdditionalNotesDescription1 => "-Begins short-circuiting in water.";
		public override string RaceEnvironmentIcon => $"MrPlagueRaces/Common/UI/RaceDisplay/Environment/Environment_Pillar_Vortex";
		public override string RaceEnvironmentOverlay1Icon => $"MrPlagueRaces/Common/UI/RaceDisplay/BlankDisplay";
		public override string RaceEnvironmentOverlay2Icon => $"MrPlagueRaces/Common/UI/RaceDisplay/BlankDisplay";
		public override string RaceGoodBiomesDisplayText => "Space";
		public override string RaceBadBiomesDisplayText => "Hallow, Underworld";
		public override void ModifyDrawInfo(Player player, Mod mod, ref PlayerDrawInfo drawInfo)
        {
			var modPlayer = player.GetModPlayer<MrPlagueRaces.MrPlagueRacesPlayer>();
			Item familiarshirt = new Item();
			familiarshirt.SetDefaults(ItemID.FamiliarShirt);
			Item familiarpants = new Item();
			familiarpants.SetDefaults(ItemID.FamiliarPants);
            if (modPlayer.resetDefaultColors)
            {
				modPlayer.resetDefaultColors = false;
				player.skinColor = new Color(25, 255, 255);
				player.eyeColor = new Color(0, 0, 255);
            }
		}
		public override string RaceMeleeDamageDisplayText => "[c/FF4F64:-15%]";
		public override string RaceBulletDamageDisplayText => "[c/34EB93:+15%]";
		public override string RaceRocketDamageDisplayText => "[c/34EB93:+15%]";
		public override string RaceArrowDamageDisplayText => "[c/FF4F64:-15%]";
		public override string RaceMagicDamageDisplayText => "[c/FF4F64:-30%]";
		public override string RaceSummonDamageDisplayText => "[c/34EB93:+30%]";
		public override string RaceSentriesDisplayText => "[c/34EB93:+1]";
		public override string RaceMinionsDisplayText => "[c/34EB93:+1]";
		public override string RaceMeleeSpeedDisplayText => "[c/FF4F64:-30%]";
		bool activeKeyhold = false;

		public override void ResetEffects(Player player)
        {
			var modPlayer = player.GetModPlayer<MrPlagueRaces.MrPlagueRacesPlayer>();
			if (modPlayer.RaceStats)
            {
				player.meleeDamage -= 0.15f;
				player.bulletDamage += 0.15f;
				player.rocketDamage += 0.15f;
				player.arrowDamage -= 0.15f;
				player.magicDamage -= 0.3f;
				player.minionDamage += 0.3f;
				player.maxTurrets += 1;
				player.maxMinions += 1;
				player.meleeSpeed -= 0.3f;
            }
        }

		public override void ProcessTriggers(Player player, Mod mod)
		{
			//custom hotkey stuff goes here
			var modPlayer = player.GetModPlayer<MrPlagueRaces.MrPlagueRacesPlayer>();
			if (modPlayer.RaceStats)
			{
				if (MrPlagueRaces.MrPlagueRaces.RacialAbilityHotKey.Current && !player.dead && !player.HasBuff(ModContent.BuffType<AirSupportCooldown>()) && NPC.downedBoss1)
				{
					player.AddBuff(ModContent.BuffType<AirSupportBuff>(), 600);
                    if (NPC.downedAncientCultist)
                    {
						Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<AirSupportDrone>(), 1, 1f, Main.myPlayer, 0f, 0f);
						Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<AirSupportDrone>(), 1, 1f, Main.myPlayer, 0f, 0f);
						player.AddBuff(ModContent.BuffType<AirSupportCooldown>(), 4200);
					}
                    else
                    {
						Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<AirSupportDrone>(), 1, 1f, Main.myPlayer, 0f, 0f);
						player.AddBuff(ModContent.BuffType<AirSupportCooldown>(), 4200);
					}
				}
                else if (MrPlagueRaces.MrPlagueRaces.RacialAbilityHotKey.Current && !player.dead && !NPC.downedBoss1 && !activeKeyhold)
                {
					Main.NewText("You must defeat Eye of Cthulhu before you can activate your drone!");
					activeKeyhold = true;
                }
                else if (!MrPlagueRaces.MrPlagueRaces.RacialAbilityHotKey.Current)
                {
					activeKeyhold = false;
				}
			}
		}

		public override void PreUpdate(Player player, Mod mod)
		{
			var modPlayer = player.GetModPlayer<MrPlagueRaces.MrPlagueRacesPlayer>();
			if (modPlayer.RaceStats)
            {
				if (Collision.DrownCollision(player.position, player.width, player.height, player.gravDir) && !player.HasBuff(ModContent.BuffType<WaterSealantBuff>()))
				{
					player.AddBuff(BuffID.Electrified, 1);
				}
                else if (player.HasBuff(BuffID.Electrified))
                {
					player.ClearBuff(BuffID.Electrified);
                }
			}
		}

		public override void ModifyDrawLayers(Player player, List<PlayerLayer> layers)
		{
			var modPlayer = player.GetModPlayer<MrPlagueRaces.MrPlagueRacesPlayer>();

			bool hideChestplate = modPlayer.hideChestplate;
			bool hideLeggings = modPlayer.hideLeggings;

			Main.playerTextures[0, 0] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Head");
			Main.playerTextures[0, 1] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Eyes_2");
			Main.playerTextures[0, 2] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Eyes");
			Main.playerTextures[0, 3] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Torso");

			if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
			{
				Main.playerTextures[0, 4] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Sleeves_1");
			}
			else
			{
				Main.playerTextures[0, 4] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			}

			Main.playerTextures[0, 5] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Hands");

			if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
			{
				Main.playerTextures[0, 6] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Shirt_1");
			}
			else
			{
				Main.playerTextures[0, 6] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			}

			Main.playerTextures[0, 7] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Arm");

			if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
			{
				Main.playerTextures[0, 8] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Sleeve_1");
			}
			else
			{
				Main.playerTextures[0, 8] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			}

			Main.playerTextures[0, 9] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Hand");
			Main.playerTextures[0, 10] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Legs");

			if ((player.armor[2].type == ItemID.FamiliarPants || player.armor[12].type == ItemID.FamiliarPants) && !hideLeggings)
			{
				Main.playerTextures[0, 11] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Pants_1");
				Main.playerTextures[0, 12] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Shoes_1");
			}
			else
			{
				Main.playerTextures[0, 11] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
				Main.playerTextures[0, 12] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			}
			if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
			{
				Main.playerTextures[0, 13] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Sleeve_1_2");
			}
			else
			{
				Main.playerTextures[0, 13] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			}
			if ((player.armor[2].type == ItemID.FamiliarPants || player.armor[12].type == ItemID.FamiliarPants) && !hideLeggings)
			{
				Main.playerTextures[0, 14] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Pants_1_2");
			}
			else
			{
				Main.playerTextures[0, 14] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			}

			Main.playerTextures[1, 0] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Head");
			Main.playerTextures[1, 1] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Eyes_2");
			Main.playerTextures[1, 2] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Eyes");
			Main.playerTextures[1, 3] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Torso");

			if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
			{
				Main.playerTextures[1, 4] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Sleeves_2");
			}
			else
			{
				Main.playerTextures[1, 4] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			}

			Main.playerTextures[1, 5] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Hands");

			if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
			{
				Main.playerTextures[1, 6] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Shirt_2");
			}
			else
			{
				Main.playerTextures[1, 6] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			}

			Main.playerTextures[1, 7] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Arm");

			if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
			{
				Main.playerTextures[1, 8] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Sleeve_2");
			}
			else
			{
				Main.playerTextures[1, 8] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			}

			Main.playerTextures[1, 9] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Hand");
			Main.playerTextures[1, 10] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Legs");

			if ((player.armor[2].type == ItemID.FamiliarPants || player.armor[12].type == ItemID.FamiliarPants) && !hideLeggings)
			{
				Main.playerTextures[1, 11] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Pants_2");
				Main.playerTextures[1, 12] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Shoes_2");
			}
			else
			{
				Main.playerTextures[1, 11] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
				Main.playerTextures[1, 12] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			}
			if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
			{
				Main.playerTextures[1, 13] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Sleeve_2_2");
			}
			else
			{
				Main.playerTextures[1, 13] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			}
			if ((player.armor[2].type == ItemID.FamiliarPants || player.armor[12].type == ItemID.FamiliarPants) && !hideLeggings)
			{
				Main.playerTextures[1, 14] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Pants_2_2");
			}
			else
			{
				Main.playerTextures[1, 14] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			}

			Main.playerTextures[2, 0] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Head");
			Main.playerTextures[2, 1] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Eyes_2");
			Main.playerTextures[2, 2] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Eyes");
			Main.playerTextures[2, 3] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Torso");

			if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
			{
				Main.playerTextures[2, 4] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Sleeves_3");
			}
			else
			{
				Main.playerTextures[2, 4] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			}

			Main.playerTextures[2, 5] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Hands");

			if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
			{
				Main.playerTextures[2, 6] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Shirt_3");
			}
			else
			{
				Main.playerTextures[2, 6] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			}

			Main.playerTextures[2, 7] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Arm");

			if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
			{
				Main.playerTextures[2, 8] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Sleeve_3");
			}
			else
			{
				Main.playerTextures[2, 8] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			}

			Main.playerTextures[2, 9] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Hand");
			Main.playerTextures[2, 10] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Legs");

			if ((player.armor[2].type == ItemID.FamiliarPants || player.armor[12].type == ItemID.FamiliarPants) && !hideLeggings)
			{
				Main.playerTextures[2, 11] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Pants_3");
				Main.playerTextures[2, 12] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Shoes_3");
			}
			else
			{
				Main.playerTextures[2, 11] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
				Main.playerTextures[2, 12] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			}
			if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
			{
				Main.playerTextures[2, 13] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Sleeve_3_2");
			}
			else
			{
				Main.playerTextures[2, 13] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			}
			if ((player.armor[2].type == ItemID.FamiliarPants || player.armor[12].type == ItemID.FamiliarPants) && !hideLeggings)
			{
				Main.playerTextures[2, 14] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Pants_3_2");
			}
			else
			{
				Main.playerTextures[2, 14] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			}

			Main.playerTextures[3, 0] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Head");
			Main.playerTextures[3, 1] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Eyes_2");
			Main.playerTextures[3, 2] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Eyes");
			Main.playerTextures[3, 3] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Torso");

			if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
			{
				Main.playerTextures[3, 4] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Sleeves_4");
			}
			else
			{
				Main.playerTextures[3, 4] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			}

			Main.playerTextures[3, 5] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Hands");

			if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
			{
				Main.playerTextures[3, 6] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Shirt_4");
			}
			else
			{
				Main.playerTextures[3, 6] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			}

			Main.playerTextures[3, 7] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Arm");

			if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
			{
				Main.playerTextures[3, 8] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Sleeve_4");
			}
			else
			{
				Main.playerTextures[3, 8] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			}

			Main.playerTextures[3, 9] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Hand");
			Main.playerTextures[3, 10] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Legs");

			if ((player.armor[2].type == ItemID.FamiliarPants || player.armor[12].type == ItemID.FamiliarPants) && !hideLeggings)
			{
				Main.playerTextures[3, 11] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Pants_4");
				Main.playerTextures[3, 12] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Shoes_4");
			}
			else
			{
				Main.playerTextures[3, 11] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
				Main.playerTextures[3, 12] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			}
			if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
			{
				Main.playerTextures[3, 13] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Sleeve_4_2");
			}
			else
			{
				Main.playerTextures[3, 13] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			}
			if ((player.armor[2].type == ItemID.FamiliarPants || player.armor[12].type == ItemID.FamiliarPants) && !hideLeggings)
			{
				Main.playerTextures[3, 14] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Pants_4_2");
			}
			else
			{
				Main.playerTextures[3, 14] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			}

			Main.playerTextures[8, 0] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Head");
			Main.playerTextures[8, 1] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Eyes_2");
			Main.playerTextures[8, 2] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Eyes");
			Main.playerTextures[8, 3] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Torso");

			if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
			{
				Main.playerTextures[8, 4] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Sleeves_9");
			}
			else
			{
				Main.playerTextures[8, 4] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			}

			Main.playerTextures[8, 5] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Hands");

			if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
			{
				Main.playerTextures[8, 6] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Shirt_9");
			}
			else
			{
				Main.playerTextures[8, 6] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			}

			Main.playerTextures[8, 7] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Arm");

			if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
			{
				Main.playerTextures[8, 8] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Sleeve_9");
			}
			else
			{
				Main.playerTextures[8, 8] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			}

			Main.playerTextures[8, 9] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Hand");
			Main.playerTextures[8, 10] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Legs");

			if ((player.armor[2].type == ItemID.FamiliarPants || player.armor[12].type == ItemID.FamiliarPants) && !hideLeggings)
			{
				Main.playerTextures[8, 11] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Pants_9");
				Main.playerTextures[8, 12] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Shoes_9");
			}
			else
			{
				Main.playerTextures[8, 11] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
				Main.playerTextures[8, 12] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			}
			if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
			{
				Main.playerTextures[8, 13] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Sleeve_9_2");
			}
			else
			{
				Main.playerTextures[8, 13] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			}
			if ((player.armor[2].type == ItemID.FamiliarPants || player.armor[12].type == ItemID.FamiliarPants) && !hideLeggings)
			{
				Main.playerTextures[8, 14] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Pants_9_2");
			}
			else
			{
				Main.playerTextures[8, 14] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			}

			Main.playerTextures[4, 0] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Head_Female");
			Main.playerTextures[4, 1] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Eyes_2_Female");
			Main.playerTextures[4, 2] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Eyes_Female");
			Main.playerTextures[4, 3] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Torso_Female");

			if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
			{
				Main.playerTextures[4, 4] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Sleeves_5");
			}
			else
			{
				Main.playerTextures[4, 4] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			}

			Main.playerTextures[4, 5] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Hands_Female");

			if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
			{
				Main.playerTextures[4, 6] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Shirt_5");
			}
			else
			{
				Main.playerTextures[4, 6] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			}

			Main.playerTextures[4, 7] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Arm_Female");

			if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
			{
				Main.playerTextures[4, 8] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Sleeve_5");
			}
			else
			{
				Main.playerTextures[4, 8] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			}

			Main.playerTextures[4, 9] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Hand_Female");
			Main.playerTextures[4, 10] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Legs_Female");

			if ((player.armor[2].type == ItemID.FamiliarPants || player.armor[12].type == ItemID.FamiliarPants) && !hideLeggings)
			{
				Main.playerTextures[4, 11] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Pants_5");
				Main.playerTextures[4, 12] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Shoes_5");
			}
			else
			{
				Main.playerTextures[4, 11] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
				Main.playerTextures[4, 12] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			}
			if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
			{
				Main.playerTextures[4, 13] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Sleeve_5_2");
			}
			else
			{
				Main.playerTextures[4, 13] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			}
			if ((player.armor[2].type == ItemID.FamiliarPants || player.armor[12].type == ItemID.FamiliarPants) && !hideLeggings)
			{
				Main.playerTextures[4, 14] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Pants_5_2");
			}
			else
			{
				Main.playerTextures[4, 14] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			}

			Main.playerTextures[5, 0] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Head_Female");
			Main.playerTextures[5, 1] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Eyes_2_Female");
			Main.playerTextures[5, 2] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Eyes_Female");
			Main.playerTextures[5, 3] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Torso_Female");

			if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
			{
				Main.playerTextures[5, 4] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Sleeves_6");
			}
			else
			{
				Main.playerTextures[5, 4] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			}

			Main.playerTextures[5, 5] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Hands_Female");

			if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
			{
				Main.playerTextures[5, 6] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Shirt_6");
			}
			else
			{
				Main.playerTextures[5, 6] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			}

			Main.playerTextures[5, 7] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Arm_Female");

			if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
			{
				Main.playerTextures[5, 8] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Sleeve_6");
			}
			else
			{
				Main.playerTextures[5, 8] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			}

			Main.playerTextures[5, 9] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Hand_Female");
			Main.playerTextures[5, 10] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Legs_Female");

			if ((player.armor[2].type == ItemID.FamiliarPants || player.armor[12].type == ItemID.FamiliarPants) && !hideLeggings)
			{
				Main.playerTextures[5, 11] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Pants_6");
				Main.playerTextures[5, 12] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Shoes_6");
			}
			else
			{
				Main.playerTextures[5, 11] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
				Main.playerTextures[5, 12] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			}
			if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
			{
				Main.playerTextures[5, 13] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Sleeve_6_2");
			}
			else
			{
				Main.playerTextures[5, 13] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			}
			if ((player.armor[2].type == ItemID.FamiliarPants || player.armor[12].type == ItemID.FamiliarPants) && !hideLeggings)
			{
				Main.playerTextures[5, 14] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Pants_6_2");
			}
			else
			{
				Main.playerTextures[5, 14] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			}

			Main.playerTextures[6, 0] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Head_Female");
			Main.playerTextures[6, 1] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Eyes_2_Female");
			Main.playerTextures[6, 2] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Eyes_Female");
			Main.playerTextures[6, 3] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Torso_Female");

			if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
			{
				Main.playerTextures[6, 4] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Sleeves_7");
			}
			else
			{
				Main.playerTextures[6, 4] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			}

			Main.playerTextures[6, 5] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Hands_Female");

			if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
			{
				Main.playerTextures[6, 6] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Shirt_7");
			}
			else
			{
				Main.playerTextures[6, 6] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			}

			Main.playerTextures[6, 7] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Arm_Female");

			if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
			{
				Main.playerTextures[6, 8] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Sleeve_7");
			}
			else
			{
				Main.playerTextures[6, 8] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			}

			Main.playerTextures[6, 9] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Hand_Female");
			Main.playerTextures[6, 10] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Legs_Female");

			if ((player.armor[2].type == ItemID.FamiliarPants || player.armor[12].type == ItemID.FamiliarPants) && !hideLeggings)
			{
				Main.playerTextures[6, 11] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Pants_7");
				Main.playerTextures[6, 12] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Shoes_7");
			}
			else
			{
				Main.playerTextures[6, 11] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
				Main.playerTextures[6, 12] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			}
			if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
			{
				Main.playerTextures[6, 13] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Sleeve_7_2");
			}
			else
			{
				Main.playerTextures[6, 13] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			}
			if ((player.armor[2].type == ItemID.FamiliarPants || player.armor[12].type == ItemID.FamiliarPants) && !hideLeggings)
			{
				Main.playerTextures[6, 14] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Pants_7_2");
			}
			else
			{
				Main.playerTextures[6, 14] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			}

			Main.playerTextures[7, 0] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Head_Female");
			Main.playerTextures[7, 1] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Eyes_2_Female");
			Main.playerTextures[7, 2] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Eyes_Female");
			Main.playerTextures[7, 3] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Torso_Female");

			if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
			{
				Main.playerTextures[7, 4] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Sleeves_8");
			}
			else
			{
				Main.playerTextures[7, 4] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			}

			Main.playerTextures[7, 5] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Hands_Female");

			if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
			{
				Main.playerTextures[7, 6] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Shirt_8");
			}
			else
			{
				Main.playerTextures[7, 6] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			}

			Main.playerTextures[7, 7] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Arm_Female");

			if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
			{
				Main.playerTextures[7, 8] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Sleeve_8");
			}
			else
			{
				Main.playerTextures[7, 8] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			}

			Main.playerTextures[7, 9] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Hand_Female");
			Main.playerTextures[7, 10] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Legs_Female");

			if ((player.armor[2].type == ItemID.FamiliarPants || player.armor[12].type == ItemID.FamiliarPants) && !hideLeggings)
			{
				Main.playerTextures[7, 11] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Pants_8");
				Main.playerTextures[7, 12] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Shoes_8");
			}
			else
			{
				Main.playerTextures[7, 11] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
				Main.playerTextures[7, 12] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			}
			if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
			{
				Main.playerTextures[7, 13] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Sleeve_8_2");
			}
			else
			{
				Main.playerTextures[7, 13] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			}
			if ((player.armor[2].type == ItemID.FamiliarPants || player.armor[12].type == ItemID.FamiliarPants) && !hideLeggings)
			{
				Main.playerTextures[7, 14] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Pants_8_2");
			}
			else
			{
				Main.playerTextures[7, 14] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			}

			Main.playerTextures[9, 0] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Head_Female");
			Main.playerTextures[9, 1] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Eyes_2_Female");
			Main.playerTextures[9, 2] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Eyes_Female");
			Main.playerTextures[9, 3] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Torso_Female");

			if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
			{
				Main.playerTextures[9, 4] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Sleeves_10");
			}
			else
			{
				Main.playerTextures[9, 4] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			}

			Main.playerTextures[9, 5] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Hands_Female");

			if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
			{
				Main.playerTextures[9, 6] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Shirt_10");
			}
			else
			{
				Main.playerTextures[9, 6] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			}

			Main.playerTextures[9, 7] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Arm_Female");

			if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
			{
				Main.playerTextures[9, 8] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Sleeve_10");
			}
			else
			{
				Main.playerTextures[9, 8] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			}

			Main.playerTextures[9, 9] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Hand_Female");
			Main.playerTextures[9, 10] = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Legs_Female");

			if ((player.armor[2].type == ItemID.FamiliarPants || player.armor[12].type == ItemID.FamiliarPants) && !hideLeggings)
			{
				Main.playerTextures[9, 11] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Pants_10");
				Main.playerTextures[9, 12] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Shoes_10");
			}
			else
			{
				Main.playerTextures[9, 11] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
				Main.playerTextures[9, 12] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			}
			if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
			{
				Main.playerTextures[9, 13] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Sleeve_10_2");
			}
			else
			{
				Main.playerTextures[9, 13] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			}
			if ((player.armor[2].type == ItemID.FamiliarPants || player.armor[12].type == ItemID.FamiliarPants) && !hideLeggings)
			{
				Main.playerTextures[9, 14] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Pants_10_2");
			}
			else
			{
				Main.playerTextures[9, 14] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			}

			Main.playerHairTexture[0] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[1] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[2] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[3] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[4] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[5] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[6] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[7] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[8] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[9] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[10] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[11] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[12] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[13] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[14] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[15] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[16] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[17] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[18] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[19] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[20] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[21] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[22] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[23] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[24] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[25] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[26] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[27] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[28] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[29] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[30] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[31] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[32] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[33] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[34] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[35] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[36] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[37] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[38] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[39] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[40] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[41] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[42] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[43] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[44] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[45] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[46] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[47] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[48] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[49] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[50] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[51] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[51] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[52] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[53] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[54] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[55] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[56] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[57] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[58] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[59] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[60] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[61] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[62] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[63] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[64] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[65] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[66] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[67] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[68] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[69] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[70] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[71] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[72] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[73] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[74] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[75] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[76] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[77] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[78] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[79] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[80] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[81] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[82] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[83] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[84] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[85] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[86] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[87] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[88] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[89] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[90] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[91] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[92] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[93] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[94] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[95] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[96] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[97] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[98] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[99] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[100] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[101] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[102] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[103] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[104] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[105] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[106] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[107] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[108] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[109] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[110] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[111] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[112] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[113] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[114] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[115] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[116] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[117] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[118] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[119] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[120] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[121] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[122] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[123] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[124] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[125] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[126] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[127] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[128] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[129] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[130] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[131] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[132] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairTexture[133] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");

			Main.playerHairAltTexture[0] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[1] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[2] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[3] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[4] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[5] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[6] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[7] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[8] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[9] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[10] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[11] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[12] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[13] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[14] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[15] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[16] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[17] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[18] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[19] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[20] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[21] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[22] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[23] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[24] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[25] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[26] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[27] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[28] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[29] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[30] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[31] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[32] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[33] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[34] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[35] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[36] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[37] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[38] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[39] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[40] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[41] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[42] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[43] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[44] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[45] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[46] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[47] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[48] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[49] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[50] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[51] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[52] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[53] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[54] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[55] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[56] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[57] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[58] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[59] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[60] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[61] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[62] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[63] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[64] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[65] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[66] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[67] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[68] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[69] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[70] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[71] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[72] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[73] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[74] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[75] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[76] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[77] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[78] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[79] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[80] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[81] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[82] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[83] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[84] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[85] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[86] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[87] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[88] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[89] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[90] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[91] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[92] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[93] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[94] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[95] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[96] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[97] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[98] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[99] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[100] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[101] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[102] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[103] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[104] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[105] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[106] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[107] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[108] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[109] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[110] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[111] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[112] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[113] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[114] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[115] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[116] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[117] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[118] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[119] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[120] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[121] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[122] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[123] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[124] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[125] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[126] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[127] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[128] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[129] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[130] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[131] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[132] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
			Main.playerHairAltTexture[133] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");

			Main.ghostTexture = ModContent.GetTexture("ExabytesProtogenMod/Content/RaceTextures/Protogen/Protogen_Ghost");
		}

		/*
		Player sheets are split into 15 different sections ([x, 0] to [x, 14]) and repeated 10 times for each default clothing style and each gender. There are a total of 10 repeats, 5 of which are used for male and 5 of which are used for female.
		Clothing sheets are put into an if/else statement to detect whether The familiar clothing is equipped onto The player. If The player is not wearing familiar clothing, The respective clothing sheets will be set to a blank sheet. Otherwise, they will appear as clothing.

		Main.playerTextures[0, 0]: The head sheet for clothing style 1 (MALE)
		Main.playerTextures[0, 1]: The eye whites sheet for clothing style 1 (MALE)
		Main.playerTextures[0, 2]: The eye iris sheet for clothing style 1 (MALE)
		Main.playerTextures[0, 3]: The torso sheet for clothing style 1 (MALE)
		Main.playerTextures[0, 4]: The clothing sleeves sheet for clothing style 1 (MALE)
		Main.playerTextures[0, 5]: The hands sheet for clothing style 1 (MALE)
		Main.playerTextures[0, 6]: The clothing shirt sheet for clothing style 1 (MALE)
		Main.playerTextures[0, 7]: The arm sheet for clothing style 1 (MALE)
		Main.playerTextures[0, 8]: The clothing singular sleeve sheet for clothing style 1 (MALE)
		Main.playerTextures[0, 9]: The singular hand sheet for clothing style 1 (MALE)
		Main.playerTextures[0, 10]: The legs sheet for clothing style 1 (MALE)
		Main.playerTextures[0, 11]: The clothing pants sheet for clothing style 1 (MALE)
		Main.playerTextures[0, 12]: The clothing shoes sheet for clothing style 1 (MALE)
		Main.playerTextures[0, 13]: An extra clothing layer that is used for some clothing styles (usually as a secondary sleeve color or a dress), otherwise being blank (MALE)
		Main.playerTextures[0, 14]: An extra clothing layer that is used for some clothing styles (usually as a secondary sleeve color or a dress), otherwise being blank (MALE)

		Main.playerTextures[1, 0]: The head sheet for clothing style 2 (MALE)
		Main.playerTextures[1, 1]: The eye whites sheet for clothing style 2 (MALE)
		Main.playerTextures[1, 2]: The eye iris sheet for clothing style 2 (MALE)
		Main.playerTextures[1, 3]: The torso sheet for clothing style 2 (MALE)
		Main.playerTextures[1, 4]: The clothing sleeves sheet for clothing style 2 (MALE)
		Main.playerTextures[1, 5]: The hands sheet for clothing style 2 (MALE)
		Main.playerTextures[1, 6]: The clothing shirt sheet for clothing style 2 (MALE)
		Main.playerTextures[1, 7]: The arm sheet for clothing style 2 (MALE)
		Main.playerTextures[1, 8]: The clothing singular sleeve sheet for clothing style 2 (MALE)
		Main.playerTextures[1, 9]: The singular hand sheet for clothing style 2 (MALE)
		Main.playerTextures[1, 10]: The legs sheet for clothing style 2 (MALE)
		Main.playerTextures[1, 11]: The clothing pants sheet for clothing style 2 (MALE)
		Main.playerTextures[1, 12]: The clothing shoes sheet for clothing style 2 (MALE)
		Main.playerTextures[1, 13]: An extra clothing layer that is used for some clothing styles (usually as a secondary sleeve color or a dress), otherwise being blank (MALE)
		Main.playerTextures[1, 14]: An extra clothing layer that is used for some clothing styles (usually as a secondary sleeve color or a dress), otherwise being blank (MALE)


		Main.playerTextures[2, 0]: The head sheet for clothing style 3 (MALE)
		Main.playerTextures[2, 1]: The eye whites sheet for clothing style 3 (MALE)
		Main.playerTextures[2, 2]: The eye iris sheet for clothing style 3 (MALE)
		Main.playerTextures[2, 3]: The torso sheet for clothing style 3 (MALE)
		Main.playerTextures[2, 4]: The clothing sleeves sheet for clothing style 3 (MALE)
		Main.playerTextures[2, 5]: The hands sheet for clothing style 3 (MALE)
		Main.playerTextures[2, 6]: The clothing shirt sheet for clothing style 3 (MALE)
		Main.playerTextures[2, 7]: The arm sheet for clothing style 3 (MALE)
		Main.playerTextures[2, 8]: The clothing singular sleeve sheet for clothing style 3 (MALE)
		Main.playerTextures[2, 9]: The singular hand sheet for clothing style 3 (MALE)
		Main.playerTextures[2, 10]: The legs sheet for clothing style 3 (MALE)
		Main.playerTextures[2, 11]: The clothing pants sheet for clothing style 3 (MALE)
		Main.playerTextures[2, 12]: The clothing shoes sheet for clothing style 3 (MALE)
		Main.playerTextures[2, 13]: An extra clothing layer that is used for some clothing styles (usually as a secondary sleeve color or a dress), otherwise being blank (MALE)
		Main.playerTextures[2, 14]: An extra clothing layer that is used for some clothing styles (usually as a secondary sleeve color or a dress), otherwise being blank (MALE)

		Main.playerTextures[3, 0]: The head sheet for clothing style 4 (MALE)
		Main.playerTextures[3, 1]: The eye whites sheet for clothing style 4 (MALE)
		Main.playerTextures[3, 2]: The eye iris sheet for clothing style 4 (MALE)
		Main.playerTextures[3, 3]: The torso sheet for clothing style 4 (MALE)
		Main.playerTextures[3, 4]: The clothing sleeves sheet for clothing style 4 (MALE)
		Main.playerTextures[3, 5]: The hands sheet for clothing style 4 (MALE)
		Main.playerTextures[3, 6]: The clothing shirt sheet for clothing style 4 (MALE)
		Main.playerTextures[3, 7]: The arm sheet for clothing style 4 (MALE)
		Main.playerTextures[3, 8]: The clothing singular sleeve sheet for clothing style 4 (MALE)
		Main.playerTextures[3, 9]: The singular hand sheet for clothing style 4 (MALE)
		Main.playerTextures[3, 10]: The legs sheet for clothing style 4 (MALE)
		Main.playerTextures[3, 11]: The clothing pants sheet for clothing style 4 (MALE)
		Main.playerTextures[3, 12]: The clothing shoes sheet for clothing style 4 (MALE)
		Main.playerTextures[3, 13]: An extra clothing layer that is used for some clothing styles (usually as a secondary sleeve color or a dress), otherwise being blank (MALE)
		Main.playerTextures[3, 14]: An extra clothing layer that is used for some clothing styles (usually as a secondary sleeve color or a dress), otherwise being blank (MALE)

		Main.playerTextures[8, 0]: The head sheet for clothing style 5 (MALE)
		Main.playerTextures[8, 1]: The eye whites sheet for clothing style 5 (MALE)
		Main.playerTextures[8, 2]: The eye iris sheet for clothing style 5 (MALE)
		Main.playerTextures[8, 3]: The torso sheet for clothing style 5 (MALE)
		Main.playerTextures[8, 4]: The clothing sleeves sheet for clothing style 5 (MALE)
		Main.playerTextures[8, 5]: The hands sheet for clothing style 5 (MALE)
		Main.playerTextures[8, 6]: The clothing shirt sheet for clothing style 5 (MALE)
		Main.playerTextures[8, 7]: The arm sheet for clothing style 5 (MALE)
		Main.playerTextures[8, 8]: The clothing singular sleeve sheet for clothing style 5 (MALE)
		Main.playerTextures[8, 9]: The singular hand sheet for clothing style 5 (MALE)
		Main.playerTextures[8, 10]: The legs sheet for clothing style 5 (MALE)
		Main.playerTextures[8, 11]: The clothing pants sheet for clothing style 5 (MALE)
		Main.playerTextures[8, 12]: The clothing shoes sheet for clothing style 5 (MALE)
		Main.playerTextures[8, 13]: An extra clothing layer that is used for some clothing styles (usually as a secondary sleeve color or a dress), otherwise being blank (MALE)
		Main.playerTextures[8, 14]: An extra clothing layer that is used for some clothing styles (usually as a secondary sleeve color or a dress), otherwise being blank (MALE)

		Main.playerTextures[4, 0]: The head sheet for clothing style 1 (FEMALE)
		Main.playerTextures[4, 1]: The eye whites sheet for clothing style 1 (FEMALE)
		Main.playerTextures[4, 2]: The eye iris sheet for clothing style 1 (FEMALE)
		Main.playerTextures[4, 3]: The torso sheet for clothing style 1 (FEMALE)
		Main.playerTextures[4, 4]: The clothing sleeves sheet for clothing style 1 (FEMALE)
		Main.playerTextures[4, 5]: The hands sheet for clothing style 1 (FEMALE)
		Main.playerTextures[4, 6]: The clothing shirt sheet for clothing style 1 (FEMALE)
		Main.playerTextures[4, 7]: The arm sheet for clothing style 1 (FEMALE)
		Main.playerTextures[4, 8]: The clothing singular sleeve sheet for clothing style 1 (FEMALE)
		Main.playerTextures[4, 9]: The singular hand sheet for clothing style 1 (FEMALE)
		Main.playerTextures[4, 10]: The legs sheet for clothing style 1 (FEMALE)
		Main.playerTextures[4, 11]: The clothing pants sheet for clothing style 1 (FEMALE)
		Main.playerTextures[4, 12]: The clothing shoes sheet for clothing style 1 (FEMALE)
		Main.playerTextures[4, 13]: An extra clothing layer that is used for some clothing styles (usually as a secondary sleeve color or a dress), otherwise being blank (FEMALE)
		Main.playerTextures[4, 14]: An extra clothing layer that is used for some clothing styles (usually as a secondary sleeve color or a dress), otherwise being blank (FEMALE)

		Main.playerTextures[5, 0]: The head sheet for clothing style 2 (FEMALE)
		Main.playerTextures[5, 1]: The eye whites sheet for clothing style 2 (FEMALE)
		Main.playerTextures[5, 2]: The eye iris sheet for clothing style 2 (FEMALE)
		Main.playerTextures[5, 3]: The torso sheet for clothing style 2 (FEMALE)
		Main.playerTextures[5, 4]: The clothing sleeves sheet for clothing style 2 (FEMALE)
		Main.playerTextures[5, 5]: The hands sheet for clothing style 2 (FEMALE)
		Main.playerTextures[5, 6]: The clothing shirt sheet for clothing style 2 (FEMALE)
		Main.playerTextures[5, 7]: The arm sheet for clothing style 2 (FEMALE)
		Main.playerTextures[5, 8]: The clothing singular sleeve sheet for clothing style 2 (FEMALE)
		Main.playerTextures[5, 9]: The singular hand sheet for clothing style 2 (FEMALE)
		Main.playerTextures[5, 10]: The legs sheet for clothing style 2 (FEMALE)
		Main.playerTextures[5, 11]: The clothing pants sheet for clothing style 2 (FEMALE)
		Main.playerTextures[5, 12]: The clothing shoes sheet for clothing style 2 (FEMALE)
		Main.playerTextures[5, 13]: An extra clothing layer that is used for some clothing styles (usually as a secondary sleeve color or a dress), otherwise being blank (FEMALE)
		Main.playerTextures[5, 14]: An extra clothing layer that is used for some clothing styles (usually as a secondary sleeve color or a dress), otherwise being blank (FEMALE)


		Main.playerTextures[6, 0]: The head sheet for clothing style 3 (FEMALE)
		Main.playerTextures[6, 1]: The eye whites sheet for clothing style 3 (FEMALE)
		Main.playerTextures[6, 2]: The eye iris sheet for clothing style 3 (FEMALE)
		Main.playerTextures[6, 3]: The torso sheet for clothing style 3 (FEMALE)
		Main.playerTextures[6, 4]: The clothing sleeves sheet for clothing style 3 (FEMALE)
		Main.playerTextures[6, 5]: The hands sheet for clothing style 3 (FEMALE)
		Main.playerTextures[6, 6]: The clothing shirt sheet for clothing style 3 (FEMALE)
		Main.playerTextures[6, 7]: The arm sheet for clothing style 3 (FEMALE)
		Main.playerTextures[6, 8]: The clothing singular sleeve sheet for clothing style 3 (FEMALE)
		Main.playerTextures[6, 9]: The singular hand sheet for clothing style 3 (FEMALE)
		Main.playerTextures[6, 10]: The legs sheet for clothing style 3 (FEMALE)
		Main.playerTextures[6, 11]: The clothing pants sheet for clothing style 3 (FEMALE)
		Main.playerTextures[6, 12]: The clothing shoes sheet for clothing style 3 (FEMALE)
		Main.playerTextures[6, 13]: An extra clothing layer that is used for some clothing styles (usually as a secondary sleeve color or a dress), otherwise being blank (FEMALE)
		Main.playerTextures[6, 14]: An extra clothing layer that is used for some clothing styles (usually as a secondary sleeve color or a dress), otherwise being blank (FEMALE)

		Main.playerTextures[7, 0]: The head sheet for clothing style 4 (FEMALE)
		Main.playerTextures[7, 1]: The eye whites sheet for clothing style 4 (FEMALE)
		Main.playerTextures[7, 2]: The eye iris sheet for clothing style 4 (FEMALE)
		Main.playerTextures[7, 3]: The torso sheet for clothing style 4 (FEMALE)
		Main.playerTextures[7, 4]: The clothing sleeves sheet for clothing style 4 (FEMALE)
		Main.playerTextures[7, 5]: The hands sheet for clothing style 4 (FEMALE)
		Main.playerTextures[7, 6]: The clothing shirt sheet for clothing style 4 (FEMALE)
		Main.playerTextures[7, 7]: The arm sheet for clothing style 4 (FEMALE)
		Main.playerTextures[7, 8]: The clothing singular sleeve sheet for clothing style 4 (FEMALE)
		Main.playerTextures[7, 9]: The singular hand sheet for clothing style 4 (FEMALE)
		Main.playerTextures[7, 10]: The legs sheet for clothing style 4 (FEMALE)
		Main.playerTextures[7, 11]: The clothing pants sheet for clothing style 4 (FEMALE)
		Main.playerTextures[7, 12]: The clothing shoes sheet for clothing style 4 (FEMALE)
		Main.playerTextures[7, 13]: An extra clothing layer that is used for some clothing styles (usually as a secondary sleeve color or a dress), otherwise being blank (FEMALE)
		Main.playerTextures[7, 14]: An extra clothing layer that is used for some clothing styles (usually as a secondary sleeve color or a dress), otherwise being blank (FEMALE)

		Main.playerTextures[9, 0]: The head sheet for clothing style 5 (FEMALE)
		Main.playerTextures[9, 1]: The eye whites sheet for clothing style 5 (FEMALE)
		Main.playerTextures[9, 2]: The eye iris sheet for clothing style 5 (FEMALE)
		Main.playerTextures[9, 3]: The torso sheet for clothing style 5 (FEMALE)
		Main.playerTextures[9, 4]: The clothing sleeves sheet for clothing style 5 (FEMALE)
		Main.playerTextures[9, 5]: The hands sheet for clothing style 5 (FEMALE)
		Main.playerTextures[9, 6]: The clothing shirt sheet for clothing style 5 (FEMALE)
		Main.playerTextures[9, 7]: The arm sheet for clothing style 5 (FEMALE)
		Main.playerTextures[9, 8]: The clothing singular sleeve sheet for clothing style 5 (FEMALE)
		Main.playerTextures[9, 9]: The singular hand sheet for clothing style 5 (FEMALE)
		Main.playerTextures[9, 10]: The legs sheet for clothing style 5 (FEMALE)
		Main.playerTextures[9, 11]: The clothing pants sheet for clothing style 5 (FEMALE)
		Main.playerTextures[9, 12]: The clothing shoes sheet for clothing style 5 (FEMALE)
		Main.playerTextures[9, 13]: An extra clothing layer that is used for some clothing styles (usually as a secondary sleeve color or a dress), otherwise being blank (FEMALE)
		Main.playerTextures[9, 14]: An extra clothing layer that is used for some clothing styles (usually as a secondary sleeve color or a dress), otherwise being blank (FEMALE)
		*/
	}
}
