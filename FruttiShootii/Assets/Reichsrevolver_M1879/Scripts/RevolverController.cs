using UnityEngine;
using System.Collections;

public class RevolverController : MonoBehaviour {
	public float revRotSpeed;			// revolver rotation speed
	public Transform cylTr;				// ref to cylinder of revolver

	public Transform barrel;

	private bool rotatingRev = false;
	private bool rotatingCyl = false;

	private bool firedGun = true;
	private float endAngle = 60F;		// rotated angle

	public float gunDamage = 10;
	public float gunHitForceMagnitude = 4;
	public float gunHitRange = 1000;

	private LineRenderer lineRenderer;
	private AudioSource audioSource;
	public AudioClip[] gunShotClips;

	void Start () {
		cylTr = cylTr.transform;
		lineRenderer = this.GetComponent<LineRenderer>();
		audioSource = this.GetComponentInChildren<AudioSource>();
	}

	void Update () {
		if (rotatingRev)
			transform.Rotate (Vector3.up * revRotSpeed * Time.deltaTime);

		if (rotatingCyl) {
			RotateCyl ();
			if(!firedGun){
				FireGun();
			}
		}
	}

	public void FlipRevState () {
		rotatingRev = !rotatingRev;
	}

	public void RotateCyl () {
		if (endAngle == 360F && cylTr.localRotation.eulerAngles.y < 60F) {
			endAngle = 0F;
		}
		if (cylTr.localRotation.eulerAngles.y < endAngle ) {
			rotatingCyl = true;
			Quaternion target = Quaternion.Euler (0, endAngle, 0); 
			cylTr.localRotation = Quaternion.RotateTowards (cylTr.localRotation, target, Time.deltaTime * 400F);
		} else {
			rotatingCyl = false;
			firedGun = false;
			endAngle += 60F;
		}
	}

	public void FireGun() {
		firedGun = true;

		audioSource.clip = gunShotClips[Random.Range(0, gunShotClips.Length)];
		audioSource.Play();

		RaycastHit rayHit;
		if(Physics.Raycast(barrel.transform.position, barrel.transform.forward, out rayHit, gunHitRange)) {
			Shootable fool = rayHit.collider.GetComponentInParent<Shootable> ();
			if (fool != null) {
				Vector3 hitForce = barrel.transform.forward * gunHitForceMagnitude;
				fool.Hit(gunDamage, hitForce, rayHit.point);
			}
			// Debug line
			StartCoroutine (DrawDebugLine(barrel.transform.position, rayHit.point));
		}
		else {
			// Debug line
			StartCoroutine (DrawDebugLine(barrel.transform.position, barrel.transform.position + barrel.transform.forward * gunHitRange));
		}
	}

	private IEnumerator DrawDebugLine(Vector3 start, Vector3 end)
    {
        lineRenderer.SetPosition(0, start);
		lineRenderer.SetPosition(1, end);
		lineRenderer.enabled = true;

        yield return new WaitForSeconds(0.5f);

        lineRenderer.enabled = false;
    }
}
