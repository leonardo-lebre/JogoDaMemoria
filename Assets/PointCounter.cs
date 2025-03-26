using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PointCounter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI tentativasText;

    private int tentativas = 0;

    private void Start() {
        AtualizarTentativasUI();
    }

    public void IncrementarTentativas() {
        tentativas++;
        AtualizarTentativasUI();
    }

    private void AtualizarTentativasUI() {
        tentativasText.text = "Tentativas: " + tentativas;
    }

    public int GetTentativas() {
        return tentativas;
    }
}
