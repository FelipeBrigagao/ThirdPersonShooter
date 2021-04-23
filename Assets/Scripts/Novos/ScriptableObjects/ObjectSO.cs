using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Object", menuName = "Object/BaseObj")]
public class ObjectSO : ScriptableObject                                                                                //Scriptable Object base para os outros, colocar tudo e comum e compartilhado nesse
{
    new public string name = "New Object";

    public ParticleSystem impactEffect;



    public void SpawnParticles(Vector3 position, Quaternion direction)                                                      //Método utilizado para emissão das particulas
    {
        ParticleSystem imp = Instantiate(impactEffect, position, direction);

        Destroy(imp.gameObject, 2f);

    }



}
