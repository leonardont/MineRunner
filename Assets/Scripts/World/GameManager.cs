using System.Collections;
using System.Collections.Generic; // List
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Gerencia a gameplay principal do jogo
/// </summary>
public class GameManager : MonoBehaviour
{
    [System.NonSerialized] public int playerScore, highScore = 0;
    public TextMeshProUGUI playerScoreText, playerScoreTextPause, highScoreTextPause, playerScoreTextGameOver, highScoreTextGameOver;

    [Tooltip("Uma referência ao tile que queremos spawnar")]
    public Transform tile;

    [Tooltip("Uma referência ao obsátculo que que queremos spawnar")]
    public Transform obstacle;

    [Tooltip("Onde o primeiro tile deverá ser posicionado")]
    public Vector3 startPoint = new Vector3(0, 0, -5);

    [Tooltip("Quantos tiles devemos criar desde o começo")]
    [Range (1, 20)]
    public int initSpawnNum = 10;

    [Tooltip("Quantos tiles a criar sem obstáculos")]
    [Range (1, 10)]
    public int initNoObstacles = 4;

    /// <summary>
    /// Onde o próximo tile deverá ser spawnado
    /// </summary>
    private Vector3 nextTileLocation;

    /// <summary>
    /// Como deverá ser rotacionado o próximo tile?
    /// </summary>
    private Quaternion nextTileRotation;

    /// <summary>
    /// Função start é chamada antes da primeira atualização de frame
    /// </summary>
    void Start()
    {
        highScore = PlayerPrefs.GetInt("highScore");

        /// Pega o volume salvo pelo jogador para tocar as músicas
        AudioListener.volume = PlayerPrefs.GetFloat("volume");

        /// Fixa o ponto de início
        nextTileLocation = startPoint;
        nextTileRotation = Quaternion.identity;

        for (int i = 0; i < initSpawnNum; ++i)
        {
            SpawnNextTile(i >= initNoObstacles);
        }
    }

    void FixedUpdate()
    {
        playerScoreText.text = playerScore.ToString();
        playerScoreTextPause.text = playerScore.ToString();
        highScoreTextPause.text = highScore.ToString();
        playerScoreTextGameOver.text = playerScore.ToString();
        highScoreTextGameOver.text = highScore.ToString();

        if (GameObject.FindWithTag("Player"))
        {
            playerScore += (int)GameObject.FindWithTag("Player").GetComponent<Rigidbody>().velocity.z;
        }
        else
        {
            if (playerScore >= PlayerPrefs.GetInt("highScore"))
            {
                highScoreTextGameOver.text = playerScore.ToString();
                PlayerPrefs.SetInt("highScore", playerScore);
            }
        }
    }

    /// <summary>
    /// Irá spawnar um tile em um local específico
    /// e configurará a próxima posição
    /// </summary>
    /// <param name="spawnObstacles">Se deverá spawnar um obstáculo</param>
    public void SpawnNextTile(bool spawnObstacles = true) 
    {
        var newTile = Instantiate(tile, nextTileLocation, nextTileRotation);

        /// Calcula onde e em qual rotação deverá spawnar o próximo item
        var nextTile = newTile.Find("NextSpawnPoint");
        nextTileLocation = nextTile.position;
        nextTileRotation = nextTile.rotation;

        if (spawnObstacles)
        {
            SpawnObstacle(newTile);
        }
    }

    private void SpawnObstacle(Transform newTile)
    {
        // É preciso todos os lugares possíveis para o spawn de um novo obstáculo
        var obstacleSpawnPoints = new List<GameObject>();

        // Passa por cada um dos game objects filhos no tile
        foreach(Transform child in newTile)
        {
            // Se tiver a tag ObstacleSpawn...
            if(child.CompareTag("ObstacleSpawn"))
            {
                // Adiciona como uma possiblidade de spawn
                obstacleSpawnPoints.Add(child.gameObject);
            }
        }

        // Confirma que tem pelo menos um
        if (obstacleSpawnPoints.Count > 0)
        {
            // Pega um ponto de spawn aleatório dos presentes
            int index = Random.Range(0, obstacleSpawnPoints.Count);
            var spawnPoint = obstacleSpawnPoints[index];

            // Guarda a posição para usar
            var spawnPos = spawnPoint.transform.position;

            // Cria o obstáculo
            var newObstacle = Instantiate(obstacle, spawnPos, Quaternion.identity);

            // Fixa com parentesco ao tile
            newObstacle.SetParent(spawnPoint.transform);
        }
    }
}
