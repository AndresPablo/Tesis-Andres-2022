using Unity.Mathematics;
using UnityEngine;

namespace Demokratos.Energia{
    
    public class BloqueEolico : BloqueBase
    {
        [SerializeField] AreaViento vientoPrefab;
        [SerializeField] Direccion direccion;
        public float fuerza = 5;
        AreaViento areaViento;
        VentiladorVisual ventiladorVisual;



        protected void Start() {
            base.Start();
            ventiladorVisual = GetComponent<VentiladorVisual>();
            // instancia y coloca el Area-Viento
            areaViento = Instantiate(vientoPrefab, transform);
            areaViento.transform.position = transform.position;
            // configura el viento inicial
            CambiarViento(direccion);
        }

        public void CambiarViento(Direccion _nuevaDireccion)
        {
            Vector2 vectorViento = new Vector2(0,0);
            switch (_nuevaDireccion)
            {
                case Direccion.IZQUIERDA:
                    vectorViento.x = -fuerza;
                    vectorViento.y = 0; 
                    //areaViento.transform.position = anclaIzq.position;
                    //areaViento.transform.SetParent(anclaIzq);
                    //areaViento.transform.rotation = anclaIzq.rotation;
                    areaViento.transform.Rotate( new Vector3(0,0,90), Space.Self);
                break;
                case Direccion.DERECHA:
                    vectorViento.x = fuerza;
                    vectorViento.y = 0;
                    //areaViento.transform.position = anclaDer.position;
                    //areaViento.transform.SetParent(anclaDer);
                    //areaViento.transform.rotation = anclaDer.rotation;
                    areaViento.transform.Rotate( new Vector3(0,0, -90), Space.Self);
                break;
                case Direccion.ABAJO:
                    vectorViento.x = 0;
                    vectorViento.y = fuerza;
                    //areaViento.transform.position = anclaAbajo.position;
                    //areaViento.transform.SetParent(anclaAbajo);
                    //areaViento.transform.rotation = anclaAbajo.rotation;
                    areaViento.transform.Rotate( new Vector3(0,0, -180), Space.Self);
                break;
                case Direccion.ARRIBA:
                    vectorViento.x = 0;
                    vectorViento.y = +fuerza;
                    //areaViento.transform.position = anclaArriba.position;
                    //areaViento.transform.SetParent(anclaArriba);
                break;
            }
            areaViento.vectorViento = vectorViento;
        }

        public void CambiarFuerzaViento(float _fuerzaNueva)
        {
            fuerza = _fuerzaNueva;
            CambiarViento(direccion); // actualiza los vectores
        }

        public void CambiarViento(Direccion _dir, float _fuerza)
        {
            fuerza = _fuerza;
            CambiarViento(_dir);  // actualiza los vectores
        }
    }
}