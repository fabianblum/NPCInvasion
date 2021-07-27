namespace AtomicTorch.CBND.CoreMod.Items.Weapons.MobWeapons
{
    using System.Collections.Generic;
    using AtomicTorch.CBND.CoreMod.CharacterStatusEffects;
    using AtomicTorch.CBND.CoreMod.CharacterStatusEffects.Debuffs;
    using AtomicTorch.CBND.CoreMod.Items.Ammo;
    using AtomicTorch.CBND.CoreMod.Systems.Weapons;
    using AtomicTorch.CBND.GameApi.Data.Characters;
    using AtomicTorch.CBND.GameApi.Data.Weapons;
    using AtomicTorch.CBND.GameApi.Data.World;
    using AtomicTorch.CBND.CoreMod.SoundPresets;
    using AtomicTorch.GameEngine.Common.Helpers;

    public class ItemWeaponMobHandgun : ProtoItemMobNPCWeaponRanged
    {
        public override ushort AmmoCapacity => 0;

        public override ushort VirtualAmmoCapacity => 10;

        public override double AmmoReloadDuration => 2.0; // lower

        public override double CharacterAnimationAimingRecoilDuration => 0.3;

        public override double DamageApplyDelay => 0.3;

        public override double DamageMultiplier => 1.1;

        public override double FireAnimationDuration => 0.1;
		
		public override uint DurabilityMax => 0;

        public override bool IsLoopedAttackAnimation => false;

        public override double FireInterval => 0.7;
		
		public override double SpecialEffectProbability => 0.05;

        public override string Name => "Handgun";

        public override double ReadyDelayDuration => WeaponReadyDelays.ConventionalPistols;

        public override string CharacterAnimationAimingName => "WeaponPistolAiming";

        public override string CharacterAnimationAimingRecoilName => "WeaponPistolShooting";

        public override void SharedOnHit(
            WeaponFinalCache weaponCache,
            IWorldObject damagedObject,
            double damage,
            WeaponHitData hitData,
            out bool isDamageStop)
        {
            base.SharedOnHit(weaponCache,
                             damagedObject,
                             damage,
                             hitData,
                             out isDamageStop);
        }
        public override double SharedUpdateAndGetFirePatternCurrentSpreadAngleDeg(WeaponState state)
        {
            // angle variation within x degrees
            return 25 * (RandomHelper.NextDouble() - 0.5);
        }

        protected override WeaponFirePatternPreset PrepareFirePatternPreset()
        {
            return new(
                initialSequence: new[] { 0.0, 0.0, 0.5, 0.5 },
                cycledSequence: new[] { 1.0, 0.5 });
        }

        protected override WeaponFireTracePreset PrepareFireTracePreset()
        {
            return WeaponFireTracePresets.Firearm;
        }

        protected override void PrepareMuzzleFlashDescription(MuzzleFlashDescription description)
        {
            description.Set(MuzzleFlashPresets.ModernHandgun)
                       .Set(textureScreenOffset: (-13, 9));
        }

        protected override void PrepareProtoWeaponRanged(
            out IEnumerable<IProtoItemAmmo> compatibleAmmoProtos,
            ref DamageDescription overrideDamageDescription)
        {
            compatibleAmmoProtos = GetAmmoOfType<IAmmoCaliber10mm>();

            var damageDistribution = new DamageDistribution()
                                     .Set(DamageType.Kinetic, 1);

            overrideDamageDescription = new DamageDescription(
                damageValue: 9,
                armorPiercingCoef: 0.2,
                finalDamageMultiplier: 1,
                rangeMax: 10,
                damageDistribution: damageDistribution);
        }

        protected override void ServerOnSpecialEffect(ICharacter damagedCharacter, double damage)
        {
            if (RandomHelper.RollWithProbability(0.07))
            {
                damagedCharacter.ServerAddStatusEffect<StatusEffectBleeding>(intensity: 0.1);
            }

            if (RandomHelper.RollWithProbability(0.05))
            {
				damagedCharacter.ServerAddStatusEffect<StatusEffectPain>(intensity: 0.08);
            }
        }

            protected override ReadOnlySoundPreset<WeaponSound> PrepareSoundPresetWeapon()
        {
            return WeaponsSoundPresets.WeaponRangedPistol;
        }

    }
}