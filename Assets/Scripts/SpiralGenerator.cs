using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiralGenerator : MonoBehaviour
{
    public int HalayorCount;
    public List<Transform> HalayorList;
    public float HalayMagicNum;
    public float HalayorDistance;
    public Transform HalayorPrefab;

	void Start ()
    {
	    HalayorList = new List<Transform>();
        for (int i = 0; i < HalayorCount; i++)
        {
            GameObject go = (GameObject) Instantiate(HalayorPrefab.gameObject, Vector3.zero, Quaternion.identity, transform);
            HalayorList.Add(go.transform);
        }
	}
	
	void Update ()
    {
        float t = HalayorDistance;
        float a = HalayMagicNum;

        for (int i = 0; i < HalayorCount; i++)
        {
            t = (HalayorDistance) * Mathf.Sqrt(i);
            a = HalayMagicNum;

            //The Archimedean Spiral
            HalayorList[i].position = new Vector3(a * t * Mathf.Cos(Mathf.Deg2Rad * t), a * t * Mathf.Sin(Mathf.Deg2Rad * t), 0);
        }
	}
}
