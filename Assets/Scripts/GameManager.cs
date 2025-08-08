using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Fruit currentFruit;

    private static GameManager instance;
    public static GameManager Instance => instance;


    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public void TouchDown()
    {
        currentFruit.Drag();
    }

    public void TouchUp()
    {
        currentFruit.Drop();
    }
}
