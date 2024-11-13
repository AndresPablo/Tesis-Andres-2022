using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Demokratos { 
    public class UI_NotificacionFlotante : MonoBehaviour
    {
        #region  SINGLETON
        public static UI_NotificacionFlotante singleton;
        private void Awake()
        {
            singleton = this;
        }
        #endregion

        [SerializeField] TextMeshProUGUI label;
        [SerializeField] Image fondo;
        bool mostrando;

        void Start()
        {
        
        }

        public void CrearEnJugador(string _texto, float _tiempo = 2f)
        {
            fondo.gameObject.SetActive(true);
            label.text = _texto;
            Vector3 pos = Game_Manager_Nuevo.singleton.JugadorPos;
            pos.y += 1f;
            pos.z = 0f;
            transform.position = pos;
            CancelInvoke();
            Invoke("Esconder", _tiempo);
        }

        void Esconder()
        {
            fondo.gameObject.SetActive(false);
            label.text = "";
        }
    }
}