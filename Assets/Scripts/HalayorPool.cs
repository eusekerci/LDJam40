using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;

public class HalayorPool : MonoBehaviour
{
    private List<GameObject> _halayorPool;
    private Transform _halayorParent;

    public int HalayorSize;
    public GameObject HalayorPrefab;
    public Color LeftMostColor;
    public Color RightMostColor;
    public Transform Halay;
    public bool OnGame;

    void Awake ()
	{
        _halayorPool = new List<GameObject>();
        _halayorParent = this.transform;
    }

    void Start()
    {
        for (int i = 0; i < HalayorSize; i++)
        {
            GameObject go = Object.Instantiate(HalayorPrefab, _halayorParent);
            Halayor halayor = go.GetComponent<Halayor>();
            halayor.Halay = Halay.GetComponent<SpiralGenerator>();
            go.transform.position = new Vector3(Random.Range(0f, 1f) >= 0.5 ? Random.Range(53.0f, 77.0f) : Random.Range(-53.0f, -77.0f), Random.Range(-40.0f, 40.0f), 0);
            halayor.HalayorColor = GameManager.LerpHSV(ColorExtension.ToHSV(LeftMostColor),
                ColorExtension.ToHSV(RightMostColor), ((float)i) / HalayorSize);
            halayor.HalayorID = i+1;
            go.transform.GetChild(0).GetComponent<TextMesh>().text = (i + 1).ToString();
            _halayorPool.Add(go);
        }

        Halay.GetComponent<SpiralGenerator>().HalayInit();
        if(OnGame)
            GameManager.Instance.SelectNewHalayor();

    }

    public GameObject Get(int i = 0)
    {
        if (_halayorPool.Count <= 0)
        {
            Debug.LogError("Pool size is exceeded");
            return null;
        }

        GameObject go = _halayorPool[i];
        _halayorPool.RemoveAt(i);
        go.SetActive(true);

        return go;
    }

    public GameObject GetFirst()
    {
        return Get(0);
    }

    public GameObject GetLast()
    {
        return Get(_halayorPool.Count - 1);
    }

    public GameObject GetRandom()
    {
        return Get(Random.Range(0, _halayorPool.Count));
    }

    public int GetRemaining()
    {
        return _halayorPool.Count;
    }

    public void Kill(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(_halayorParent);
        _halayorPool.Add(obj);
    }
}