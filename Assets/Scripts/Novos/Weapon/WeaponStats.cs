using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStats : MonoBehaviour
{

    [SerializeField]
    ParticleSystem muzzleFlash;

    [SerializeField]
    TrailRenderer bulletTracer;
    

    [SerializeField]
    Transform firePoint;

    [SerializeField]
    Transform crossHairTarget;

    
    [SerializeField]
    public WeaponSO weaponInfo;

    float nextTimeToShoot = 0f;
    

    public void Shoot()
    {

        if(nextTimeToShoot <= Time.time)
        {
            nextTimeToShoot = Time.time + 1f/weaponInfo.fireRate;

            muzzleFlash.Play();

            Vector3 bulletVelocity = (crossHairTarget.position - firePoint.position).normalized * weaponInfo.bulletSpeed;

            TrailRenderer tracer = Instantiate(bulletTracer, firePoint.position, Quaternion.identity);
            tracer.AddPosition(firePoint.position);

            Bullet bullet = new Bullet(bulletVelocity, firePoint.position, 0f, tracer, weaponInfo.bulletDrop, (weaponInfo.range/weaponInfo.bulletSpeed));

            BulletsManager.Instance.AddBullet(bullet);
            
            
        }
    }

}
