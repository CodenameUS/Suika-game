using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
                            << FruitManager >>

        - ����/���� ������ ���� �� ����(Ǯ��)
            - ���� ������ UI�� �̹����� ǥ��
        
        - ��ġ & �巡�� �̺�Ʈ ����

        - ���� ����
            - ���� Ȱ��ȭ�Ǿ��ִ� ���ϵ��� ��� Ǯ�� ��ȯ
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
    public Fruit nextFruit;                                         // ���� ����

    public bool isWaitingNextFruit = false;                         // �ߺ������� �÷���
    public int watermelonCount;                                     // ���� ����(2���� �Ǹ� ��ǥ�޼�)

    private int nextFruitLevel;                                     // ���� ���� ����
    private int poolSize = 5;                                      // ���� Ǯ ������(���Ϻ�)

    private Dictionary<int, Queue<Fruit>> fruitPool = new();        // ���� Ǯ
    private List<Fruit> activeFruits = new();                       // Ȱ��ȭ �Ǿ��ִ� ����(���� ���¿�)

    #endregion

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
            activeFruits.Add(fruit);
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
    public void MakeFruit()
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
        nextFruitImage.SetNativeSize();
    }

    // ���� ���� ���� ����
    public void SpawnNextLevel(int level, Vector3 position)
    {
        Fruit fruit = GetFruitFromPool(level + 1);
        fruit.transform.position = position;
        fruit.transform.rotation = Quaternion.identity;
        fruit.GetComponent<Rigidbody2D>().simulated = true;
    }

    // ����� ������ Ǯ�� ��ȯ�ϱ�
    public void ReturnFruitToPool(Fruit fruit)
    {
        fruit.gameObject.SetActive(false);
        fruitPool[fruit.level].Enqueue(fruit);
    }

    // ���� ����۽�
    public void Reset()
    {
        foreach(var fruit in activeFruits)
        {
            ReturnFruitToPool(fruit);
        }

        activeFruits.Clear();
        currentFruit = null;
        nextFruit = null;
        isWaitingNextFruit = false;

        MakeFruit();
    }

    // �巡��
    public void TouchDown()
    {
        if (currentFruit == null || isWaitingNextFruit || GameManager.Instance.isPaused || 
            GameManager.Instance.isGameOver || GameManager.Instance.isGameClear) return;

        currentFruit.Drag();
    }

    // ���
    public void TouchUp()
    {
        if (currentFruit == null || isWaitingNextFruit || GameManager.Instance.isPaused ||
            GameManager.Instance.isGameOver || GameManager.Instance.isGameClear) return;

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
