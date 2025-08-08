using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitManager : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;              // ���� ���� ��ġ
    [SerializeField] private GameObject fruitPrefab;            // ���� ������

    public Fruit currentFruit;                                  // ���� ����

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

    // ���� ����
    private Fruit SpawnFruit()
    {
        GameObject spawnedFruit = Instantiate(fruitPrefab, spawnPoint);

        return spawnedFruit.GetComponent<Fruit>();
    }

    // ���� ����
    private void SetFruit()
    {
        Fruit fruit = SpawnFruit();
        currentFruit = fruit;

        StartCoroutine(WaitForNextFruit());
    }

    // �巡��
    public void TouchDown()
    {
        if (currentFruit == null) return;

        currentFruit.Drag();
    }

    // ���
    public void TouchUp()
    {
        if (currentFruit == null) return;

        currentFruit.Drop();
        currentFruit = null;
    }

    // ������ ����� �� ���� ���� ����
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
