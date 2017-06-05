using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelCtrl : MonoBehaviour {

    public GameObject expEffect;
    private Transform tr;

    private int hitCount = 0;

    // 무작위로 선택할 텍스쳐 배열
    public Texture[] textures;

	// Use this for initialization
	void Start () {
        tr = GetComponent<Transform>();

        int idx = Random.Range(0, textures.Length);
        GetComponentInChildren<MeshRenderer>().material.mainTexture = textures[idx];
	}

    private void OnCollisionEnter(Collision coll)
    {
        if(coll.collider.tag == "BULLET")
        {
            Destroy(coll.gameObject);

            if(++hitCount >= 3)
            {
                ExpBarrel();
            }
        }
    }

    void ExpBarrel()
    {
        Instantiate(expEffect, tr.position, Quaternion.identity);

        Collider[] colls = Physics.OverlapSphere(tr.position, 10.0f);

        foreach(Collider coll in colls)
        {
            Rigidbody rbody = coll.GetComponent<Rigidbody>();
            if(rbody != null)
            {
                rbody.mass = 1.0f;
                rbody.AddExplosionForce(1000.0f, tr.position, 10.0f, 300.0f);
            }
        }

        Destroy(gameObject, 5.0f);
    }

    void OnDamage(object[] _params)
    {
        Vector3 firePos = (Vector3)_params[0];
        Vector3 hitPos = (Vector3)_params[1];
        Vector3 incomeVector = hitPos - firePos;

        incomeVector = incomeVector.normalized;

        GetComponent<Rigidbody>().AddForceAtPosition(incomeVector * 1000f, hitPos);

        if (++hitCount >= 3)
            ExpBarrel();
    }
}
