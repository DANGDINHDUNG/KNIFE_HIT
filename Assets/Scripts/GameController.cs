using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(GameUI))]
public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    [SerializeField] private int knifeCount;
    [SerializeField] private int currentKnifeCount;

    [Header("Knife Spawning")]
    [SerializeField] private Vector2 knifeSpawnPosition;
    [SerializeField] private GameObject knifeObject;

    [Header("Stage Controller")]

    // Số lượng wave mỗi stage
    [SerializeField] private int waveCount;

    // Số lượng stage
    [SerializeField] public int stageCount;

    // Danh sách các vật thể trong game
    [SerializeField] private List<GameObject> listOfWoods;
    [SerializeField] private List<GameObject> listOfBosses;
    [SerializeField] private GameObject currentWood;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI stageTMP;
    [SerializeField] public int appleScore;
    [SerializeField] private TextMeshProUGUI scoreTMP;
    [SerializeField] private GameObject notification;

    public GameUI GameUI { get; private set; }


    private void Awake()
    {
        currentKnifeCount = knifeCount + stageCount - 1;
        Instance = this;
        waveCount = 0;
        appleScore = 0;
        stageCount = 1;
        GameUI = GetComponent<GameUI>();
    }

    private void Update()
    {
        scoreTMP.text = appleScore.ToString();
    }

    private void Start()
    {
        GameUI.ShowKnifeCount(knifeCount);
        SpawnWood();
    }

    public void OnSuccessfulKnifeHit()
    {
        if (currentKnifeCount > 0)
        {
            SpawnKnife();
        }
        else
        {
            StartGameOverSequence(true);
        }

    }

    private void SpawnKnife()
    {
        currentKnifeCount--;
        Instantiate(knifeObject, knifeSpawnPosition, Quaternion.identity);
    }

    public void StartGameOverSequence(bool win)
    {
        StartCoroutine(GameOverSequenceCoroutine(win));
    }

    private IEnumerator GameOverSequenceCoroutine(bool win)
    {
        if (win)
        {
            Destroy(currentWood);
            yield return new WaitForSeconds(0.3f);
            SpawnWood();
        }
        else
        {
            GameUI.ShowRestartButton();
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    private void SpawnWood()
    {
        waveCount++;

        currentKnifeCount = knifeCount + stageCount - 1;
        GameUI.ResetKnifeCount(currentKnifeCount);
        SpawnKnife();
        if (waveCount == 5)
        {
            StartCoroutine(BossAppear());
        }
        else
        {
            AudioManager.audioInstance.Play("SpawnWood");
            // Chọn ngẫu nhiên một vật thể trong danh sách
            GameObject wood = listOfWoods[Random.Range(0, listOfWoods.Count)];
            // Tạo một vật thể mới
            currentWood = Instantiate(wood);

            if (waveCount > 5)
            {
                stageCount++;
                waveCount = 1;
                GameUI.IncreasingKnifeCount();
            }
        }

        stageTMP.SetText("Stage {0} - Wave {1}", stageCount, waveCount);
    }

    IEnumerator BossAppear()
    {
        notification.SetActive(true);
        AudioManager.audioInstance.Play("BossAppear");
        yield return new WaitForSeconds(0.5f);
        notification.SetActive(false);
        // Chọn ngẫu nhiên một vật thể trong danh sách
        GameObject wood = listOfBosses[Random.Range(0, listOfWoods.Count)];
        // Tạo một vật thể mới
        currentWood = Instantiate(wood);
    }
}
