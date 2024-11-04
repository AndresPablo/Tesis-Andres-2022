using UnityEngine;
using Viejo;
using UnityEngine.UI;

public class UI_VoteTimer : MonoBehaviour
{
    public VoteControl control;
    public Image imagen;
    public Text timeLabel;

    void Start()
    {
        
    }

    void Update()
    {
        if(control.votingInProgress)
        {
            imagen.fillAmount = control.voteTimer / (control.maxVoteTime);
            timeLabel.text = ((int)control.voteTimer +1).ToString();
        }
    }
}
