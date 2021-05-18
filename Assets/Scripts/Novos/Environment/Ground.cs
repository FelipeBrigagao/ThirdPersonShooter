using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour, IHitable
{
    [SerializeField]
    private ObjectSO _obj;

    public ObjectSO objInfo { get { return _obj; } }         //Implementação da interface que vai possibilitar o raycast do weaponstats localizar qual objeto pode ser atingido
                                                                            //É o scriptable object, e esse referencia ao SO private, que é colocado no inspector


}
