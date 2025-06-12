using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class FlagManager : MonoBehaviour
{
    [Header("Flag Settings")]
    [SerializeField] Animator animatorFlag;

    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasTriggered && collision.CompareTag("Player"))
        {
            Debug.Log("Player check!");
            hasTriggered = true;
            animatorFlag.SetInteger("State", 1);
            StartCoroutine(DelaySceneChange());
        }
    }
    private IEnumerator DelaySceneChange()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("ThuanTest");
    }
}
