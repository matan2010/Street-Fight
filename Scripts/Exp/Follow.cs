using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform target;
    public float minModifier = 7;
    public float maxModifier = 11;

    Vector3 velocity = Vector3.zero;
    bool isFollwing = false;
 

    public void StartFollowing()
    {
        isFollwing = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFollwing)
        {
            transform.position = Vector3.SmoothDamp(transform.position, target.position, ref velocity, Time.deltaTime * Random.Range(minModifier, maxModifier));
        }
    }
}
