using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
   [SerializeField] TextMeshProUGUI timerText;

   float tempoDecorrido;

   private bool jogoAcabou = false;

    void Update()
    {
        if (!jogoAcabou)
        {
            tempoDecorrido += Time.deltaTime;
            int minutos = Mathf.FloorToInt(tempoDecorrido / 60);
            int segundos = Mathf.FloorToInt(tempoDecorrido % 60);

            timerText.text = string.Format("{0:00}:{1:00}", minutos, segundos);
        }
    }

    public void FinalizarJogo() {
        jogoAcabou = true;
    }


    public float GetTempoDecorrido() {
        return tempoDecorrido;
    }
}
