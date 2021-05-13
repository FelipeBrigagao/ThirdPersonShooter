using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Singleton

    private static UIManager _instance;

    public static UIManager Instance
    {
        get
        {
            if(_instance == null)
            {
                Debug.LogError("UI Manager não encontrado");

                GameObject go = new GameObject("UIManager");

                go.AddComponent<UIManager>();

            }

            return _instance;

        }
    }

    private void Awake()
    {
        if(_instance != null)
        {
            Debug.LogWarning("UI Manager já existe, outra instância não será criada");
            return;
        }


        Debug.Log("UI Manager criado.");
        _instance = this;

    }





    #endregion

    [SerializeField]
    Image crosshairDot;

    [SerializeField]
    float crosshairAlphaSpeed;
    
    bool aiming;

    //Tirar o ponto quando mirar, usando o event do player manager

    private void Start()
    {
        aiming = false;

        PlayerManager.OnAimWeapon += ChangeAimState;
    }


    void ChangeAimState(bool stat)
    {
        aiming = stat;

    }


    private void Update()
    {
        ChangeCrossHairUIAlpha();


    }



    void ChangeCrossHairUIAlpha()
    {
        if (aiming && crosshairDot.color.a < 1)
        {
            Color auxColor = crosshairDot.color;
            auxColor.a += Time.deltaTime / crosshairAlphaSpeed;
            crosshairDot.color = auxColor;

        }
        else if (!aiming && crosshairDot.color.a > 0)
        {
            Color auxColor = crosshairDot.color;
            auxColor.a -= Time.deltaTime / crosshairAlphaSpeed;
            crosshairDot.color = auxColor;
        }
    }



    private void OnDisable()
    {
        PlayerManager.OnAimWeapon -= ChangeAimState;
    }

}
