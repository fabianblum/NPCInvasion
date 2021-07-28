namespace AtomicTorch.CBND.CoreMod.Characters.Mobs
{
    using AtomicTorch.CBND.CoreMod.CharacterSkeletons;
    using AtomicTorch.CBND.CoreMod.Items.Food;
    using AtomicTorch.CBND.CoreMod.Items.Generic;
	using AtomicTorch.CBND.CoreMod.Items.Medical;
	using AtomicTorch.CBND.CoreMod.Items.Equipment;
    using AtomicTorch.CBND.CoreMod.Items.Devices;
    using AtomicTorch.CBND.CoreMod.Items.Weapons.MobWeapons;
    using AtomicTorch.CBND.CoreMod.Items.Weapons.Ranged;
    using AtomicTorch.CBND.CoreMod.Items.Weapons.Melee;
    using AtomicTorch.CBND.CoreMod.Items.Ammo;
    using AtomicTorch.CBND.CoreMod.Objects;
    using AtomicTorch.CBND.CoreMod.Skills;
    using AtomicTorch.CBND.CoreMod.SoundPresets;
    using AtomicTorch.CBND.CoreMod.Stats;
    using AtomicTorch.CBND.CoreMod.Technologies;
    using AtomicTorch.CBND.CoreMod.Systems.Droplists;
	using AtomicTorch.CBND.GameApi.Data.World;
	using AtomicTorch.CBND.GameApi.Data.Characters;
	using AtomicTorch.CBND.CoreMod.Systems.ServerTimers;


    public class MobNPC_BA_Cop : ProtoCharacterNPCBA
    {
		public override string Name => "Cop";
		
		public override ObjectMaterial ObjectMaterial => ObjectMaterial.SoftTissues;
        
		public override float CharacterWorldHeight => 1.5f;
		
		public override float CharacterWorldWeaponOffsetRanged => 0.4f;
        
        public override double StatMoveSpeed => 2.4;

        public override double StatMoveSpeedRunMultiplier => 2.2;
		
		public override bool AiIsRunAwayFromHeavyVehicles => true;
		
		public override double MobKillExperienceMultiplier => 0.8;
        
		public override double StatDefaultHealthMax => 180;
		
		public override double StatHealthRegenerationPerSecond => 10.0 / 60.0; // Default: 10/60 10 health points per minute
		
        public override bool IsAvailableInCompletionist => false;

        public override double RetreatDistance => 8;

        public override double EnemyToCloseDistance => 8;

        public override double EnemyToFarDistance => 14;

        public override bool RetreatWhenReloading => true;

        public override double RetreatingHealthPercentage => 50.0;


        protected override void FillDefaultEffects(Effects effects)
        {
            base.FillDefaultEffects(effects);

            effects.AddValue(this, StatName.DefenseImpact,     0.3);
            effects.AddValue(this, StatName.DefenseKinetic,    0.4);
			effects.AddValue(this, StatName.DefenseExplosion, 0.2);
			effects.AddValue(this, StatName.DefenseChemical,   0.1);

        }
		
	    protected override void PrepareProtoCharacterMob(
            out ProtoCharacterSkeleton skeleton,
            ref double scale,
            DropItemsList lootDroplist)
        {
            skeleton = GetProtoEntity<CharacterSkeletons.NPC_BA_Cop>();

            // primary loot
			lootDroplist.Add(
				new DropItemsList(outputs: 1, outputsRandom: 1)
						
						.Add<ItemAmmo10mmHollowPoint>(count: 12, countRandom: 24, weight: 15)
						.Add<ItemCigarettes>(count: 1, countRandom: 1, weight: 6)
						.Add<ItemPainkiller>(count: 1, countRandom: 1, weight: 5)
						.Add<ItemSalami>(count: 1, weight: 1));
					
			// extra loot from skill
            lootDroplist.Add(
                condition: SkillSearching.ServerRollExtraLoot,
                nestedList:
                new DropItemsList(outputs: 1, outputsRandom: 1)

					.Add<ItemHandgun10mm>(count: 1, weight: 1)
					.Add<ItemStunBaton>(count: 1, probability: 1 / 5.0));
        }

        protected override void ServerInitializeCharacterMob(ServerInitializeData data)
        {
            base.ServerInitializeCharacterMob(data);

            var weaponProto = GetProtoEntity<ItemWeaponMobHandgun>();
            data.PrivateState.WeaponState.SharedSetWeaponProtoOnly(weaponProto);
            data.PublicState.SharedSetCurrentWeaponProtoOnly(weaponProto);
        }
	}
}