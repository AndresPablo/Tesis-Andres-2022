using System.Collections;
using System.Collections.Generic;
using Demokratos;
using UnityEngine;

namespace SistemaVotacion{
public class VotingCounter : MonoBehaviour
{
    // Las opciones para elegir
    [SerializeField] List<Acta> ActasEnEleccion = new List<Acta>();

    #region EVENTOS
        public delegate void EventoConteoActualizado(Acta[] actas);
        public delegate void EventoNuevoVoto(int acta, int cantidad);
        public static event EventoConteoActualizado Ev_CambiarVotos;
        public static event EventoNuevoVoto Ev_NuevoVoto;
    #endregion
   

    void Start()
    {
        
    }

    public void SetActasParaEleccion(Acta[] actas)
    {
        ActasEnEleccion.Clear();
        foreach(Acta a in actas)
        {
            ActasEnEleccion.Add(a);
        }
    }

    public void CambiarVotos(int _actaIndice, int _cantidad = 1)
    {
        ActasEnEleccion[_actaIndice].votos += _cantidad;
        if(Ev_CambiarVotos != null)
            Ev_CambiarVotos.Invoke(ActasEnEleccion.ToArray());
        if(Ev_NuevoVoto != null)
            Ev_NuevoVoto.Invoke(_actaIndice, _cantidad);
    }

    void Update()
    {
        DebugInput();
    }

    public Acta[] GetActasActuales()
    {
        return ActasEnEleccion.ToArray();
    }

    public Acta GetGanadora()
    {
        int indiceGanador = Random.Range(0, ActasEnEleccion.Count);
        int _votosMax = 0;
        Acta _ganadora = ActasEnEleccion[indiceGanador];
        foreach(Acta a in ActasEnEleccion)
        {
            if(a.votos > _votosMax)
            {
                _votosMax = a.votos;
                indiceGanador = ActasEnEleccion.IndexOf(a);
            }
        }
        return _ganadora;
    }

    void DebugInput()
    {
        // Suma votos con el teclado numerico (A=1, B=2)
        if(Input.GetKeyDown(KeyCode.Keypad1))
        {
            CambiarVotos(0);
        }else
        if(Input.GetKeyDown(KeyCode.Keypad2))
        {
            CambiarVotos(1);
        }
        // Resetea todos los votos

        // Apaga el sistema de votacion
        if(Input.GetKeyDown(KeyCode.Return))
        {
            VotingLogic.singleton.ToogleVotacion();
        }
    }
}

[System.Serializable]
public class Acta {
    public string name= "Acta...";
    public int votos;
    public Sprite sprite_A;
    public Sprite sprite_B;
    public TipoEfecto efecto;
}
}