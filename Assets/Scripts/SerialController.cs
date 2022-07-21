using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.IO.Ports;
using UnityEngine.UI;
using UnityEngine.Events;


namespace ArduinoSerialControl {

public class SerialController : MonoBehaviour
{
    [SerializeField] string puertoCOM = "COM1";
    [SerializeField] int baudRate = 9600;
    [Space]
    public GameObject comDropdown;
    public GameObject baudInput;
    public Text avPorts_Label;
    public Text conected_Label;
    public Text error_Label;
    public Button connectTry_btn;
    public Button close_btn;

    SerialPort arduinoPort;
    //SerialPort arduinoPort = new SerialPort("COM4");
    public UnityEvent OnConectadoExito;
    public UnityEvent OnConectadoFallo;


        void Awake()
    {
        avPorts_Label.text = string.Empty;
        string[] ports = SerialPort.GetPortNames();
        foreach(string port in ports)
        {
            avPorts_Label.text += '\n';
            avPorts_Label.text += port;
        }
    }

    void Start()
    {
        
        //arduinoPort.ReadTimeout = 50;
    }

    public void ConectarCOMActual()
    {
        bool portExists = false;
        if(!string.IsNullOrEmpty(avPorts_Label.text))
            portExists = true;

        arduinoPort = new SerialPort(puertoCOM, baudRate);
        arduinoPort.BaudRate = baudRate;
        arduinoPort.Parity = Parity.None;
        arduinoPort.StopBits = StopBits.One;
        arduinoPort.DataBits = 8;
        arduinoPort.Handshake = Handshake.None ;
        arduinoPort.ReadTimeout = 500;
        arduinoPort.WriteTimeout = 500;

        arduinoPort.Open();

        close_btn.gameObject.SetActive(true);
        connectTry_btn.gameObject.SetActive(false);

        if(portExists)
        {
            error_Label.gameObject.SetActive(false);
            conected_Label.gameObject.SetActive(true);
            conected_Label.text = "Conectado " + puertoCOM;
            baudInput.SetActive(false);
            comDropdown.SetActive(false);
            OnConectadoExito.Invoke();
        }
        else
        {
            baudInput.SetActive(true);
            comDropdown.SetActive(true);
            error_Label.gameObject.SetActive(true);
            conected_Label.gameObject.SetActive(false);
            OnConectadoFallo.Invoke();
        }

    }

    public void CambiarPuertoCOM(int i)
    {
        puertoCOM = "COM" + (i+1);
    }

    public void CambiarFrecuenciaBaudios(string _baud){
        baudRate = int.Parse(_baud);
    }

    void Update()
    {
        
    }

    public void EnviarMensajeAlArduino(string msj)
    {
        if (arduinoPort == null)
        {
            Debug.LogWarning("Arduino no encontrado");
        }else
        arduinoPort.WriteLine(msj);
    }

    public void CerrarPuerto()
    {
        if(arduinoPort != null)
            arduinoPort.Close();
        close_btn.gameObject.SetActive(false);
        connectTry_btn.gameObject.SetActive(true);
        error_Label.gameObject.SetActive(false);
        conected_Label.gameObject.SetActive(false);
        baudInput.SetActive(true);
        comDropdown.SetActive(true);
    }

    void OnApplicationQuit()
    {
        CerrarPuerto();
    }
}
}