using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitioner : MonoBehaviour
{
    public Transform[] stuffToDisable;
    public bool enableOnStart;
    public string nextScene;

    public static SceneTransitioner instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (enableOnStart)
            EnableSceneTransition();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnableSceneTransition()
    {
        foreach (Transform transform in stuffToDisable)
        {
            transform.gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            SceneManager.LoadScene(nextScene);
        }
    }
}
