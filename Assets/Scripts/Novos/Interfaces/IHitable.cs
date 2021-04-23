using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IHitable                                                                                               //Classe herdada pelos objetos na cena que podem ser atingidos e irão soltar particulas de impacto
{
    ObjectSO objInfo { get;}                                                        //O scriptable object qe contem as informações de particulas e o método para a emissão dessas


}
