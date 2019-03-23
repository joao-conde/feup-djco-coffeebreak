using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour {

    public Animator animator;
    
    private int nextScene;

    public void FadeToScene (int sceneIndex) {
        nextScene = sceneIndex;
        animator.SetTrigger ("fadeOut");
    }

    public void OnFadeComplete () {
        SceneManager.LoadScene (nextScene);
    }
}