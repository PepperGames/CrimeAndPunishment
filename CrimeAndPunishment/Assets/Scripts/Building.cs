using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public Material[] materials;
   
    public void MakeTransparent()
    {
        gameObject.GetComponent<MeshRenderer>().material = materials[0];
    }
    public void MakeItOpaque()
    {
        gameObject.GetComponent<MeshRenderer>().material = materials[1];
    }
}
