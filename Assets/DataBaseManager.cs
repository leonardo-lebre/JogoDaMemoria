using UnityEngine;
using System.IO;
using SQLite;


public class DataBaseManager : MonoBehaviour
{
    private SQLiteConnection connection;

    private string databasePath;

    void Start() {
        string dbPath = Path.Combine(Application.persistentDataPath, "gameData.db");

        connection = new SQLiteConnection(dbPath);

        connection.CreateTable<PlayerData>();

        Debug.Log("Banco de dados está em: " + Application.persistentDataPath);

    }

    public int SalvarDados(string playerName, string email, float tempoDecorrido, int tentativas) {
        var playerData = new PlayerData {
            playerName = playerName,
            email = email,
            tempoDecorrido = 0,
            tentativas = 0
        };

        connection.Insert(playerData);

        int id = playerData.id;

        Debug.Log("Dados salvos com sucesso! ID: " + id);

        connection.Close(); 

        return id;
    }

    public void AtualizarDadosJogador(int id, float tempoDecorrido, int tentativas) {
        var player = connection.Table<PlayerData>().Where(x => x.id == id).FirstOrDefault();

        if (player != null)
        {
            player.tempoDecorrido = tempoDecorrido;
            player.tentativas = tentativas;
            connection.Update(player);

            Debug.Log("Dados do jogador de id: " + id + " atualizados com secesso!");
        }
        else
        {
            Debug.Log("Nenhum jogador encontrado com esse id!");
        }

        connection.Close(); 
        
    }
}

public class PlayerData
{
    [PrimaryKey, AutoIncrement] 
    public int id { get; set; }
    public string playerName { get; set; }
    public string email { get; set; }
    public float tempoDecorrido { get; set; }
    public int tentativas { get; set; }
}
