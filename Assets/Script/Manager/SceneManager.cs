using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    //Design pattern
    private static SceneManager instance;
    public static SceneManager Instance { get => instance; }

    private void Awake()
    {
        //Design pattern
        if (instance != null) Debug.LogError("Only 1 SceneManager allows to exist!");
        instance = this;
    }
}
