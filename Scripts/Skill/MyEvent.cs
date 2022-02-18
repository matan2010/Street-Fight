using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MyEvent : MonoBehaviour
{
    [SerializeField]
    private GameObject playAgain;

    public void SetFirstSelected()
    {
        GetComponent<EventSystem>().SetSelectedGameObject(playAgain);
        GetComponent<EventSystem>().firstSelectedGameObject = playAgain;
    }
}
