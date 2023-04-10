using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherPlanet : MonoBehaviour
{
    public float spinSpeed=1000;
    public Material m;
    public float startWidth = 0.1f;
    public float endWidth = 0.1f;
    public int pointNum=360;
    
    private float _pGravity;
    private Vector3 _pos;
    

     private void Start()
     {
         _pos = this.transform.position;
         _pGravity = spinSpeed / Mathf.Pow(Vector3.Distance(_pos, Vector3.zero), 1.5f);
         DrawLineRenderer(Vector3.zero, Vector3.Distance(_pos, Vector3.zero),pointNum);
     }
     
    void FixedUpdate()
    {
        this.transform.RotateAround(Vector3.zero,Vector3.up,_pGravity
            *Time.deltaTime);
    }

    //Draw a circle
    public void DrawLineRenderer(Vector3 centerPos, float r,int pointNum)
    {
        //Creat a line
        GameObject obj = new GameObject();
        obj.name = "R";
        obj.transform.SetParent(this.transform);
        LineRenderer line = obj.AddComponent<LineRenderer>();
        
        line.loop = true;
        line.positionCount = pointNum;
        line.startWidth = startWidth;
        line.endWidth = endWidth;

        line.material = m;
        
        float angle = 360f / pointNum;
        for (int i = 0; i < pointNum; i++)
        {
            line.SetPosition(i,centerPos+Quaternion.AngleAxis(angle*i,Vector3.up)*Vector3.forward*r);
        }
    }
    
}
