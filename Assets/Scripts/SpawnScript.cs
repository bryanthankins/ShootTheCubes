using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class SpawnScript : MonoBehaviour {
	public GameObject mCubeObj;

	public int mTotalCubes = 10;
	public float mTimeToSpawn = 1f;

	private GameObject[] mCubes;

	private bool mPositionSet;
	private bool SetPosition(){
		Transform cam = Camera.main.transform;
		transform.position = cam.forward * 10;
		return true;
	}

	// Use this for initialization
	void Start () {
	   // Initializing spawning loop
        StartCoroutine( SpawnLoop() );
 
        // Initialize Cubes array according to
        // the desired quantity
        mCubes = new GameObject[ mTotalCubes ];
		
	}

	private IEnumerator SpawnLoop(){

		StartCoroutine( ChangePosition() );
		yield  return new WaitForSeconds(0.2f);

		int i = 0;
		while( i<= (mTotalCubes-1)) {
			mCubes[i] = SpawnElement();
			i++;
			yield return new WaitForSeconds(Random.Range(mTimeToSpawn, mTimeToSpawn *3));
		}
	}

	// Spawn a cube
    private GameObject SpawnElement() 
    {
        // spawn the element on a random position, inside a imaginary sphere
        GameObject cube = Instantiate(mCubeObj, (Random.insideUnitSphere*4) + transform.position, transform.rotation ) as GameObject;
        // define a random scale for the cube
        float scale = Random.Range(0.5f, 2f);
        // change the cube scale
        cube.transform.localScale = new Vector3( scale, scale, scale );
        return cube;
    }

	private IEnumerator ChangePosition() {
		yield return new WaitForSeconds(0.2f);
		if(!mPositionSet){
			if(VuforiaBehaviour.Instance.enabled)
				SetPosition();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
