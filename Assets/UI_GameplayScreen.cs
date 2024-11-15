using UnityEngine;
using Demokratos;

namespace Demokratos.UI{
public class UI_GameplayScreen : MonoBehaviour
{        
    [SerializeField] Canvas canvas;

    void Start()
    {
        
    }

    public void Mostrar(bool _estado){
        canvas.enabled = _estado;
    }
}
}