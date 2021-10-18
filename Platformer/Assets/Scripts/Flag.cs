using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Flag : MonoBehaviour
{
    [SerializeField] string _sceneName;

    void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<Player>();
        if (player == null)
            return;

        var animator = GetComponent<Animator>();
        animator.SetTrigger("Raise");

        //load new level
        StartCoroutine(LoadAfterDelay());
        
    }

    IEnumerator LoadAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(_sceneName);
    }
}
