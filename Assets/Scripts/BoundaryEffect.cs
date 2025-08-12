using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
                            << BoundaryEffect >>

        - 경계선 깜빡임 효과 구현
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

    // 경계선 깜빡임 효과
    private IEnumerator BlinkingBoundary()
    {
        while(true)
        {
            // 알파값을 0~1 증감
            float alpha = Mathf.PingPong(Time.time * 2f, 1f);   // 1초마다 깜빡임
            Color color = boundaryRenderer.color;
            boundaryRenderer.color = new Color(color.r, color.g, color.b, alpha);

            yield return null;
        }
    }
   
}
