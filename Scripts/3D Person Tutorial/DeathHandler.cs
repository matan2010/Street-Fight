using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeathHandler : MonoBehaviour
{
    [SerializeField] Canvas gameOverCanvas;
    //[SerializeField] GameObject gameOver;
    private void Start()
    {
        gameOverCanvas.enabled = false;
      //  gameOver.SetActive(false);
    }
    public void HandleDeath()
    {
        GetComponent<ThirdPersonController>().enabled = false;
        StartCoroutine(WaitSeconds());
        FindObjectOfType<EventSystem>().GetComponent<MyEvent>().SetFirstSelected();
        
    }
    IEnumerator WaitSeconds()
    {
        //gameOver.SetActive(true);
        gameOverCanvas.enabled = true;
        yield return new WaitForSeconds(1.5f);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
