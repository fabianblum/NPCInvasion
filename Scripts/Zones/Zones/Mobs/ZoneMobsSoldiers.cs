namespace AtomicTorch.CBND.CoreMod.Zones
{
    using AtomicTorch.CBND.GameApi;

    public class ZoneMobsSoldiers : ProtoZoneDefault
    {
        [NotLocalizable]
        public override string Name => "Mobs - Soldiers";

        protected override void PrepareZone(ZoneScripts scripts)
        {
            scripts
                .Add(GetScript<SpawnMobsSoldiers>());
        }
    }
}