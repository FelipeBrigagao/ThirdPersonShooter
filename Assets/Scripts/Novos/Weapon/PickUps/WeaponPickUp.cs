using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickUp : MonoBehaviour
{
    [SerializeField]
    WeaponStats weaponPrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            WeaponStats weapon =  Instantiate(weaponPrefab);
            StartCoroutine(WeaponManager.Instance.EquipNewWeapon(weapon));
        }
    }


}
