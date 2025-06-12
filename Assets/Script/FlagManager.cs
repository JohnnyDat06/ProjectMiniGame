using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class FlagManager : MonoBehaviour
{
    //FlagManager
    [Header("Flag Settings")]
    [SerializeField] Animator animatorFlag;

    private bool hasTriggered = false;
    //When the player touches the flag, the animation will be performed and the DelaySceneChange() function will be called
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
    //Wait 3 seconds for the flag variable to be animated and then loadscene
    private IEnumerator DelaySceneChange()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("ThuanTest");
    }
}
