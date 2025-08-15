using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
                            << Fruit >>

        - 과일 움직임
            - 터치 & 드래그
        
        - 과일 합쳐짐
            - Collsion 이벤트로 구현

        - 경계선 이펙트 활성화 요청 및 게임오버
            - Trigger 이벤트로 구현
 */


public class Fruit : MonoBehaviour
{
    [SerializeField] private GameObject dropLine;           // 드랍위치 표시줄

    private bool isDrag;                                    // 드래그 여부
    private bool isMerge;                                   // 합쳐짐 여부
    private float boundaryEffectTimer = 0f;                 // 경계선 활성화 타이머
    private float deadTimer = 0f;                           // 게임오버 타이머

    public int level;                                       // 과일 레벨

    private Rigidbody2D rigid;
    private Animator anim;

    readonly private int hashLevel = Animator.StringToHash("Level");

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        anim.SetInteger(hashLevel, level);
    }

    private void Update()
    {
        if (GameManager.Instance.isGameOver || GameManager.Instance.isGameClear || GameManager.Instance.isPaused)
            return;

        MoveFruit();
    }

    // 과일 합치기
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Fruit")) return;

        Fruit other = collision.gameObject.GetComponent<Fruit>();

        // 같은 과일만 합치기
        if (level == other.level && !isMerge && !other.isMerge && level < 10)
        {
            // 두 과일의 중간위치 계산
            Vector3 midPos = (transform.position + other.transform.position) / 2f;

            isMerge = true;
            other.isMerge = true;

            FruitManager.Instance.ReturnFruitToPool(this);
            FruitManager.Instance.ReturnFruitToPool(other);

            // 다음 레벨 과일 생성
            FruitManager.Instance.SpawnNextLevel(level, midPos);
            AudioManager.Instance.PlaySFX(AudioManager.Sfx.Pop);

            // 점수 획득
            AddScore();

            // 수박(레벨10)을 완성시켰을 때
            if(level == 9)
            {
                FruitManager.Instance.watermelonCount++;

                // 수박을 두개 만들었을경우 게임 종료
                if(FruitManager.Instance.watermelonCount == 2)
                {
                    GameManager.Instance.GameClear();
                }
            }
        }
    }

    
    private void OnTriggerStay2D(Collider2D collision)
    {
        // 경계선 활성화
        if (collision.CompareTag("BoundaryEffect")) 
        {
            boundaryEffectTimer += Time.deltaTime;
            if (boundaryEffectTimer > 1f)
            {
                collision.GetComponent<BoundaryEffect>().boundaryRenderer.enabled = true;
            }
        }
        // 게임 오버
        else if(collision.CompareTag("Boundary"))
        {
            deadTimer += Time.deltaTime;
            if (deadTimer > 4f)
            {
                GameManager.Instance.GameOver();
            }
        }
    }

    // 경계선 비활성화
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("BoundaryEffect"))
        {
            boundaryEffectTimer = 0f;
            collision.GetComponent<BoundaryEffect>().boundaryRenderer.enabled = false;
        }

        if(collision.CompareTag("Boundary"))
        {
            deadTimer = 0f;
        }
    }

    // 과일 움직이기
    private void MoveFruit()
    {
        if (!isDrag || FruitManager.Instance.isWaitingNextFruit) return;

        // 마우스 포인터 따라가기
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // 가로로 최대 이동가능한 영역
        float minX = -1.96f + transform.localScale.x / 2f;
        float maxX = 1.96f - transform.localScale.x / 2f;
        float clampedX = Mathf.Clamp(mousePos.x, minX, maxX);

        // 가로로만 이동
        mousePos.z = 0;
        mousePos.y = transform.position.y;
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }
    
    // 점수 획득
    private void AddScore()
    {
        GameManager.Instance.score += (level+1) * 2 + 1;
    }

    // 드래그
    public void Drag()
    {
        isDrag = true;
        dropLine.SetActive(true);
    }

    // 과일 드랍
    public void Drop()
    {
        isDrag = false;
        dropLine.SetActive(false);
        rigid.simulated = true;
    }
}
