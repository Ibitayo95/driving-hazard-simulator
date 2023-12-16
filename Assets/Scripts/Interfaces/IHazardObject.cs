using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHazardObject
{
    public string Name { get; set; }
    public float hazardOffsetTime { get; set; }
    public int ChanceOfOccuring { get; set; }
    public void ActivateHazard();
}