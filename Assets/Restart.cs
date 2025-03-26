using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Restart : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI tempoText;
    [SerializeField] TextMeshProUGUI tentativasText;
    public Button RestartButton;

    void Start() {
        RestartButton.onClick.AddListener(RestartGame);

        float tempoDecorrido = PlayerPrefs.GetFloat("TempoDecorrido", 0f);
        int tentativas = PlayerPrefs.GetInt("Tentativas", 0);

        int minutos = Mathf.FloorToInt(tempoDecorrido / 60);
        int segundos = Mathf.FloorToInt(tempoDecorrido % 60);
        string tempoFormatado = string.Format("{0:D2}:{1:D2}", minutos, segundos);

        tempoText.text = "Tempo Decorrido: " + tempoFormatado;
        tentativasText.text = "Tentativas: " + tentativas;
    }

    public void RestartGame() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("CadastroScene");
    }
}
