using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IsEnabled : MonoBehaviour
{
    public int NeedToUnlock;
    public Material blackMaterial;
    public RaycastHit hit;
    private List<Material> posibleCubes;
    private void Start()
    {
        if (PlayerPrefs.GetInt("score") < NeedToUnlock)
            GetComponent<MeshRenderer>().material = blackMaterial;
    }

    private void Update()
    {
       

    }

}
