using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

public class EnemyController : MonoBehaviour
{
    private DetectedPlane detectedPlane;
    private GameObject enemyInstance;

    public GameObject[] enemyModels;
    Vector3 spawnposition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (detectedPlane == null)
        {
            return;
        }

        if (detectedPlane.TrackingState != TrackingState.Tracking)
        {
            return;
        }
        // Check for the plane being subsumed
        // If the plane has been subsumed switch attachment to the subsuming plane.
        while (detectedPlane.SubsumedBy != null)
        {
            detectedPlane = detectedPlane.SubsumedBy;
        }
        if (enemyInstance == null || enemyInstance.activeSelf == false)
        {
            SpawnEnemyInstance();
            return;
        }

        Rigidbody rb = enemyInstance.GetComponent<Rigidbody>();
        rb.transform.LookAt(detectedPlane.CenterPose.position);

        rb.velocity = enemyInstance.transform.forward * 2 ;

        if(Vector3.Distance(enemyInstance.transform.position, detectedPlane.CenterPose.position) == 0)
        {
            enemyInstance.transform.position = spawnposition;
            return;
        }

    }

    public void SetSelectedPlane(DetectedPlane selectedPlane)
    {
        detectedPlane = selectedPlane;
    }

    void SpawnEnemyInstance()
    {
        GameObject foodItem = enemyModels[Random.Range(0, enemyModels.Length)];

        // Pick a location.  This is done by selecting a vertex at random and then
        // a random point between it and the center of the plane.
        List<Vector3> vertices = new List<Vector3>();
        detectedPlane.GetBoundaryPolygon(vertices);
        Vector3 pt = vertices[Random.Range(0, vertices.Count)];
        float dist = Random.Range(0.05f, 1f);
        spawnposition = Vector3.Lerp(pt, detectedPlane.CenterPose.position, dist);
        // Move the object above the plane.
        spawnposition.y += .05f;


        enemyInstance = Instantiate(foodItem, spawnposition, Quaternion.identity);

        // Set the tag.
        enemyInstance.tag = "Enemy";

        
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            Debug.Log("Enemy Hit");

            enemyInstance.transform.position = spawnposition;


        }
    }
}
