namespace AtomicTorch.CBND.CoreMod.Zones
{
    using AtomicTorch.CBND.CoreMod.Characters.Mobs;
    using AtomicTorch.CBND.CoreMod.Triggers;

    public class SpawnMobsRuinsNormal : ProtoZoneSpawnScript
    {
        protected override double MaxSpawnAttemptsMultiplier => 10;

        protected override void PrepareZoneSpawnScript(Triggers triggers, SpawnList spawnList)
        {
            triggers
                .Add(GetTrigger<TriggerWorldInit>())
                .Add(GetTrigger<TriggerTimeInterval>().ConfigureForSpawn(SpawnRuinsConstants.SpawnInterval));

            var lizard = spawnList.CreatePreset(interval: 15, padding: 0.5)
                                  .AddExact<MobCloakedLizard>(weight: 0.5)
                                  .SetCustomPaddingWithSelf(18);

            var crawler = spawnList.CreatePreset(interval: 15, padding: 0.5)
                                   .AddExact<MobCrawler>(weight:0.5)
                                   .SetCustomPaddingWith(lizard, 18)
                                   .SetCustomPaddingWithSelf(18);

            var scorpion = spawnList.CreatePreset(interval: 20, padding: 0.5)
                                    .AddExact<MobScorpion>(weight: 0.5)
                                    .SetCustomPaddingWith(lizard,  18)
                                    .SetCustomPaddingWith(crawler, 18)
                                    .SetCustomPaddingWithSelf(18);

            // spawn only few mutants in radtowns and central town to make a rare surprise
            var mutants = spawnList.CreatePreset(interval: 80, padding: 0.5, useSectorDensity: false)
                                   .AddExact<MobMutantBoar>()
                                   .AddExact<MobMutantHyena>()
                                   .AddExact<MobMutantWolf>()
                                   .SetCustomPaddingWith(lizard,   8)
                                   .SetCustomPaddingWith(crawler,  8)
                                   .SetCustomPaddingWith(scorpion, 8)
                                   // very large padding with self to prevent spawning mutants nearby
                                   .SetCustomPaddingWithSelf(79);

            var NPC_BA_Sniper = spawnList.CreatePreset(interval: 15, padding: 0.5, useSectorDensity: false)
                                  .AddExact<MobNPC_BA_Sniper>(weight: 2.0)
                                   .SetCustomPaddingWith(lizard, 8)
                                   .SetCustomPaddingWith(crawler, 8)
                                   .SetCustomPaddingWith(scorpion, 8)
                                  .SetCustomPaddingWithSelf(24);

            var NPC_BA_Specialist = spawnList.CreatePreset(interval: 15, padding: 0.5, useSectorDensity: false)
                                   .AddExact<MobNPC_BA_Specialist>(weight: 3.0)
                                   .SetCustomPaddingWith(lizard, 8)
                                   .SetCustomPaddingWith(crawler, 8)
                                   .SetCustomPaddingWith(scorpion, 8)
                                   .SetCustomPaddingWith(NPC_BA_Sniper, 16)
                                   .SetCustomPaddingWithSelf(16);

            var NPC_BA_Cop = spawnList.CreatePreset(interval: 15, padding: 0.5, useSectorDensity: false)
                                   .AddExact<MobNPC_BA_Cop>(weight: 1.0)
                                   .SetCustomPaddingWith(lizard, 8)
                                   .SetCustomPaddingWith(crawler, 8)
                                   .SetCustomPaddingWith(scorpion, 8)
                                   .SetCustomPaddingWith(NPC_BA_Sniper, 18)
                                   .SetCustomPaddingWith(NPC_BA_Specialist, 18)
                                   .SetCustomPaddingWithSelf(20);

            var NPC_BA_Shotgunner = spawnList.CreatePreset(interval: 15, padding: 0.5, useSectorDensity: false)
                                   .AddExact<MobNPC_BA_Shotgunner>(weight: 3.0)
                                   .SetCustomPaddingWith(lizard, 8)
                                   .SetCustomPaddingWith(crawler, 8)
                                   .SetCustomPaddingWith(scorpion, 8)
                                   .SetCustomPaddingWith(NPC_BA_Sniper, 20)
                                   .SetCustomPaddingWith(NPC_BA_Specialist, 20)
                                   .SetCustomPaddingWith(NPC_BA_Cop, 12)
                                   .SetCustomPaddingWithSelf(20);
        }
    }
}