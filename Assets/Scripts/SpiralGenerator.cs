using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiralGenerator : MonoBehaviour
{
    public int InitialHalayorCount;
    public float HalayStartParameter;
    public float HalayMagicNum;
    public float HalayorDistance;
    public Transform HalayorPrefab;
    public Vector2 HalayOffset;

    public bool IsStartPointChange;
    public bool IsCircleDistanceChange;
    public bool IsHalayorDistanceChange;

    private List<Transform> _halayorList;
    private float _halayStartParamChangeRate;
    private float _halayMagicNumChangeRate;
    private float _halayorDistanceChangeRate;

    void Start ()
    {
	    _halayorList = new List<Transform>();
        for (int i = 0; i < InitialHalayorCount; i++)
        {
            GameObject go = (GameObject) Instantiate(HalayorPrefab.gameObject, Vector3.zero, Quaternion.identity, transform);
            _halayorList.Add(go.transform);
        }

        _halayStartParamChangeRate = 0.1f;
        _halayMagicNumChangeRate = 0.00001f;
        _halayorDistanceChangeRate = 0.1f;
        Time.timeScale = 1f;

        StartCoroutine(FirstSilence(2.5f));
    }

    void FixedUpdate ()
	{
	    if (Input.GetKeyUp(KeyCode.DownArrow))
	    {
	        _halayorList.RemoveAt(_halayorList.Count-1);
	    }
        else if (Input.GetKeyUp(KeyCode.UpArrow))
	    {
	        GameObject go = (GameObject)Instantiate(HalayorPrefab.gameObject, Vector3.zero, Quaternion.identity, transform);
	        _halayorList.Add(go.transform);
	    }

	    if(IsStartPointChange)
            HalayStartParameter += _halayStartParamChangeRate * (Mathf.Sin(Time.time*1.625f)+0.1f);
        if(IsCircleDistanceChange)
            HalayMagicNum += _halayMagicNumChangeRate * Mathf.Cos(Time.time/10f);
        if(IsHalayorDistanceChange)
	        HalayorDistance += _halayorDistanceChangeRate * Mathf.Sin(Time.time*2);

	    float t;
        float a;

        for (int i = 0; i < _halayorList.Count; i++)
        {
            t = Double.IsNaN(HalayorDistance * Mathf.Sqrt(i + HalayStartParameter)) ? 0 : HalayorDistance * Mathf.Sqrt(i + HalayStartParameter);
            a = HalayMagicNum;

            //The Archimedean Spiral
            _halayorList[i].position = new Vector3(a * t * Mathf.Cos(Mathf.Deg2Rad * t), a * t * Mathf.Sin(Mathf.Deg2Rad * t), 0) + (Vector3)HalayOffset;
        }
	}

    IEnumerator FirstSilence(float sec)
    {
        IsStartPointChange = false;
        IsCircleDistanceChange = false;
        yield return new WaitForSeconds(sec);
        StartCoroutine(LockUnlockMovementBy(14.5f, 2f));
    }
    IEnumerator LockUnlockMovementBy(float secRun, float secWait)
    {
        IsStartPointChange = true;
        IsCircleDistanceChange = true;
        yield return new WaitForSeconds(secRun);
        IsStartPointChange = false;
        IsCircleDistanceChange = false;
        yield return new WaitForSeconds(secWait);
        StartCoroutine(LockUnlockMovementBy(14.5f, 2f));
    }
}
