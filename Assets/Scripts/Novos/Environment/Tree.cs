using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour, IHitable
{
    [SerializeField]
    private ObjectSO obj;

    public ObjectSO objInfo { get { return obj; } }


}
