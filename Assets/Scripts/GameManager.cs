using UnityEngine;
using ArduinoSerialControl;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.Tilemaps;


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
    int nivelIndex;
    int muertes;
    float tiempo;
    Tilemap NivelActual { get { return niveles[nivelIndex]; } }

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
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            Reiniciar();
        }
        if(nivelIndex == 0)
        {
            if(Input.anyKey)
            {
                //levelGO.SetActive(false);
            }
        }
    }

    void SwapAllTiles(TileBase tileVieja, TileBase tileNueva)
    {
        NivelActual.SwapTile(tileVieja, tileNueva);
    }

    public void SpawnearJugador()
    {
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
        int cantHijos = NivelActual.transform.childCount;
        GameObject go = null;
        foreach(Transform ch in NivelActual.transform)
        {
            TileProps props = ch.GetComponent<TileProps>();
            if (!props) continue;
            if(props.Tipo == tipo)
            {
                go = props.gameObject;
                
                return go;
            }
        }
        if(go == null)
        {
            Debug.LogWarning("No hay nada");
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
    }

    public void Reiniciar()
    {
        muertes = 0;
        SceneManager.LoadScene(0);
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