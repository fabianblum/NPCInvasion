namespace AtomicTorch.CBND.CoreMod.Items.Special
{
    using AtomicTorch.CBND.CoreMod.StaticObjects.Misc;

    public class ItemSecretLocationData : ProtoItemSecretLocationUnlock<ObjectSecretLocationTeleport>
    {
        public override string Description =>
            "Contains records of coordinates and navigation frequency for the zombies island.";

        public override string Name => "Secret location data";
    }
}