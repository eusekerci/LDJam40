using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Halayor : MonoBehaviour
{
    public int HalayorID;
    public Color HalayorColor;
    public bool OnHalay;
    public SpiralGenerator Halay;
    private Transform _dress;
    private Animator _anim;
    private Transform _triangle;

    void Awake()
    {
        _triangle = transform.GetChild(2);
        _triangle.gameObject.SetActive(false);
        _dress = transform.GetChild(1);
        _anim = _dress.GetComponent<Animator>();
    }

	void Start ()
	{
	    GetComponent<Renderer>().material.color = HalayorColor;
	    //_halay = GameObject.Find("Halay").GetComponent<SpiralGenerator>();
	    //_dress.GetComponent<Renderer>().material.color = HalayorColor;
	}
	
	void Update ()
    {
        if (OnHalay)
        {
            if (Halay.IsHalayRunning)
            {
                _anim.SetBool("start", true);
            }
            else
            {
                _anim.SetBool("start", false);
            }
        }
        _dress.LookAt(new Vector3(0.0f,0.0f,1.5f));
    }

    public void OnActive()
    {
        if (_triangle == null)
            return;
        _triangle.gameObject.SetActive(true);
        StartCoroutine(EnableFor());
    }

    IEnumerator EnableFor()
    {
        yield return new WaitForSeconds(2f);
        _triangle.gameObject.SetActive(false);
    }
}
