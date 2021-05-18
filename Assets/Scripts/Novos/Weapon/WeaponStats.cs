using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStats : MonoBehaviour
{

    ParticleSystem muzzleFlash;

    [SerializeField]
    Transform firePoint;


    Transform crossHairTarget;


    [SerializeField]
    WeaponSO _weaponInfo;

    public WeaponSO WeaponInfo
    {
        get 
        {
            if(_weaponInfo != null)
            {
                return _weaponInfo;
            }

            return null;
        }
    }

    float nextTimeToShoot = 0f;
    



    private void Start()
    {
        muzzleFlash = Instantiate(WeaponInfo.muzzleFlash, firePoint);

    }

    public void SetCrossHairTarget(Transform cht)
    {
        this.crossHairTarget = cht;
    }

    public void Shoot()
    {

        if(nextTimeToShoot <= Time.time)
        {
            nextTimeToShoot = Time.time + 1f/WeaponInfo.fireRate;

            muzzleFlash.Play();

            Vector3 bulletVelocity = (crossHairTarget.position - firePoint.position).normalized * WeaponInfo.bulletSpeed;

            TrailRenderer tracer = Instantiate(WeaponInfo.bulletTracer, firePoint.position, Quaternion.identity);
            tracer.AddPosition(firePoint.position);

            Bullet bullet = new Bullet(bulletVelocity, firePoint.position, tracer, WeaponInfo.bulletDrop, (WeaponInfo.range/WeaponInfo.bulletSpeed));

            BulletsManager.Instance.AddBullet(bullet);
            
            
        }
    }

}
