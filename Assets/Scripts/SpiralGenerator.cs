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

    public bool IsHalayRunning;
    public HalayorPool HalayorPool;
    public bool InitRandom;

    private List<Transform> _halayorList;
    private float _halayStartParamChangeRate;
    private float _halayMagicNumChangeRate;
    private float _halayorDistanceChangeRate;

    private float _startParamOffset;

    private bool _isInitialized;

    public void HalayInit ()
    {
        IsHalayRunning = false;
	    _halayorList = new List<Transform>();

        if (InitialHalayorCount <= 2)
        {
            GameObject go = HalayorPool.GetFirst();
            _halayorList.Add(go.transform);
            go.transform.parent = this.transform;
            go.transform.GetComponent<Halayor>().OnHalay = true;

            go = HalayorPool.GetLast();
            _halayorList.Add(go.transform);
            go.transform.parent = this.transform;
            go.transform.GetComponent<Halayor>().OnHalay = true;
        }
        else
        {
            for (int i = 0; i < InitialHalayorCount; i++)
            {
                GameObject go;
                if (!InitRandom)
                    go = HalayorPool.Get();
                else
                {
                    go = HalayorPool.GetRandom();
                }
                _halayorList.Add(go.transform);
                go.transform.parent = this.transform;
            }
        }

        _halayStartParamChangeRate = 0.1f;
        _halayMagicNumChangeRate = 0.00001f;
        _halayorDistanceChangeRate = 0.1f;
        _startParamOffset = 0.1f;
        Time.timeScale = 1f;

        StartCoroutine(FirstSilence(2.5f));
        _isInitialized = true;
    }

    void FixedUpdate ()
    {
        if (!_isInitialized)
            return;
        if (IsStartPointChange)
	    {
	        if (HalayStartParameter > 25f)
	        {
	            _startParamOffset = -0.1f;
	        }
            else if
	            (HalayStartParameter < 0f)
	        {
	            _startParamOffset = 0.1f;
	        }
            HalayStartParameter += _halayStartParamChangeRate * (Mathf.Sin(Time.time * 1.625f) + _startParamOffset);
	    }
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
        IsHalayRunning = true;
        IsStartPointChange = true;
        IsCircleDistanceChange = true;
        yield return new WaitForSeconds(secRun);
        IsHalayRunning = false;
        IsStartPointChange = false;
        IsCircleDistanceChange = false;
        yield return new WaitForSeconds(secWait);
        StartCoroutine(LockUnlockMovementBy(14.5f, 2f));
    }

    public void AddNewHalayorAt(int index, Transform halayorTransform)
    {
        if (index > _halayorList.Count)
        {
            _halayorList.Add(halayorTransform);
        }
        else
        {
            _halayorList.Insert(index, halayorTransform);
        }

        halayorTransform.parent = this.transform;
        halayorTransform.GetComponent<Halayor>().OnHalay = true;
    }

    public int FindHalayorIndex(Transform halayor)
    {
        if(_halayorList.Contains(halayor))
            return _halayorList.IndexOf(halayor);

        return -1;
    }

    public int GetScore()
    {
        if (_halayorList.Count == HalayorPool.HalayorSize)
        {
            int score = 0;
            for (int i = 0; i < _halayorList.Count; i++)
            {
                score += (i + 1) - Mathf.Abs(_halayorList[i].GetComponent<Halayor>().HalayorID - (i + 1));
            }
            return score;
        }
        else
            return 0;
    }
}
