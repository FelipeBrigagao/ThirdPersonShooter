using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Weapon", menuName = "Object/Weapon")]
public class WeaponSO : ObjectSO
{
    public float damage;

    public float fireRate;

    public float range;

    public int maxAmmo;

    public float bulletSpeed;

    public float bulletDrop;

    public WeaponType type;

    public WeaponPosesInfo posesInfo;

}

public enum WeaponType { Primary, Secundary};

   
