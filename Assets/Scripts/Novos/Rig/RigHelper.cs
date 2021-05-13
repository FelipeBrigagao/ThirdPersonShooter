using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigHelper : MonoBehaviour
{
    public void ChangeUnarmedStateToFalse()
    {
        WeaponManager.Instance.ChangeUnarmedStatToFalse();
    }

    public void ChangeUnarmedStateToTrue()
    {
        WeaponManager.Instance.ChangeUnarmedStatToTrue();
    }

}
