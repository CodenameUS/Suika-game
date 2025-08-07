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

        // ���콺 ������ ���󰡱�
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // ���η� �ִ� �̵������� ����
        float minX = -2.4f + transform.localScale.x / 2f;
        float maxX = 2.4f - transform.localScale.x / 2f;
        float clampedX = Mathf.Clamp(mousePos.x, minX, maxX);

        // ���ηθ� �̵�
        mousePos.z = 0;
        mousePos.y = transform.position.y;
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }
    
    // �巡��
    
    public void Drag()
    {
        isDrag = true;
    }

    // ���� ���
    public void Drop()
    {
        isDrag = false;
        rigid.simulated = true;
    }
}
