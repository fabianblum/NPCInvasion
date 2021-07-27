namespace AtomicTorch.CBND.CoreMod.Zones
{
    using System;
    using AtomicTorch.CBND.CoreMod.Characters.Mobs;
    using AtomicTorch.CBND.CoreMod.Triggers;

    public class SpawnMobsSoldiers : ProtoZoneSpawnScript
    {
        protected override double MaxSpawnAttemptsMultiplier => 10;

        protected override void PrepareZoneSpawnScript(Triggers triggers, SpawnList spawnList)
        {
            triggers
                .Add(GetTrigger<TriggerWorldInit>())
                .Add(GetTrigger<TriggerTimeInterval>().ConfigureForSpawn(SpawnRuinsConstants.SpawnInterval));

            var NPC_BA_Sniper = spawnList.CreatePreset(interval: 15, padding: 0.5)
                                  .AddExact<MobNPC_BA_Sniper>(weight: 0.5)
                                  .SetCustomPaddingWithSelf(24);

            var NPC_BA_Specialist = spawnList.CreatePreset(interval: 15, padding: 0.5)
                                   .AddExact<MobNPC_BA_Specialist>(weight:1.0)
                                   .SetCustomPaddingWith(NPC_BA_Sniper, 16)
                                   .SetCustomPaddingWithSelf(16);

            var NPC_BA_Cop = spawnList.CreatePreset(interval: 15, padding: 0.5)
                                   .AddExact<MobNPC_BA_Cop>(weight: 1.0)
                                   .SetCustomPaddingWith(NPC_BA_Sniper, 18)
                                   .SetCustomPaddingWith(NPC_BA_Specialist, 18)
                                   .SetCustomPaddingWithSelf(20);

            var NPC_BA_Shotgunner = spawnList.CreatePreset(interval: 15, padding: 0.5)
                                   .AddExact<MobNPC_BA_Shotgunner>(weight: 1.0)
                                   .SetCustomPaddingWith(NPC_BA_Sniper, 20)
                                   .SetCustomPaddingWith(NPC_BA_Specialist, 20)
								   .SetCustomPaddingWith(NPC_BA_Cop, 12)
                                   .SetCustomPaddingWithSelf(20);
        }
    }
}