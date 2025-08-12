using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
                            << FruitManager >>

        - ����/���� ������ ���� �� ����(Ǯ��)
            - ���� ������ UI�� �̹����� ǥ��
        
        - ��ġ & �巡�� �̺�Ʈ ����
 */


public class FruitManager : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;                  // ���� ���� ��ġ
    [SerializeField] private Transform poolParent;                  // Ǯ�� ��ġ
    [SerializeField] private GameObject[] fruitPrefabs;             // ���� ������
    [SerializeField] private Sprite[] fruitSprites;                 // ���� �̹���
    [SerializeField] private Image nextFruitImage;                  // ���� ���� �̹���

    #region ** Fields **
    public Fruit currentFruit;                                      // ���� ����
    public bool isWaitingNextFruit = false;                         // �ߺ������� �÷���
    public int watermelonCount;                                     // ���� ����(2���� �Ǹ� ��ǥ�޼�)

    private Fruit nextFruit;                                        // ���� ����
    private int nextFruitLevel;                                     // ���� ���� ����
    private int poolSize = 10;                                      // ���� Ǯ ������(���Ϻ�)
    private Dictionary<int, Queue<Fruit>> fruitPool = new();        // ���� Ǯ
    private static FruitManager instance;
    #endregion

    public static FruitManager Instance => instance;


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        InitPool();
    }

    private void Start()
    {
        MakeFruit();
    }
   
    // Ǯ �ʱ�ȭ
    private void InitPool()
    {
        // ���� ������
        for(int i = 0;i<fruitPrefabs.Length;i++)
        {
            fruitPool[i] = new Queue<Fruit>();

            // ���ϴ� 10��
            for(int j = 0;j<poolSize;j++)
            {
                GameObject obj = Instantiate(fruitPrefabs[i], poolParent);
                obj.SetActive(false);
                Fruit fruit = obj.GetComponent<Fruit>();
                fruitPool[i].Enqueue(fruit);
            }
        }
    }

    // Ǯ���� ���� ��������
    private Fruit GetFruitFromPool(int level)
    {
        // Ǯ�� ������ ���� ��
        if(fruitPool[level].Count > 0)
        {
            Fruit fruit = fruitPool[level].Dequeue();
            fruit.gameObject.SetActive(true);
            return fruit;
        }
        // Ǯ�� ������ ���� ��
        else
        {
            GameObject obj = Instantiate(fruitPrefabs[level]);
            return obj.GetComponent<Fruit>();
        }
    }

    // ����� ������ Ǯ�� ��ȯ�ϱ�
    public void ReturnFruitToPool(Fruit fruit)
    {
        fruit.gameObject.SetActive(false);
        fruitPool[fruit.level].Enqueue(fruit);
    }

    // ���� ����
    private Fruit SpawnFruitObj(int level)
    {
        Fruit fruit = GetFruitFromPool(level);

        // ��ġ ����
        fruit.transform.position = spawnPoint.position;
        fruit.transform.rotation = Quaternion.identity;

        // ����Ǳ� ������ ��Ȱ��ȭ
        fruit.GetComponent<Rigidbody2D>().simulated = false;

        return fruit;
    }

    
    // ���� ���� �����
    private void MakeFruit()
    {
        // ���� ���۽�
        if (currentFruit == null)
        {
            // ���� ó�� ������ ü��
            currentFruit = SpawnFruitObj(0);

        }
        else
        {
            currentFruit = SpawnFruitObj(nextFruit.level);
        }

        // ����(ü��~������)
        nextFruitLevel = Random.Range(0, 5);
        // ������ �� ����
        nextFruit = fruitPrefabs[nextFruitLevel].GetComponent<Fruit>();
        // ������ �� ���� ������ ����
        nextFruitImage.sprite = fruitSprites[nextFruitLevel];
    }

    // ���� ���� ���� ����
    public void SpawnNextLevel(int level, Vector3 position)
    {
        Fruit fruit = GetFruitFromPool(level + 1);
        fruit.transform.position = position;
        fruit.transform.rotation = Quaternion.identity;
        fruit.GetComponent<Rigidbody2D>().simulated = true;
    }

    // �巡��
    public void TouchDown()
    {
        if (currentFruit == null || isWaitingNextFruit) return;

        currentFruit.Drag();
    }

    // ���
    public void TouchUp()
    {
        if (currentFruit == null || isWaitingNextFruit) return;

        currentFruit.Drop();
        StartCoroutine(WaitForNextFruit());
    }

    // 1�� ������ �� ���� ����
    IEnumerator WaitForNextFruit()
    {
        isWaitingNextFruit = true;

        yield return new WaitForSeconds(1f);        

        MakeFruit();
        isWaitingNextFruit = false;
    }
}
