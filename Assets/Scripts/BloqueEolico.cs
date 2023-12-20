using Unity.Mathematics;
using UnityEngine;

namespace Demokratos.Energia{
    
    public enum TipoEmpuje { ADITIVA, FIJA };

    public class BloqueEolico : BloqueBase
    {
        [SerializeField] AreaViento vientoPrefab;
        [SerializeField] Direccion direccion;
        [SerializeField] TipoEmpuje tipoEmpuje;
        [SerializeField] float fuerza = 5;
        [SerializeField] float recarga = 5;
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
                    vectorViento.x = fuerza;
                    vectorViento.y = 0; 
                    areaViento.transform.Rotate( new Vector3(0,0,90), Space.Self);
                break;
                case Direccion.DERECHA:
                    vectorViento.x = fuerza;
                    vectorViento.y = 0;
                    areaViento.transform.Rotate( new Vector3(0,0, -90), Space.Self);
                break;
                case Direccion.ABAJO:
                    vectorViento.x = 0;
                    vectorViento.y = fuerza;
                    areaViento.transform.Rotate( new Vector3(0,0, -180), Space.Self);
                break;
                case Direccion.ARRIBA:
                    vectorViento.x = 0;
                    vectorViento.y = fuerza;
                break;
            }
            areaViento.CargarValores(vectorViento, recarga, tipoEmpuje);
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