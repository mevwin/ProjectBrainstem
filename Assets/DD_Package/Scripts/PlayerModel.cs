using System.Collections;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    public GameObject player;
    public Animator BodyAnimator;

    public float idleTimer;
    public bool spin_idle_switch = true;

    public Vector3 lastPos;
    public Vector3 delta;

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        lastPos = player.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    { 
        delta = player.transform.position - lastPos;

        // follow position
        transform.position = player.transform.position;

        Debug.DrawRay(player.transform.position, delta.normalized * 1f, Color.green, 2f);

        Vector3 direction = transform.forward + Vector3.RotateTowards(transform.forward, delta.normalized, 10 * Time.deltaTime, 0f);

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
            switch (spin_idle_switch)
            {
                case true:
                    BodyAnimator.SetBool("goIdle", true);
                    break;
                case false:
                    BodyAnimator.SetBool("goSpin", true);
                    break;


            }

            spin_idle_switch = !spin_idle_switch;
            


            idleTimer = 0f;

            StartCoroutine(ResetState());

        }



    }

    IEnumerator ResetState()
    {
        yield return new WaitForSeconds(4.1f);
        BodyAnimator.SetBool("goIdle", false);
        BodyAnimator.SetBool("goSpin", false);
        idleTimer = 0f;


    }


}
