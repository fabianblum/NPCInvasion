namespace AtomicTorch.CBND.CoreMod.Characters.Mobs
{
    using AtomicTorch.CBND.CoreMod.CharacterSkeletons;
    using AtomicTorch.CBND.CoreMod.Items.Food;
    using AtomicTorch.CBND.CoreMod.Items.Generic;
	using AtomicTorch.CBND.CoreMod.Items.Medical;
	using AtomicTorch.CBND.CoreMod.Items.Equipment;
	using AtomicTorch.CBND.CoreMod.Items.Equipment.Assault;
	using AtomicTorch.CBND.CoreMod.Items.Devices;
    using AtomicTorch.CBND.CoreMod.Items.Weapons.MobWeapons;
    using AtomicTorch.CBND.CoreMod.Items.Weapons.Ranged;
    using AtomicTorch.CBND.CoreMod.Items.Ammo;
	using AtomicTorch.CBND.GameApi.Data.Characters;
    using AtomicTorch.CBND.CoreMod.Objects;
    using AtomicTorch.CBND.CoreMod.Skills;
    using AtomicTorch.CBND.CoreMod.SoundPresets;
    using AtomicTorch.CBND.CoreMod.Stats;
    using AtomicTorch.CBND.CoreMod.Systems.Droplists;
    using AtomicTorch.CBND.GameApi.Data.World;


    public class MobNPC_BA_Striker : ProtoCharacterNPCBA
    {
		public override string Name => "Striker";
		
		public override ObjectMaterial ObjectMaterial => ObjectMaterial.HardTissues;
        
		public override float CharacterWorldHeight => 1.5f;
		
		public override float CharacterWorldWeaponOffsetRanged => 0.4f;
        
        public override double StatMoveSpeed => 3.2;

        public override double StatMoveSpeedRunMultiplier => 1.3;
		
		public override bool AiIsRunAwayFromHeavyVehicles => false;
		
		public override double MobKillExperienceMultiplier => 1.0;
        
		public override double StatDefaultHealthMax => 320;
		
		public override double StatHealthRegenerationPerSecond => 10.0 / 15.0; // Default: 10/60 10 health points per minute
		
        public override bool IsAvailableInCompletionist => false;

        public override double RetreatDistance => 5;

        public override double EnemyToCloseDistance => 6;

        public override double EnemyToFarDistance => 15;

        public override bool RetreatWhenReloading => true;

        public override double RetreatingHealthPercentage => 50.0;

        protected override void FillDefaultEffects(Effects effects)
        {
            base.FillDefaultEffects(effects);

            effects.AddValue(this, StatName.DefenseImpact,     0.7);
            effects.AddValue(this, StatName.DefenseKinetic,    0.9);
			effects.AddValue(this, StatName.DefenseExplosion, 1.0);
			effects.AddValue(this, StatName.DefenseChemical,   0.3);

        }
		
	   protected override void PrepareProtoCharacterMob(
            out ProtoCharacterSkeleton skeleton,
            ref double scale,
            DropItemsList lootDroplist)
        {
            skeleton = GetProtoEntity<CharacterSkeletons.NPC_BA_Striker>();

            // primary loot
			lootDroplist.Add(
				new DropItemsList(outputs: 1, outputsRandom: 1)
				
					.Add<ItemEnergyTablets>(count: 3, countRandom: 2, weight: 4)
					.Add<ItemPeredozin>(count: 1, weight: 3)
					.Add<ItemMedkit>(count: 1,	countRandom: 1,	weight: 2)
					.Add<ItemStimpack>(count: 1, weight: 1)
					.Add<ItemAmmoGrenadeHE>(count: 2, countRandom: 6, weight: 8));
					
			// extra loot from skill
            lootDroplist.Add(
                condition: SkillSearching.ServerRollExtraLoot,
                nestedList:
                new DropItemsList(outputs: 1, outputsRandom: 1)
                
					.Add<ItemAssaultArmor>(count: 1, probability: 1 / 14.0)
					.Add<ItemAssaultHelmet>(count: 1, probability: 1 / 15.0)
					.Add<ItemGrenadeLauncher>(count: 1, probability: 1 / 16.0)
					.Add<ItemPowerBankLarge>(count: 1, probability: 1 / 13.0));
        }

        protected override void ServerInitializeCharacterMob(ServerInitializeData data)
        {
            base.ServerInitializeCharacterMob(data);

            var weaponProto = GetProtoEntity<ItemWeaponMobGrenadeLauncher>();
            data.PrivateState.WeaponState.SharedSetWeaponProtoOnly(weaponProto);
            data.PublicState.SharedSetCurrentWeaponProtoOnly(weaponProto);

        }

		protected override void ServerOnAggro(ICharacter characterMob, ICharacter characterToAggro)
		{
            // cannot auto-aggro
        }

	}
}