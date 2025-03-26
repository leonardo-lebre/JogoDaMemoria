using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsController : MonoBehaviour
{
    [SerializeField] Card cardPrefab;
    [SerializeField] Transform gridTransform;
    [SerializeField] Sprite[] sprites;
    [SerializeField] PointCounter pointCounter;
    [SerializeField] Timer timer;
    [SerializeField] DataBaseManager dataBaseManager;
    [SerializeField] CadastroManager cadastroManager;

    private List<Sprite> spritePairs;

    Card firstSelected;
    Card secondSelected;

    int Completed;

    private void Start() {
        PrepareSprites();
        CreateCards();

        if (dataBaseManager == null)
        {
            dataBaseManager = FindObjectOfType<DataBaseManager>();
        }
    }

    private void PrepareSprites() {
        spritePairs = new List<Sprite>();

        for (int i = 0; i < sprites.Length; i++)
        {
            spritePairs.Add(sprites[i]);
            spritePairs.Add(sprites[i]);
        }

        ShuffleSprites(spritePairs);
    }

    void CreateCards() {
        for (int i = 0; i < spritePairs.Count; i++)
        {
            Card card = Instantiate(cardPrefab, gridTransform);
            card.SetIconSprite(spritePairs[i]);
            card.controller = this;
        }
    }

    public void SetSelected(Card card) {
        if (card.isSelected == false)
        {
            card.Show();

            if (firstSelected == null)
            {
                firstSelected = card;
                return;
            }

            if (secondSelected == null)
            {
                pointCounter.IncrementarTentativas();

                secondSelected = card;
                StartCoroutine(CheckMatchingCards(firstSelected, secondSelected));
                firstSelected = null;
                secondSelected = null;
            }
        }

        
    }

    IEnumerator CheckMatchingCards(Card a, Card b) {
        yield return new WaitForSeconds(0.3f);
        if (a.iconSprite == b.iconSprite)
        {
            Completed++;
            if (Completed >= spritePairs.Count / 2)
            {
                PrimeTween.Sequence.Create()
                    .Chain(PrimeTween.Tween.Scale(gridTransform, Vector3.one*1.2f, 0.2f, ease: PrimeTween.Ease.OutBack))
                    .Chain(PrimeTween.Tween.Scale(gridTransform, Vector3.one, 0.1f));

                FindObjectOfType<Timer>().FinalizarJogo();
                SalvarDadosJogo();

                yield return new WaitForSeconds(5f);
                
                UnityEngine.SceneManagement.SceneManager.LoadScene("EndingScene");
            }
        }
        else
        {
            a.Hide();
            b.Hide();
        }
    }

    void ShuffleSprites(List<Sprite> spriteList) {
        for (int i = spriteList.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);

            Sprite temp = spriteList[i];
            spriteList[i] = spriteList[randomIndex];
            spriteList[randomIndex] = temp;
        }
    }

    void SalvarDadosJogo() {
        int id = PlayerPrefs.GetInt("ID", -1);
        string playerName = PlayerPrefs.GetString("PlayerName", "Unknown");
        string email = PlayerPrefs.GetString("PlayerEmail", "Unknown");

        if (id == -1)
        {
            Debug.Log("Erro: Nenhum jogador encontrado!");
            return;
        }

        float tempoDecorrido = timer.GetTempoDecorrido();
        int tempoEmSegundos = Mathf.RoundToInt(tempoDecorrido);

        int minutos = tempoEmSegundos / 60;
        int segundos = tempoEmSegundos % 60;
        string tempoFormatado = string.Format("{0:D2}:{1:D2}", minutos, segundos);

        int tentativas = pointCounter.GetTentativas();

        PlayerPrefs.SetFloat("TempoDecorrido", tempoDecorrido);
        PlayerPrefs.SetInt("Tentativas", tentativas);
        PlayerPrefs.Save();


        Debug.Log("Verificando variáveis: ");
        Debug.Log("dataBaseManager: " + (dataBaseManager != null));
        Debug.Log("playerName: " + playerName);
        Debug.Log("email: " + email);
        Debug.Log("tempoTotal: " + tempoFormatado);
        Debug.Log("tentativas: " + tentativas);

        if (dataBaseManager != null)
        {
            dataBaseManager.AtualizarDadosJogador(id, tempoEmSegundos, tentativas);
            Debug.Log("Dados atualizados para o jogador de ID: " + id);
        }
        else
        {
            Debug.Log("Erro: dataBaseManager não foi inicializado!");
        }
    }
}