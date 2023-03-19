using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public int Score = 0;
    public Text bulletText;

    public Slider hpBar;
    public float maxHP;
    private float currentHP;

    public int bulletQuantity = 100;
    public int currentBulletQuantity;

    public GameObject objectPrefab; // Prefab của đối tượng cần tạo ra
    public Transform spawnAreaMin; // Đối tượng đại diện cho điểm bắt đầu của vùng spawn
    public Transform spawnAreaMax; // Đối tượng đại diện cho điểm kết thúc của vùng spawn

    public int numberOfObjectsToSpawn = 10; // Số lượng đối tượng cần tạo ra
    public float spawnDelay = 0.1f; // Thời gian chờ giữa mỗi lần tạo ra đối tượng mới

    private int spawnedObjectCount = 0; // Số lượng đối tượng đã tạo ra

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && spawnedObjectCount < numberOfObjectsToSpawn)
        {
            StartCoroutine(SpawnObjects());
        }
    }

    IEnumerator SpawnObjects()
    {
        while (spawnedObjectCount < numberOfObjectsToSpawn)
        {
            // Sinh ra vị trí ngẫu nhiên trong vùng spawn
            Vector3 spawnPosition = new Vector3(Random.Range(spawnAreaMin.position.x, spawnAreaMax.position.x),
                Random.Range(spawnAreaMin.position.y, spawnAreaMax.position.y),
                Random.Range(spawnAreaMin.position.z, spawnAreaMax.position.z));
            // Sinh ra đối tượng mới tại vị trí vừa được sinh ra
            Instantiate(objectPrefab, spawnPosition, Quaternion.identity);

            // Tăng số lượng đối tượng đã tạo ra và chờ đợi 0,1 giây trước khi tạo ra đối tượng tiếp theo
            spawnedObjectCount++;
            yield return new WaitForSeconds(spawnDelay);
        }

        StopCoroutine(SpawnObjects());
        if (spawnedObjectCount >= numberOfObjectsToSpawn)
        {
            spawnedObjectCount = 0;
        }
    }

    private void FixedUpdate()
    {
        if (Score == 10)
        {
            SceneManager.LoadScene(1);
        }
    }

    private void Start()
    {
        currentBulletQuantity = bulletQuantity;
        bulletText.text = currentBulletQuantity + " / " + bulletQuantity;

        hpBar.maxValue = maxHP;
        currentHP = 4;
        hpBar.value = currentHP;
    }

    public void CollectCoint()
    {
        // Score++;
        // this.UpdateScoreText();

        currentHP++;
        hpBar.value = currentHP;
    }

    public void minusBulletQuantity()
    {
        if (currentBulletQuantity <= 0) return;
        currentBulletQuantity--;
        bulletText.text = currentBulletQuantity + " / " + bulletQuantity;
    }
}