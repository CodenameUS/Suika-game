using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
                            << Fruit >>

        - ���� ������
            - ��ġ & �巡��
        
        - ���� ������
            - Collsion �̺�Ʈ�� ����

        - ��輱 ����Ʈ Ȱ��ȭ ��û �� ���ӿ���
            - Trigger �̺�Ʈ�� ����
 */


public class Fruit : MonoBehaviour
{
    [SerializeField] private GameObject dropLine;           // �����ġ ǥ����

    private bool isDrag;                                    // �巡�� ����
    private bool isMerge;                                   // ������ ����
    private float boundaryEffectTimer = 0f;                 // ��輱 Ȱ��ȭ Ÿ�̸�
    private float deadTimer = 0f;                           // ���ӿ��� Ÿ�̸�

    public int level;                                       // ���� ����

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

    // ���� ��ġ��
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Fruit")) return;

        Fruit other = collision.gameObject.GetComponent<Fruit>();

        // ���� ���ϸ� ��ġ��
        if (level == other.level && !isMerge && !other.isMerge && level < 10)
        {
            // �� ������ �߰���ġ ���
            Vector3 midPos = (transform.position + other.transform.position) / 2f;

            isMerge = true;
            other.isMerge = true;

            FruitManager.Instance.ReturnFruitToPool(this);
            FruitManager.Instance.ReturnFruitToPool(other);

            // ���� ���� ���� ����
            FruitManager.Instance.SpawnNextLevel(level, midPos);
            AudioManager.Instance.PlaySFX(AudioManager.Sfx.Pop);

            // ���� ȹ��
            AddScore();

            // ����(����10)�� �ϼ������� ��
            if(level == 9)
            {
                FruitManager.Instance.watermelonCount++;

                // ������ �ΰ� ���������� ���� ����
                if(FruitManager.Instance.watermelonCount == 2)
                {
                    GameManager.Instance.GameClear();
                }
            }
        }
    }

    
    private void OnTriggerStay2D(Collider2D collision)
    {
        // ��輱 Ȱ��ȭ
        if (collision.CompareTag("BoundaryEffect")) 
        {
            boundaryEffectTimer += Time.deltaTime;
            if (boundaryEffectTimer > 1f)
            {
                collision.GetComponent<BoundaryEffect>().boundaryRenderer.enabled = true;
            }
        }
        // ���� ����
        else if(collision.CompareTag("Boundary"))
        {
            deadTimer += Time.deltaTime;
            if (deadTimer > 4f)
            {
                GameManager.Instance.GameOver();
            }
        }
    }

    // ��輱 ��Ȱ��ȭ
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

    // ���� �����̱�
    private void MoveFruit()
    {
        if (!isDrag || FruitManager.Instance.isWaitingNextFruit) return;

        // ���콺 ������ ���󰡱�
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // ���η� �ִ� �̵������� ����
        float minX = -1.96f + transform.localScale.x / 2f;
        float maxX = 1.96f - transform.localScale.x / 2f;
        float clampedX = Mathf.Clamp(mousePos.x, minX, maxX);

        // ���ηθ� �̵�
        mousePos.z = 0;
        mousePos.y = transform.position.y;
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }
    
    // ���� ȹ��
    private void AddScore()
    {
        GameManager.Instance.score += (level+1) * 2 + 1;
    }

    // �巡��
    public void Drag()
    {
        isDrag = true;
        dropLine.SetActive(true);
    }

    // ���� ���
    public void Drop()
    {
        isDrag = false;
        dropLine.SetActive(false);
        rigid.simulated = true;
    }
}
