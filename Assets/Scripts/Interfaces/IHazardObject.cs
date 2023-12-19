public interface IHazardObject
{
    public string Name { get; }
    public float HazardOffsetTime { get; }
    public int ChanceOfOccuring { get; }

    public void ActivateHazard();
}