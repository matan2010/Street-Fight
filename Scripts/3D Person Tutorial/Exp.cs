using UnityEngine;
using UnityEngine.UI;

public class Exp : MonoBehaviour
{
    private int myExpPoint=0;
    public Text exp;

    private void Start()
    {
        exp.text = myExpPoint.ToString();
    }
    public void GetExp(int expPoint)
    {
        myExpPoint += expPoint;
        exp.text = myExpPoint.ToString();
    }
}
