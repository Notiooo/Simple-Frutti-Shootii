using UnityEngine;
using System.Collections;
using System;

public class RevolverController : MonoBehaviour {
	public float revRotSpeed;			// revolver rotation speed
	public Transform cylTr;				// ref to cylinder of revolver

	private bool rotatingRev = false;
	private bool rotatingCyl = false;

	private bool firedGun = true;
	private float endAngle = 60F;		// rotated angle

	void Start () {
		cylTr = cylTr.transform;
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
		Debug.Log ("BAM");
		firedGun = true;
	}
}
