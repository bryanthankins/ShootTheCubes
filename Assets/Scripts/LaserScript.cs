using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LaserScript : MonoBehaviour
{
    public GameObject explosionPrefab;
	public AudioClip shootSound;
    public AudioClip laserSound;
    public Text scoreText;
    private int scoreCount = 0;
	private AudioSource source;

    public float mFireRate = .5f;
    public float mFireRange = 50f;
    public float mHitForce = 100f;
    public int mLaserDamage = 100;

    // Line render that will represent the Laser
    private LineRenderer mLaserLine;

    // Define if laser line is showing
    private bool mLaserLineEnabled;

    // Time that the Laser lines shows on screen
    private WaitForSeconds mLaserDuration = new WaitForSeconds(0.05f);

    // time of the until the next fire
    private float mNextFire;
void Update () {
    if ( Input.GetButton("Fire1") && Time.time > mNextFire ){
        Fire();
        }    
    }

// Show the Laser Effects
private IEnumerator LaserFx(){
    mLaserLine.enabled = true;
 
    // Way for a specific time to remove the LineRenderer
    yield return mLaserDuration;
    mLaserLine.enabled = false;
}

    // Use this for initialization
    void Start()
    {
        // getting the Line Renderer
        mLaserLine = GetComponent<LineRenderer>();
    }

    // Shot the Laser
    // Shot the Laser
    private void Fire()
    {
        // Get ARCamera Transform
        Transform cam = Camera.main.transform;

        // Define the time of the next fire
        mNextFire = Time.time + mFireRate;

        // Set the origin of the RayCast
        Vector3 rayOrigin = cam.position;

        // Set the origin position of the Laser Line
        // It will always 10 units down from the ARCamera
        // We adopted this logic for simplicity
        mLaserLine.SetPosition(0, transform.up * -10f);
        // Hold the Hit information
        RaycastHit hit;

        // Checks if the RayCast hit something
        if (Physics.Raycast(rayOrigin, cam.forward, out hit, mFireRange))
        {
            scoreCount++;
            scoreText.text = "SCORE: " + scoreCount;

            // Set the end of the Laser Line to the object hit
            mLaserLine.SetPosition(1, hit.point);
			// Get the CubeBehavior script to apply damage to target
            CubeBehaviorScript cubeCtr = hit.collider.GetComponent<CubeBehaviorScript>();
            if ( cubeCtr != null ) {
                if ( hit.rigidbody != null ) {
                    // apply force to the target
                    hit.rigidbody.AddForce(-hit.normal*mHitForce);
                    // apply damage the target
                    cubeCtr.Hit( mLaserDamage );
                    source = GetComponent<AudioSource>();
                    source.PlayOneShot(shootSound, 1F);
                    Instantiate(explosionPrefab, hit.point, Quaternion.identity);
                }
            }

        }
        else
        {
            // Set the enfo of the laser line to be forward the camera
            // using the Laser range
            mLaserLine.SetPosition(1, cam.forward * mFireRange);
        }

           source = GetComponent<AudioSource>();
           source.PlayOneShot(laserSound, 1F);
// Show the Laser using a Coroutine
    StartCoroutine(LaserFx());
		
    }
}
