using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class FlagManager : MonoBehaviour
{
    [SerializeField] Animator animatorFlag;

    [SerializeField] private float sceneChangeDelay = 3f;

    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasTriggered && collision.CompareTag("Player"))
        {
            hasTriggered = true;

            if (animatorFlag != null)
            {
                animatorFlag.SetInteger("State", 1);
            }

            StartCoroutine(DelayAndLoadNextScene());
        }
    }

    private IEnumerator DelayAndLoadNextScene()
    {
        yield return new WaitForSeconds(sceneChangeDelay);

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("End scense");
        }
    }
}