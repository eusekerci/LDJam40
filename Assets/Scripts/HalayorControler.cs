using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalayorControler : MonoBehaviour
{
    public float MovementSpeed;
    public SpiralGenerator Halay;
    public Animator Anim;

    private Rigidbody _rb;
    private bool _isActive;    

	void Start ()
	{
	    _isActive = true;
	    Halay = GameObject.Find("Halay").GetComponent<SpiralGenerator>();
        _rb = GetComponent<Rigidbody>();
	    MovementSpeed = 32;
	    Anim = transform.GetChild(1).GetComponent<Animator>();
	}
	
	void Update ()
	{
	    if (!_isActive)
	        return;
	    float x = Input.GetAxis("Horizontal");
	    float y = Input.GetAxis("Vertical");

        _rb.velocity = new Vector3(Mathf.Abs(x) > 0.2f ? x : 0, Mathf.Abs(y) > 0.2f ? y : 0, 0).normalized * MovementSpeed;
        Anim.SetBool("start", true);
	}

    void OnTriggerEnter(Collider col)
    {
        if (Halay == null || !_isActive)
            return;
        if (col.gameObject.tag == "Halayor" && Halay.FindHalayorIndex(col.gameObject.transform) > -1)
        {
            _isActive = false;
            _rb.isKinematic = true;
            int ind = Halay.FindHalayorIndex(col.gameObject.transform);
            Halay.AddNewHalayorAt(ind, this.transform);
            GameManager.Instance.SelectNewHalayor();
        }
    }
}
