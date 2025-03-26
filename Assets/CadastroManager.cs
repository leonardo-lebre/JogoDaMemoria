using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;

public class CadastroManager : MonoBehaviour
{
    public TMP_InputField inputNome;
    public TMP_InputField inputEmail;
    public Button startButton;

    public GameObject erroCamposVazios;
    public GameObject erroEmailInvalido;
    public GameObject erroDominioInvalido;

    private DataBaseManager dataBaseManager;

    void Start() {
        dataBaseManager = GetComponent<DataBaseManager>();

        startButton.onClick.AddListener(CadastrarJogador);
    }

    bool emailValidado(string email) {
        string pattern = @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$";
        return Regex.IsMatch(email, pattern);
    }

    bool dominioEmailValido(string email) {
        string emailLower = email.ToLower();

        string[] dominiosEsperados = { "@gmail.com", "@yahoo.com", "@hotmail.com", "outlook.com", "me.com", "mac.com", "protonmail.com", "zoho.com" }; // Adicione os domínios desejados

        foreach (string dominio in dominiosEsperados) {
            if (emailLower.EndsWith(dominio)) {
                return true;
            }
        }
        return false;
    }

    void CadastrarJogador() {
        string nome = inputNome.text;
        string email = inputEmail.text;

        DesativarErros();

        if (string.IsNullOrEmpty(nome) || string.IsNullOrEmpty(email))
        {
            erroCamposVazios.SetActive(true);
            Debug.Log("Por favor, preencha todos os campos!");
            return;
        }
        else if (!emailValidado(email))
        {
            erroEmailInvalido.SetActive(true);
            Debug.Log("Insira um e-mail válido!");
            return;
        }
        else if (!dominioEmailValido(email)) {
            erroDominioInvalido.SetActive(true);
            Debug.Log("O domínio de e-mail não é permitido!");
            return;
        }
        else
        {
            if (!emailValidado(email))
            {
                Debug.Log("Insira um e-mail válido!");
                return;
            }
            else
            {
                DesativarErros();

                float tempoDecorrido = 0;
                int tentativas = 0;

                int id = dataBaseManager.SalvarDados(nome, email, tempoDecorrido, tentativas);

                PlayerPrefs.SetInt("ID", id);
                PlayerPrefs.SetString("PlayerName", nome);
                PlayerPrefs.SetString("PlayerEmail", email);
                PlayerPrefs.Save();

                Debug.Log("Jogador cadastrado com sucesso! ID: " + id);

                UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
            }
        }
    }

    void DesativarErros() {
        erroCamposVazios.SetActive(false);
        erroEmailInvalido.SetActive(false);
        erroDominioInvalido.SetActive(false);
    }
}

