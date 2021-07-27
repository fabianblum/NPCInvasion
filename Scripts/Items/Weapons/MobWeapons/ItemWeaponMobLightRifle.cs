namespace AtomicTorch.CBND.CoreMod.Items.Weapons.MobWeapons
{
    using System.Collections.Generic;
    using AtomicTorch.CBND.CoreMod.CharacterStatusEffects;
    using AtomicTorch.CBND.CoreMod.CharacterStatusEffects.Debuffs;
    using AtomicTorch.CBND.CoreMod.Items.Ammo;
    using AtomicTorch.CBND.CoreMod.Systems.Weapons;
    using AtomicTorch.CBND.GameApi.Data.Weapons;
    using AtomicTorch.CBND.GameApi.Data.World;
    using AtomicTorch.CBND.CoreMod.SoundPresets;
    using AtomicTorch.GameEngine.Common.Helpers;

    public class ItemWeaponMobLightRifle : ProtoItemMobNPCWeaponRanged
    {
        public override ushort AmmoCapacity => 0;

        public override ushort VirtualAmmoCapacity => 10;

        public override double DamageApplyDelay => 0.1;

        public override double AmmoReloadDuration => 2.0; // lower

        public override double CharacterAnimationAimingRecoilDuration => 0.4;

        public override double CharacterAnimationAimingRecoilPower => 1.1;

        public override double FireAnimationDuration => 0.6;

        public override double FireInterval => 1;

        public override string Name => "Light Rifle";

        public override string CharacterAnimationAimingName => "WeaponRifleAiming";
		
		public override double SpecialEffectProbability => 0.1;


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
            return 15 * (RandomHelper.NextDouble() - 0.5);
        }


        protected override WeaponFirePatternPreset PrepareFirePatternPreset()
        {
            return new WeaponFirePatternPreset(
                initialSequence: new[] { 0.0, 0.3, 0.4 },
                cycledSequence: new[] { 0.5, 0.3, 0.4, 0.5, 0.4 });
        }

        protected override WeaponFireTracePreset PrepareFireTracePreset()
        {
            return WeaponFireTracePresets.HeavySniper;
        }

        protected override void PrepareMuzzleFlashDescription(MuzzleFlashDescription description)
        {
            description.Set(MuzzleFlashPresets.ModernRifle)
                       .Set(textureScreenOffset: (42, 6));
        }

        protected override void PrepareProtoWeaponRanged(
            out IEnumerable<IProtoItemAmmo> compatibleAmmoProtos,
            ref DamageDescription overrideDamageDescription)
        {
            compatibleAmmoProtos = null;

            var damageDistribution = new DamageDistribution()
                                     .Set(DamageType.Kinetic, 1);

            overrideDamageDescription = new DamageDescription(
                damageValue: 6,
                armorPiercingCoef: 0.6,
                finalDamageMultiplier: 1,
                rangeMax: 13.5,
                damageDistribution: damageDistribution);
        }

        protected override ReadOnlySoundPreset<WeaponSound> PrepareSoundPresetWeapon()
        {
            return WeaponsSoundPresets.WeaponRangedLightRifle;
        }

    }
}