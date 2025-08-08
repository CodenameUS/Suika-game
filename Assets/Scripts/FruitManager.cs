using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitManager : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;              // 과일 생성 위치
    [SerializeField] private GameObject fruitPrefab;            // 과일 프리팹

    public Fruit currentFruit;                                  // 현재 과일

    private static FruitManager instance;
    public static FruitManager Instance => instance;


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void Start()
    {
        SetFruit();
    }

    // 과일 생성
    private Fruit SpawnFruit()
    {
        GameObject spawnedFruit = Instantiate(fruitPrefab, spawnPoint);

        return spawnedFruit.GetComponent<Fruit>();
    }

    // 과일 설정
    private void SetFruit()
    {
        Fruit fruit = SpawnFruit();
        currentFruit = fruit;

        StartCoroutine(WaitForNextFruit());
    }

    // 드래그
    public void TouchDown()
    {
        if (currentFruit == null) return;

        currentFruit.Drag();
    }

    // 드랍
    public void TouchUp()
    {
        if (currentFruit == null) return;

        currentFruit.Drop();
        currentFruit = null;
    }

    // 과일이 드랍된 후 다음 과일 세팅
    IEnumerator WaitForNextFruit()
    {
        while(currentFruit != null)
        {
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        SetFruit();
    }
}
