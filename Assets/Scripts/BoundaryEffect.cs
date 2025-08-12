using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
                            << BoundaryEffect >>

        - ��輱 ������ ȿ�� ����
 */

public class BoundaryEffect : MonoBehaviour
{
    [SerializeField] private GameObject boundaryLine;

    public SpriteRenderer boundaryRenderer;


    private void Awake()
    {
        boundaryRenderer = boundaryLine.GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        StartCoroutine(BlinkingBoundary());
    }

    // ��輱 ������ ȿ��
    private IEnumerator BlinkingBoundary()
    {
        while(true)
        {
            // ���İ��� 0~1 ����
            float alpha = Mathf.PingPong(Time.time * 2f, 1f);   // 1�ʸ��� ������
            Color color = boundaryRenderer.color;
            boundaryRenderer.color = new Color(color.r, color.g, color.b, alpha);

            yield return null;
        }
    }
   
}
