using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGo : MonoBehaviour {
    public float speed=10;
    Rigidbody rb;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        
        rb.velocity = transform.forward * speed;
		StartCoroutine (DestroyBullet());
    }

	IEnumerator DestroyBullet()
	{
		yield return new WaitForSeconds (5f);
		Destroy (gameObject);
	}
	
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy") {
            Debug.Log("Bullet Hit");
            Destroy(gameObject);

        }

    }
		
    // Update is called once per frame
    void Update () {
		
	}
}
