using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

public class PlayerController : MonoBehaviour
{
    private DetectedPlane detectedPlane;

    public GameObject playerPrefab;
    private GameObject playerInstance;
    public GameObject pointer;
    public Camera firstPersonCamera;
    // Speed to move.
    public float speed = 20f;
    //forshooting
    public GameObject bulletPrefab;
    public Transform SpawnPoint;
    float nextFire;
    public float fireRate;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInstance == null || playerInstance.activeSelf == false)
        {
            pointer.SetActive(false);
            return;
        }
        else
        {
            pointer.SetActive(true);
        }
        TrackableHit hit;
        TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinBounds;

        if (Frame.Raycast(Screen.width / 2, Screen.height / 2, raycastFilter, out hit))
        {
            Vector3 pt = hit.Pose.position;
            //Set the Y to the Y of the snakeInstance
            pt.y = playerInstance.transform.position.y;
            // Set the y position relative to the plane and attach the pointer to the plane
            Vector3 pos = pointer.transform.position;
            pos.y = pt.y;
            pointer.transform.position = pos;

            // Now lerp to the position                                         
            pointer.transform.position = Vector3.Lerp(pointer.transform.position, pt,
              Time.smoothDeltaTime * speed);
        }

        // Move towards the pointer, slow down if very close.                                                                                     
        float dist = Vector3.Distance(pointer.transform.position,
            playerInstance.transform.position) - 0.1f;
        if (dist < 0)
        {
            dist = 0;
        }

        Rigidbody rb = playerInstance.GetComponent<Rigidbody>();
        rb.transform.LookAt(pointer.transform.position);
        
        rb.velocity = playerInstance.transform.localScale.x *
            playerInstance.transform.forward * dist / .01f;
        SpawnPoint = playerInstance.transform.GetChild(1);
        timer = Time.time;
    }

    public void SetPlane(DetectedPlane plane)
    {
        detectedPlane = plane;
        // Spawn a new snake.
        SpawnPlayer();
        
    }

    void SpawnPlayer()
    {
        if (playerInstance != null)
        {
            DestroyImmediate(playerInstance);
        }

        Vector3 pos = detectedPlane.CenterPose.position;

        // Not anchored, it is rigidbody that is influenced by the physics engine.
        playerInstance = Instantiate(playerPrefab, pos,
                Quaternion.identity, transform);
        

    }

    public void ShootButton() {
        if (timer > nextFire)
        {
            nextFire =timer + fireRate;
           
            Instantiate(bulletPrefab, pointer.transform.position, SpawnPoint.transform.rotation);
           
        }
    }
    
}
