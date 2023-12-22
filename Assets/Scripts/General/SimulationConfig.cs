using System;


public static class SimulationConfig
{
    public static bool CarPositionRandomised { get; set; }
    public static bool IsHighFidelity { get; set; }
    public static readonly Random random = new();

}
