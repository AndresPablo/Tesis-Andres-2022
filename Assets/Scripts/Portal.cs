using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] ParticleSystem particulas;
    public float recargaMax = 5f;
    public float carga;
    public GameObject prefab;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        carga += Time.deltaTime;
        if(carga >= recargaMax)
        {
            carga = 0;
            GenerarObjeto();
        }

    }

    void GenerarObjeto()
    {
        if (prefab == null) return;
        GameObject go = Instantiate(prefab, transform.position, transform.rotation) ;
    }
}
