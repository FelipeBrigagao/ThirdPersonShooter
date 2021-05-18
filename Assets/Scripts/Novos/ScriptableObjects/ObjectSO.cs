using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Object", menuName = "Object/BaseObj")]
public class ObjectSO : ScriptableObject                                                                                //Scriptable Object base para os outros, colocar tudo e comum e compartilhado nesse
{
    new public string name = "New Object";

    public ParticleEffectType particleEffectType;


}
