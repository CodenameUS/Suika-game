using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
                            << FruitManager >>

        - 현재/다음 과일의 생성 및 관리(풀링)
            - 다음 과일은 UI에 이미지로 표시
        
        - 터치 & 드래그 이벤트 관리
 */


public class FruitManager : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;                  // 과일 생성 위치
    [SerializeField] private Transform poolParent;                  // 풀링 위치
    [SerializeField] private GameObject[] fruitPrefabs;             // 과일 프리팹
    [SerializeField] private Sprite[] fruitSprites;                 // 과일 이미지
    [SerializeField] private Image nextFruitImage;                  // 다음 과일 이미지

    #region ** Fields **
    public Fruit currentFruit;                                      // 현재 과일
    public bool isWaitingNextFruit = false;                         // 중복방지용 플래그
    public int watermelonCount;                                     // 수박 갯수(2개가 되면 목표달성)

    private Fruit nextFruit;                                        // 다음 과일
    private int nextFruitLevel;                                     // 다음 과일 레벨
    private int poolSize = 10;                                      // 과일 풀 사이즈(과일별)
    private Dictionary<int, Queue<Fruit>> fruitPool = new();        // 과일 풀
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
   
    // 풀 초기화
    private void InitPool()
    {
        // 과일 레벨별
        for(int i = 0;i<fruitPrefabs.Length;i++)
        {
            fruitPool[i] = new Queue<Fruit>();

            // 과일당 10개
            for(int j = 0;j<poolSize;j++)
            {
                GameObject obj = Instantiate(fruitPrefabs[i], poolParent);
                obj.SetActive(false);
                Fruit fruit = obj.GetComponent<Fruit>();
                fruitPool[i].Enqueue(fruit);
            }
        }
    }

    // 풀에서 과일 꺼내쓰기
    private Fruit GetFruitFromPool(int level)
    {
        // 풀에 여분이 있을 때
        if(fruitPool[level].Count > 0)
        {
            Fruit fruit = fruitPool[level].Dequeue();
            fruit.gameObject.SetActive(true);
            return fruit;
        }
        // 풀에 여분이 없을 때
        else
        {
            GameObject obj = Instantiate(fruitPrefabs[level]);
            return obj.GetComponent<Fruit>();
        }
    }

    // 사용한 과일을 풀에 반환하기
    public void ReturnFruitToPool(Fruit fruit)
    {
        fruit.gameObject.SetActive(false);
        fruitPool[fruit.level].Enqueue(fruit);
    }

    // 과일 생성
    private Fruit SpawnFruitObj(int level)
    {
        Fruit fruit = GetFruitFromPool(level);

        // 위치 설정
        fruit.transform.position = spawnPoint.position;
        fruit.transform.rotation = Quaternion.identity;

        // 드랍되기 전까지 비활성화
        fruit.GetComponent<Rigidbody2D>().simulated = false;

        return fruit;
    }

    
    // 다음 과일 만들기
    private void MakeFruit()
    {
        // 게임 시작시
        if (currentFruit == null)
        {
            // 제일 처음 과일은 체리
            currentFruit = SpawnFruitObj(0);

        }
        else
        {
            currentFruit = SpawnFruitObj(nextFruit.level);
        }

        // 랜덤(체리~오렌지)
        nextFruitLevel = Random.Range(0, 5);
        // 다음에 올 과일
        nextFruit = fruitPrefabs[nextFruitLevel].GetComponent<Fruit>();
        // 다음에 올 과일 아이콘 설정
        nextFruitImage.sprite = fruitSprites[nextFruitLevel];
    }

    // 다음 레벨 과일 생성
    public void SpawnNextLevel(int level, Vector3 position)
    {
        Fruit fruit = GetFruitFromPool(level + 1);
        fruit.transform.position = position;
        fruit.transform.rotation = Quaternion.identity;
        fruit.GetComponent<Rigidbody2D>().simulated = true;
    }

    // 드래그
    public void TouchDown()
    {
        if (currentFruit == null || isWaitingNextFruit) return;

        currentFruit.Drag();
    }

    // 드랍
    public void TouchUp()
    {
        if (currentFruit == null || isWaitingNextFruit) return;

        currentFruit.Drop();
        StartCoroutine(WaitForNextFruit());
    }

    // 1초 딜레이 후 과일 세팅
    IEnumerator WaitForNextFruit()
    {
        isWaitingNextFruit = true;

        yield return new WaitForSeconds(1f);        

        MakeFruit();
        isWaitingNextFruit = false;
    }
}
