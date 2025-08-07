using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    public bool isDrag;
    private Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        MoveFruit();
    }

    private void MoveFruit()
    {
        if (!isDrag) return;

        // 마우스 포인터 따라가기
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // 가로로 최대 이동가능한 영역
        float minX = -2.4f + transform.localScale.x / 2f;
        float maxX = 2.4f - transform.localScale.x / 2f;
        float clampedX = Mathf.Clamp(mousePos.x, minX, maxX);

        // 가로로만 이동
        mousePos.z = 0;
        mousePos.y = transform.position.y;
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }
    
    // 드래그
    
    public void Drag()
    {
        isDrag = true;
    }

    // 과일 드랍
    public void Drop()
    {
        isDrag = false;
        rigid.simulated = true;
    }
}
