using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Absorb : MonoBehaviour
{
    public float absorbRadius=3f;
    private SphereCollider _absorbArea;
    private PlanetController _fatherObj;
    private Transform trans;
    [SerializeField] private GameObject absorbedItem;
    
#if UNITY_EDITOR
    private void OnValidate()
    {
        _fatherObj = GetComponent<PlanetController>();
        _absorbArea = GetComponents<SphereCollider>().
            First(sphereCollider => sphereCollider.isTrigger);
        _absorbArea.radius = absorbRadius;
    }
#endif
    

    private void OnTriggerStay(Collider other)
    {
        // IEnumerator InstantiateAbsorbed()
        // {
        //     while (other.CompareTag("Planet"))
        //     {
        //         GameObject obj=Instantiate(absorbedItem, other.transform.position, other.transform.rotation);
        //         AbsorbedMove absorbedObj = obj.GetComponent<AbsorbedMove>();
        //         absorbedObj.SetFather(_fatherObj);
        //         Debug.Log("ins");
        //         yield return new WaitForSeconds(0.3f);
        //     }
        // }

        // if (other.CompareTag("Planet"))
        // {
        //     GameObject obj=Instantiate(absorbedItem, other.transform.position, other.transform.rotation);
        //     AbsorbedMove absorbedObj = obj.GetComponent<AbsorbedMove>();
        //     absorbedObj.SetFather(_fatherObj);
        // }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Planet"))
        {
            trans = other.transform;
            InvokeRepeating("IniAbsorbed", 0.3f, 0.3f);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        CancelInvoke();
    }

    private void IniAbsorbed()
    {
        GameObject obj=Instantiate(absorbedItem, trans.transform.position, trans.transform.rotation);
        AbsorbedMove absorbedObj = obj.GetComponent<AbsorbedMove>();
        absorbedObj.SetFather(_fatherObj);
    }

    
}
