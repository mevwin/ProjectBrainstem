using System.Collections;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    [SerializeField] private Player playerScript;
    [SerializeField] private GameObject model;
    [SerializeField] private GameObject ParticleParent;
    [SerializeField] private Animator BodyAnimator;
    
    [Header("==Job Prefabs==")]
    [SerializeField] private GameObject[] artistPrefabs;
    [SerializeField] private GameObject[] athletePrefabs;
    [SerializeField] private GameObject[] builderPrefabs;
    [SerializeField] private GameObject[] musicianPrefabs;

    [Header("==Inspect Fields==")]
    [SerializeField] private float turnSpeed = 10f;
    [SerializeField] private float idleTimer;
    [SerializeField] private bool spin_idle_switch = true;
    [SerializeField] private Vector3 lastPos;
    [SerializeField] private Vector3 delta;
    [SerializeField] private Vector3 zoomHeldForward = Vector3.zero;


    private void FixedUpdate()
    {
        lastPos = model.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    { 
        if (Time.deltaTime == 0f) return;

        delta = model.transform.position - lastPos;

        // follow position
        transform.position = model.transform.position;

        if (playerScript.IsZoomHeld())
        {
            zoomHeldForward = Vector3.ProjectOnPlane(playerScript.cam.transform.forward, Vector3.up);
            transform.forward = zoomHeldForward;
        }
        else
        {
            Vector3 direction = transform.forward + Vector3.RotateTowards(transform.forward, delta.normalized, turnSpeed * Time.deltaTime, 0f);

            direction = Vector3.ProjectOnPlane(direction, Vector3.up);
            transform.forward = direction;

            if (delta.magnitude < .1f)
            {
                idleTimer++;
            }
            else
            {
                idleTimer = 0f;
            }

            if (idleTimer > 700f)
            {
                if (spin_idle_switch)
                    BodyAnimator.SetBool("goIdle", true);
                else
                    BodyAnimator.SetBool("goSpin", true);

                spin_idle_switch = !spin_idle_switch;
                idleTimer = 0f;
                StartCoroutine(ResetState());
            }
        }
    }

    IEnumerator ResetState()
    {
        yield return new WaitForSeconds(4.1f);
        BodyAnimator.SetBool("goIdle", false);
        BodyAnimator.SetBool("goSpin", false);
        idleTimer = 0f;
    }

    public void JobKitSwitch()
    {
        foreach (Transform child in ParticleParent.transform) {
            child.gameObject.SetActive(false);
            child.gameObject.SetActive(true);
            child.GetComponent<ParticleSystem>().Play();
        }
    }

    public void JobKitToggle(JobManager.Job job, bool toggle)
    {
        GameObject[] kitToReset = null;
    
        switch(job)
        {
            case JobManager.Job.ARTIST:
                kitToReset = artistPrefabs;
                break;
            
            case JobManager.Job.ATHLETE:
                kitToReset = athletePrefabs;
                break;

            case JobManager.Job.BUILDER:
                kitToReset = builderPrefabs;
                break;

            case JobManager.Job.MUSICIAN:
                kitToReset = musicianPrefabs;
                break;
        }

        if (kitToReset == null) return;

        foreach (GameObject go in kitToReset)
        {
            go.SetActive(toggle);
        }
    }
}
