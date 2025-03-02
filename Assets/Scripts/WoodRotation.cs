using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodRotation : MonoBehaviour
{
    // Tốc độ quay của vật thể
    [SerializeField] private float Speed;

    // Tốc độ quay hiện tại
    [SerializeField] private float currentSpeed;

    // Thời gian quay
    [SerializeField] private float duration;

    // Thời gian đã trôi qua
    [SerializeField] private float elapsedTime;

    // Thời gian cuối cùng
    [SerializeField] private float finalTime;

    public float circleRadius = 5f; // Bán kính hình tròn
    [SerializeField] private GameObject applePrefab;
    [SerializeField] private GameObject knifePrefab;
    public Transform circleCenter; // Tâm của hình tròn

    private SpeedDatasPerStage speedDatasPerStage = new SpeedDatasPerStage();
    private int currentStage;

    private void Awake()
    {
        SpawnApple();
        if (this.transform.GetChild(0).gameObject.CompareTag("Wood"))
        {
            for (int i = 0; i < GameController.Instance.stageCount - 1; i++)
            {
                SpawnKnife();
            }
        }
        StartCoroutine(PlayRotationPattern());
    }

    private void Update()
    {
        // Tốc độ quay của vật thể giảm dần theo thời gian.
        elapsedTime += Time.deltaTime;

        finalTime = Mathf.Clamp01(elapsedTime / (duration * 1.5f));

        currentSpeed = Mathf.Lerp(Speed, 5f, finalTime);

        // Xoay vòng vật thể.
        transform.Rotate(Vector3.forward * currentSpeed * Time.deltaTime);
    }

    private IEnumerator PlayRotationPattern()
    {
        while (true)
        {
            currentStage = GameController.Instance.stageCount;
            GetRandomValue(speedDatasPerStage.speedDatas[currentStage].maxSpeed, speedDatasPerStage.speedDatas[currentStage].minSpeed);

            duration = Random.Range(speedDatasPerStage.speedDatas[currentStage].minDuration, speedDatasPerStage.speedDatas[currentStage].maxDuration);
            yield return new WaitForSeconds(duration);
            elapsedTime = 0;
        }
    }

    /// <summary>
    /// Tạo giá trị Speed ngẫu nhiên.
    /// </summary>
    /// <param name="maxValue"></param>
    /// <param name="minValue"></param>
    void GetRandomValue(int maxValue, int minValue)
    {
        if (Random.value > 0.5f)
        {
            Speed = Random.Range(-maxValue, -minValue);
        }
        else
        {
            Speed = Random.Range(minValue, maxValue);
        }
    }

    void SpawnApple()
    {
        // Chọn một góc ngẫu nhiên (hoặc cố định nếu muốn)
        float angle = Random.Range(0f, 360f);

        // Tính toán vị trí dựa trên tọa độ cực
        float x = circleCenter.position.x + circleRadius * Mathf.Cos(angle * Mathf.Deg2Rad);
        float y = circleCenter.position.y + circleRadius * Mathf.Sin(angle * Mathf.Deg2Rad);

        // Vị trí mới trên vành hình tròn
        Vector3 spawnPosition = new Vector3(x, y, circleCenter.position.z);

        Vector3 direction = (spawnPosition - circleCenter.position).normalized;

        // Spawn quả táo
        GameObject apple = Instantiate(applePrefab, spawnPosition, Quaternion.identity, circleCenter);
        apple.transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
    }

    void SpawnKnife()
    {
        // Chọn một góc ngẫu nhiên (hoặc cố định nếu muốn)
        float angle = Random.Range(0f, 360f);

        // Tính toán vị trí dựa trên tọa độ cực
        float x = circleCenter.position.x + circleRadius * Mathf.Cos(angle * Mathf.Deg2Rad);
        float y = circleCenter.position.y + circleRadius * Mathf.Sin(angle * Mathf.Deg2Rad);

        // Vị trí mới trên vành hình tròn
        Vector3 spawnPosition = new Vector3(x, y, circleCenter.position.z);

        Vector3 direction = (-spawnPosition + circleCenter.position).normalized;

        // Spawn knife
        GameObject knife = Instantiate(knifePrefab, spawnPosition, Quaternion.identity, circleCenter);
        knife.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        knife.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        knife.transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
    }


}

[System.Serializable]
public class SpeedPerStage
{
    public int maxSpeed;
    public int minSpeed;
    public int maxDuration;
    public int minDuration;
}

[System.Serializable]
public class SpeedDatasPerStage
{
    public Dictionary<int, SpeedPerStage> speedDatas;

    public SpeedDatasPerStage()
    {
        speedDatas = new Dictionary<int, SpeedPerStage>
        {
            { 1, new SpeedPerStage { maxSpeed = 300, minSpeed = 100, maxDuration = 3, minDuration = 8 } },
            { 2, new SpeedPerStage { maxSpeed = 400, minSpeed = 200, maxDuration = 3, minDuration = 6 } },
            { 3, new SpeedPerStage { maxSpeed = 500, minSpeed = 300, maxDuration = 3, minDuration = 6 } },
            { 4, new SpeedPerStage { maxSpeed = 600, minSpeed = 300, maxDuration = 2, minDuration = 6 } },
        };
    }
}


