using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject basicZombiePrefab;
    
    [SerializeField]
    private PlayerController playerController;

    [SerializeField]
    private float spawnerInterval = 3.5f;

    // Start is called before the first frame update
    void Start()
    {
        basicZombiePrefab.GetComponent<AiController>().playerController = playerController;
        StartCoroutine(spawnEnemy(spawnerInterval, basicZombiePrefab));
    }

    private IEnumerator spawnEnemy(float interval,GameObject enemy)
    {
        
        yield return new WaitForSeconds(interval);
        GameObject newEnemy = Instantiate(enemy, new Vector3(Random.Range(-50f, 50), Random.Range(-6f, 6f), Random.Range(-50f, 50)), Quaternion.identity);
        StartCoroutine(spawnEnemy(interval, enemy));
    }
}
