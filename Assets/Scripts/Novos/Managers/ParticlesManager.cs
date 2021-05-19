using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ParticlesManager : MonoBehaviour
{
    #region Singleton

    private static ParticlesManager _instance;

    public static ParticlesManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogWarning("Particles Manager não encontrado, criando um.");

                GameObject go = new GameObject("ParticlesManager");

                go.AddComponent<ParticlesManager>();

            }

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null)
        {
            Debug.LogWarning("Particles Manager já existente, uma segunda instância não será criada.");
            return;
        }

        Debug.Log("Particles Manager criado.");
        _instance = this;

    }

    #endregion

    [SerializeField]
    ParticleSystem[] impactEffect = new ParticleSystem[Enum.GetValues(typeof(ParticleEffectType)).Length];


    public void SpawnParticles(Vector3 position, Quaternion direction, ParticleEffectType pet)                                                      //Método utilizado para emissão das particulas
    {
        

        ParticleSystem imp = Instantiate(impactEffect[(int)pet], position, direction);

        Destroy(imp.gameObject, 2f);

    }

}


public enum ParticleEffectType {Stone, Wood};

