using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;



public class LineController : MonoBehaviour
{
    public Material m1;
    public Material m2;
    public float startWidth = 0.1f;
    public float endWidth = 0.1f;
    

    private LineRenderer _lineRenderer;
    private LineRenderer _velocityRenderer;
    private GameObject _parent;
    private Rigidbody _rb;
    private PlanetController _planet;
    private float _materialValue;

    private Vector3 _mousePosition;
    
    void OnEnable()
    {
        _parent = GameObject.Find("PlayerPlanet");
        _planet = _parent.GetComponent<PlanetController>();
        
        #region Linerenderer

        GameObject line = new GameObject();
        line.name = "Line";
        line.transform.SetParent(_parent.transform);
        line.transform.position=_parent.transform.position;

        _lineRenderer = line.AddComponent<LineRenderer>();
        
        //Set width
        _lineRenderer.startWidth=startWidth;
        _lineRenderer.endWidth=endWidth;
        //Set color
        _lineRenderer.startColor=Color.white;
        _lineRenderer.endColor=Color.white;
        //Ser material
        _lineRenderer.material=m1;
        //Set the number of points
        _lineRenderer.positionCount = 2;
        
        _lineRenderer.useWorldSpace = true;
        #endregion

        #region VelocityRenderer
        GameObject velocityLine=new GameObject();
        
        velocityLine.name="velocityLine";
        velocityLine.transform.SetParent(_parent.transform);
        velocityLine.transform.position = _parent.transform.position;
        _velocityRenderer=velocityLine.AddComponent<LineRenderer>();
        
        _rb = _parent.GetComponent<Rigidbody>();
        //Set width
        _velocityRenderer.startWidth=startWidth;
        _velocityRenderer.endWidth=endWidth;
        //Set color
        _velocityRenderer.startColor=Color.white;
        _velocityRenderer.endColor=Color.white;
        //Set material
        _velocityRenderer.material=m2;
        //Set points
        _velocityRenderer.positionCount=2;
        
        
        _velocityRenderer.useWorldSpace=false;
        

        #endregion
    }
    
    void Update()
    {
        DrawLine();
        DrawVelocity();
    }

    #region Draw Lines
    private  void DrawLine()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        _mousePosition = ray.origin + ray.direction * ((this.transform.position.y - ray.origin.y) / ray.direction.y);
        _lineRenderer.SetPosition(0,this.transform.position);
        _lineRenderer.SetPosition(1,_mousePosition);
    }

    private void DrawVelocity()
    {
        _velocityRenderer.SetPosition(0,Vector3.zero);
        _velocityRenderer.SetPosition(1,_planet.velocity.normalized*4);
        _materialValue = _planet.velocity.magnitude / 4;
        m2.SetFloat("_stepValue",_materialValue);
    }
    #endregion
}
