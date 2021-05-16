using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Damage Numbers", menuName = "Damage Numbers")]
public class DamageNumbersSO: ScriptableObject
{
   public Damage[] DamageArray;
}
[System.Serializable]
public class Damage{
   public int[] DamageNumbers;
}
