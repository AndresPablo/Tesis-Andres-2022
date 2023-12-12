using UnityEngine;
using ArduinoSerialControl;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.Tilemaps;
using SistemaVotacion;

namespace Viejo{
public class GameManager : MonoBehaviour
{
    public static GameManager singleton;
    private void Awake()
    {
        singleton = this;
    }

    [SerializeField] VoteControl Votador;
    [SerializeField] SerialController Serial;
    [SerializeField] csOpenTSPSListener TSPS_Listener;
    [Space]
    public CharacterController2D playerController;
    public Jugador player;
    [SerializeField][Range(1f,5f)] float respawnTime = 2f;
    [SerializeField] Transform contenedorNiveles;
    [SerializeField] Tilemap[] niveles;
    GameObject levelGO;
    [Space]
    [Range(.1f,1f)][SerializeField] float valorSlowMo = .5f;
    public Color peligroColor;
    public Color normalColor;
    public UnityEvent OnVictory;
    public UnityEvent OnRestart;
    int nivelIndex;
    public int muertes;
    public float tiempo;
    Tilemap NivelActual { get { return niveles[nivelIndex]; } }
    public bool gameOver;

    void Start()
    {
        Jugador.Ev_Muere += InvocarRespawn;
    }

    public void EmpezarJuego()
    {
        nivelIndex = 0;
        for(int i = 0; i < niveles.Length; i++)
        {
           niveles[i].gameObject.SetActive(false);
        }
        niveles[nivelIndex].gameObject.SetActive(true);
        SpawnearJugador();
        AudioManager.instance.Stop();
        muertes = 0;
        tiempo = 0;
        // Empezamos a votar
        Votador.Inicializar();
    }



    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            Reiniciar();
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            PasarNivel();
        }

        // sumamos los segundos en el Update
        tiempo += Time.deltaTime;
    }

    void SwapAllTiles(TileBase tileVieja, TileBase tileNueva)
    {
        NivelActual.SwapTile(tileVieja, tileNueva);
    }

    public void SpawnearJugador()
    {
        if (nivelIndex >= niveles.Length) return;

        foreach(Transform child in NivelActual.transform)
        {
            if(child.CompareTag("Spawn"))
            {
                player.transform.position = child.position;
            }
        }
        player.gameObject.SetActive(true);
        player.Spawn();

    }

    public void PasarNivel()
    {
        nivelIndex++;
        if(nivelIndex >= niveles.Length)
        {
            Victoria();
            Votador.CancelarVotaciones();
            return;
        }

        Votador.AbortarVotacion();
        for (int i = 0; i < niveles.Length; i++)
        {
            if (nivelIndex == i)
            {
                niveles[i].gameObject.SetActive(true);
            }
            else
                niveles[i].gameObject.SetActive(false);
        }
        SpawnearJugador();
    }

    public GameObject GetRandomObjectFromlevel()
    {
        int cantHijos = NivelActual.transform.childCount;
        GameObject go;
        go = NivelActual.transform.GetChild(Random.Range(0, cantHijos)).gameObject;
        Debug.Log("tomando " + go.name);
        return go;
    }

    public GameObject GetObjetoDeTipoEnNivel(TipoObjeto tipo)
    {
        if (gameOver)
            return null;

        int cantHijos;
        if (NivelActual)
        {
             cantHijos = NivelActual.transform.childCount;
        }
        else
        {
            Debug.LogWarning("No hay hijos del nivel");
        }
            
        
        GameObject go = null;
        foreach (Transform ch in NivelActual.transform)
        {
            TileProps props = ch.GetComponent<TileProps>();
            if (!props) continue;
            if(props.Tipo == tipo)
            {
                go = props.gameObject;
                Debug.Log("tomando objeto del tipo: " + tipo);
                
                return go;
            }
        }
        if(go == null)
        {
            Debug.LogError("No hay nada del tipo �["  + tipo + "] en el nivel actual.");
        }
        return go;
    }


    // AYUDAS PARA SETEAR PROPIEDADES
    public void CambiarReboteDeObjetos(TipoObjeto _tipoObjeto, float _valor)
    {
        foreach (Transform ch in NivelActual.transform)
        {
            TileProps props = ch.GetComponent<TileProps>();
            if (!props) continue;
            if (props.Tipo == _tipoObjeto)
            {
                props.SetRebote(_valor);
            }
        }
    }

    public void SetPeligroTodos(TipoObjeto _tipoObjeto)
    {
        foreach (Transform ch in NivelActual.transform)
        {
            TileProps props = ch.GetComponent<TileProps>();
            if (!props) continue;
            if (props.Tipo == _tipoObjeto)
            {
                props.SetPeligro();
            }
        }
    }

    public void SetSolidezTodos(TipoObjeto _tipoObjeto)
    {
        foreach (Transform ch in NivelActual.transform)
        {
            TileProps props = ch.GetComponent<TileProps>();
            if (!props) continue;
            if (props.Tipo == _tipoObjeto)
            {
                props.SetSolido();
            }
        }
    }

    public void ActivarSlowMo()
    {
        Time.timeScale = valorSlowMo;
        Invoke("ApagarSlowMo", 2f);
    }

    public void ApagarSlowMo()
    {
        Time.timeScale = 1;
    }

    public void Victoria()
    {
        OnVictory.Invoke();
        Votador.AbortarVotacion();
        Votador.CancelarVotaciones();
        player.ApagarCharControl();
        gameOver = true;
    }

    public void Reiniciar()
    {
        muertes = 0;
        tiempo = 0;
        //SceneManager.LoadScene(0);
        Time.timeScale = 1;
        nivelIndex = 0;
        gameOver = false;
        // Encender nivel 1
        for (int i = 0; i < niveles.Length; i++)
        {
            if (nivelIndex == i)
            {
                niveles[i].gameObject.SetActive(true);
            }
            else
                niveles[i].gameObject.SetActive(false);
        }
        if (OnRestart != null)
            OnRestart.Invoke();
        Debug.Log("__Juego reiniciado__");

    }

    void InvocarRespawn()
    {
        muertes++;
        Invoke("SpawnearJugador", respawnTime);
    }


}

public enum GameState { INTRO, PLAY, FIN }

public struct GameSettings{


}

}