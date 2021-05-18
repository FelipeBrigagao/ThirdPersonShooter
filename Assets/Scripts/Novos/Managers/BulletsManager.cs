using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletsManager : MonoBehaviour
{
    #region Singleton

    private static BulletsManager _instance;
    public static BulletsManager Instance
    {
            get
        {
                if (_instance == null)
                {
                    Debug.LogWarning("Bullet manager não encontrado, criando um.");

                    GameObject go = new GameObject("BulletManager");

                    go.AddComponent<BulletsManager>();

                }

                return _instance;
            }
        }

    private void Awake()
    {
        if (_instance != null)
        {
            Debug.LogWarning("Bullet manager já existente, uma segunda instância não será criada.");
            return;
        }

        Debug.Log("Bullet Manager criado.");
        _instance = this;

    }

    #endregion

    List<Bullet> bullets = new List<Bullet>();

    Ray ray;
    RaycastHit hitInfo;

    Vector3 p0;
    Vector3 p1;


    private void Update()                                                                       //Realiza a movimentação e a remoção das balas armazenadas
    {
        SimulateBullets(Time.deltaTime);
        RemoveBullets();
    }


    void RemoveBullets()
    {
        bullets.RemoveAll(bullet => bullet.time >= bullet.maxLifeTime);                     //Remove da lista as balas que excederam o seu tempo de vida
    }

    public void AddBullet(Bullet bullet)
    {
        bullets.Add(bullet);
    }

    Vector3 GetBulletPosition(Bullet bullet)                        // Pega a proxima posição da bala com relação ao tempo que a mesma existe, usa a posição inicial(dada pelo ponto de tiro da arma), velocidade inicial (dada pela direção e velocidade da bala)
    {                                                                         //bullet time(tempo que a bala existe)   

        return bullet.initialPosition + bullet.initialVelocity * bullet.time + 0.5f * bullet.bulletGravity * bullet.time * bullet.time;             //Formula da trajetória de um projétil Po + Vo*t + 0.5*g*t²
    }

    void SimulateBullets(float deltatime)                      //simula a movimentação da bala, pegando a posição com o tempo que ela está e após mais uma aferição de tempo, com isso se consegue um pequeno deslocamento
    {                                                                    // que tera um raycast associado a esse, para verificar se no próximo movimento teria um impacto
        foreach (Bullet bullet in bullets)
        {
            p0 = GetBulletPosition(bullet);
            bullet.time += deltatime;
            p1 = GetBulletPosition(bullet);

            RaycastBulletSegment(p0, p1, bullet);

        }
    }

    void RaycastBulletSegment(Vector3 start, Vector3 end, Bullet bullet)
    {                                                                                               // Verifica através de um raycast se a bala irá impactar em alguma coisa no próximo movimento dela
        Vector3 direction = end - start;
        float distance = direction.magnitude;

        ray.origin = start;
        ray.direction = direction.normalized;


        if(Physics.Raycast(ray, out hitInfo, distance))
        {

            Debug.Log("Hit" + hitInfo.transform.name);

            bullet.bulletTracer.transform.position = hitInfo.point;

            IHitable obj = hitInfo.transform.GetComponent<IHitable>();                      // Verifica a presença da interface Ihitable no alvo atingido, se essa for encontrada o alvo gerará a particula de impacto referente a esse
                                                                                            //O método de instanciação está no alvo atingido
            if (obj != null)
            {
                ParticlesManager.Instance.SpawnParticles(hitInfo.point, Quaternion.LookRotation(hitInfo.normal), obj.objInfo.particleEffectType);

            }

            bullet.time = bullet.maxLifeTime;                           //Se a bala impactou em alguma coisa essa tem o tempo dela setado para o máximo dela, porque assim ela será excluida da lista de balas a se verificar

        }
        else
        {
            bullet.bulletTracer.transform.position = end;
        }

    }



}
