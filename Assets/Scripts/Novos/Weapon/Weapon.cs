using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    new public string name = "New Weapon";

    public int damage;

    public WeaponType type;

    public WeaponPosesInfo posesInfo;

}

public enum WeaponType { Primary, Secundary};

   
