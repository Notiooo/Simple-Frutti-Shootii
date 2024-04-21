using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[AddComponentMenu("Nokobot/Modern Guns/Simple Shoot")]
public class SimpleShoot : MonoBehaviour
{
    [Header("Prefab Refrences")]
    public GameObject bulletPrefab;
    public GameObject casingPrefab;
    public GameObject muzzleFlashPrefab;
    public AudioSource source;
    public AudioClip fireSound;
    public AudioClip reloadSound;
    public AudioClip noAmmoSound;
    public Magazine magazine;
    public XRBaseInteractor socketInteractor;
    private bool hasSlide = true;
	public float gunDamage = 10;
	public float gunHitForceMagnitude = 4;
	public float gunHitRange = 1000;
    private LineRenderer lineRenderer;

    [Header("Location Refrences")]
    [SerializeField] private Animator gunAnimator;
    [SerializeField] private Transform barrelLocation;
    [SerializeField] private Transform casingExitLocation;

    [Header("Settings")]
    [Tooltip("Specify time to destory the casing object")] [SerializeField] private float destroyTimer = 2f;
    [Tooltip("Bullet Speed")] [SerializeField] private float shotPower = 500f;
    [Tooltip("Casing Ejection Speed")] [SerializeField] private float ejectPower = 150f;


    public void AddMagazine(XRBaseInteractable interactable)
    {
        magazine = interactable.GetComponent<Magazine>();
        source.PlayOneShot(reloadSound);
        hasSlide = false;
    }

    public void RemoveMagazine(XRBaseInteractable interactable)
    {
        magazine = null;
        source.PlayOneShot(reloadSound);
    }

    public void Slide()
    {
        if(!hasSlide)
        {
            source.PlayOneShot(reloadSound);
        }
        hasSlide = true;
    }

    void Start()
    {
        if (barrelLocation == null)
            barrelLocation = transform;

        if (gunAnimator == null)
            gunAnimator = GetComponentInChildren<Animator>();

        lineRenderer = this.GetComponent<LineRenderer>();

        socketInteractor.onSelectEntered.AddListener(AddMagazine);
        socketInteractor.onSelectExited.AddListener(RemoveMagazine);
    }

    public void PullTheTrigger()
    {
        if(magazine && magazine.numberOfBullets > 0 && hasSlide)
        {
            gunAnimator.SetTrigger("Fire");
        }
        else
        {
            source.PlayOneShot(noAmmoSound);
        }
    }


    //This function creates the bullet behavior
    void Shoot()
    {
        magazine.numberOfBullets--;
        source.PlayOneShot(fireSound);
        if (muzzleFlashPrefab)
        {
            //Create the muzzle flash
            GameObject tempFlash;
            tempFlash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation);

            //Destroy the muzzle flash effect
            Destroy(tempFlash, destroyTimer);
        }

        RaycastHit rayHit;
        if(Physics.Raycast(barrelLocation.position, barrelLocation.forward, out rayHit, gunHitRange))
        {
            Shootable fool = rayHit.collider.GetComponentInParent<Shootable> ();
            if (fool != null) 
            {
				Vector3 hitForce = barrelLocation.forward * gunHitForceMagnitude;
				fool.Hit(gunDamage, hitForce, rayHit.point);
            }
            // Debug line
			StartCoroutine (DrawDebugLine(barrelLocation.position, rayHit.point));
        }
        else
        {
            // Debug line
            StartCoroutine (DrawDebugLine(barrelLocation.position, barrelLocation.position + barrelLocation.forward * gunHitRange));
        }

        //cancels if there's no bullet prefeb
        if (!bulletPrefab)
        { return; }

        // Create a bullet and add force on it in direction of the barrel
        Instantiate(bulletPrefab, barrelLocation.position, barrelLocation.rotation).GetComponent<Rigidbody>().AddForce(barrelLocation.forward * shotPower);
    }

    //This function creates a casing at the ejection slot
    void CasingRelease()
    {
        //Cancels function if ejection slot hasn't been set or there's no casing
        if (!casingExitLocation || !casingPrefab)
        { return; }

        //Create the casing
        GameObject tempCasing;
        tempCasing = Instantiate(casingPrefab, casingExitLocation.position, casingExitLocation.rotation) as GameObject;
        //Add force on casing to push it out
        tempCasing.GetComponent<Rigidbody>().AddExplosionForce(Random.Range(ejectPower * 0.7f, ejectPower), (casingExitLocation.position - casingExitLocation.right * 0.3f - casingExitLocation.up * 0.6f), 1f);
        //Add torque to make casing spin in random direction
        tempCasing.GetComponent<Rigidbody>().AddTorque(new Vector3(0, Random.Range(100f, 500f), Random.Range(100f, 1000f)), ForceMode.Impulse);

        //Destroy casing after X seconds
        Destroy(tempCasing, destroyTimer);
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
