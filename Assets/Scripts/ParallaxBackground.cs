using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{   //TODO:控制背景跟随摄像机做变化

    private GameObject cam;

    [SerializeField] private float parallaxEffect;

    private float xPostion;
    private float length;
    void Start()
    {
        cam = GameObject.Find("Main Camera");
        xPostion = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceMoved = cam.transform.position.x * (1 - parallaxEffect);
        float distanceToMove = cam.transform.position.x * parallaxEffect;
        transform.position = new Vector3(xPostion+distanceToMove, transform.position.y);

        if(distanceMoved > xPostion+length)
        {   
            //超过时需要做一个移动,此时向右移动
            xPostion += length;
        }else if (distanceMoved < xPostion-length)
        {
            xPostion -= length;
        }
    }
}
