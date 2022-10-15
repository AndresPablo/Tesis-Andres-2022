using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VoteControl : MonoBehaviour
{

    public bool votingInProgress;
    public float tiempoEntreElecciones = 6f;
    public float maxVoteTime = 6f;
    public float voteTimer;
    public int votos_si;
    public int votos_no;
    [SerializeField] AudioClip voteClose_SFX;
    [Space]
    [SerializeField] csOpenTSPSListener TSPS_Listener;
    [SerializeField] UI_VotePanel miPanel;
    [SerializeField] VoteEffector effector;
    [Space]
    public VotacionData dataActual;
    [Space]
    public UnityEvent OnVotingClose;
    public UnityEvent OnVotingOpen;

    public delegate void NuevoVotoDelegate(bool _bool);
    public delegate void EleccionDelegate(VotacionData _vData);
    public static event NuevoVotoDelegate Ev_NuevoVotoSi;
    public static event NuevoVotoDelegate Ev_NuevoVotoNo;
    public static event EleccionDelegate Ev_OnVotingClose;
    public static event EleccionDelegate Ev_OnVotingOpen;
    public static event EleccionDelegate Ev_OnVotingAbort;
    public static event EleccionDelegate Ev_ProcesarEleccion;


    void Start()
    {
        Debug.Log("Centro: " + Screen.width / 2);
    }

    public void Inicializar()
    {
        votingInProgress = false;
        voteTimer = maxVoteTime;
        votos_no = 0;
        votos_si = 0;
        dataActual = null; 
        StartCoroutine("RutinadeVotacion");
    }

    public void OverrideVotos(int a, int b)
    {
        votos_si = a;
        votos_no = b;
        miPanel.OverrideVotos(votos_si, votos_no);
    }

    public void NuevoVoto(bool si)
    {
        if(si)
        {
            votos_si++;
            if (Ev_NuevoVotoSi != null)
                Ev_NuevoVotoSi(si);
        }
        else
        {
            votos_no++;
            if (Ev_NuevoVotoNo != null)
                Ev_NuevoVotoNo(si);
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            Inicializar();
        }

        if (votingInProgress)
        {
            MonitorearPosiciones();
            voteTimer -= Time.deltaTime;
            if(voteTimer <= 0)
            {
                CerrarVotacion();
            }
        }
    }

    // Aca tomamos la data del TSPS y la traducimos a un conteo de votos
    void MonitorearPosiciones()
    {
        int a = 0;
        int b = 0;
        foreach (KeyValuePair<int, GameObject> blob in TSPS_Listener.blobGameObjects)
        {
            if (blob.Value.transform.position.x > 0)
            {
                a++;
            }
            else
            {
                b++;
            }
        }
        OverrideVotos(a, b);
    }

    void ProcesarResultado()
    {
        if(votos_si > votos_no)
        {
            effector.ProcesarResultado(dataActual.actaA);
        }else
        if(votos_no > votos_si)
        {
            effector.ProcesarResultado(dataActual.actaB);
        }else
        if (votos_no == votos_si)
        {
            float roll = Random.value;
            bool moneda;
            if (roll > .5f) moneda = true; else moneda = false;
            if(moneda) 
                effector.ProcesarResultado(dataActual.actaA); 
            else 
                effector.ProcesarResultado(dataActual.actaB);
        }
        if (Ev_ProcesarEleccion != null)
            Ev_ProcesarEleccion(dataActual);
    }
       
    void AbrirVotacion()
    {
        votingInProgress = true;
        dataActual = effector.GetRandomPoll(); 
        OnVotingOpen.Invoke();
        if(Ev_OnVotingOpen != null)
        {
            Ev_OnVotingOpen.Invoke(dataActual);
        }
    }

    void CerrarVotacion()
    {
        votingInProgress = false;
        voteTimer = maxVoteTime;
        ProcesarResultado();
        if (Ev_OnVotingClose != null)
        {
            Ev_OnVotingClose.Invoke(dataActual);
        }
        votos_no = 0;
        votos_si = 0;
        dataActual = null;
        OnVotingClose.Invoke();
        AudioManager.instance.PlayOneShot(voteClose_SFX);
        StartCoroutine("RutinadeVotacion");
    }

    // igual, solo que no Procesa() los resultados
    public void AbortarVotacion()
    {
        votingInProgress = false;
        voteTimer = maxVoteTime;
        if (Ev_OnVotingClose != null)
        {
            Ev_OnVotingClose.Invoke(dataActual);    // TODO: DESFAZAR?? ose cambiar por evento de aborto
        }
        if (Ev_OnVotingAbort != null)
        {
            Ev_OnVotingAbort.Invoke(dataActual);
        }
        OnVotingClose.Invoke();
        votos_no = 0;
        votos_si = 0;
        dataActual = null;
        StartCoroutine("RutinadeVotacion");
    }

    IEnumerator RutinadeVotacion()
    {
        yield return new WaitForSeconds(tiempoEntreElecciones);
        AbrirVotacion();
    }
}
