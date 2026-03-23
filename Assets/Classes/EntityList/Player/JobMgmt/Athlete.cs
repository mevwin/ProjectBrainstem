using System.Collections.Generic;
using UnityEngine;

public class Athlete : JobState
{
    public Athlete(Player player): base(player) { }

    private readonly float poleDistance = 25f;
    private bool vaultActive = false;

    private float targetDistance = 0f;
    private float rotationSpeed = 0f;
    private float currentAngle = 0f;

    private Vector3 output = Vector3.zero;


    // TODO: implement me
    public override void EnterState(Dictionary<string, object> args = null)
    {
        //Debug.Log("Activated Athlete Ability");
        currentAngle = 0f;

        player.movementSpeed *= 2.0f;
        vaultActive = false;
    }

    public override void UpdateState()
    {
        //Debug.Log("Updating Athlete Ability State");

        // Update Pole Raycast
        Quaternion rotation = Quaternion.Euler(5f, 0, 0);
        Vector3 direction = rotation * Vector3.forward;
        Vector3 rotatedDirection = player.gameObject.transform.rotation * direction;

        if (player.HasJumped() && !vaultActive &&
            Physics.Raycast(player.gameObject.transform.position, rotatedDirection, out RaycastHit hit) &&
            player.GetRigidbodyVelocity().magnitude > 2f
        ) {
            vaultActive = true;
            rotationSpeed = player.GetRigidbodyVelocity().magnitude * 0.2f;
            targetDistance = hit.distance * 5f;
            player.ChangeState("NoState");
        }
        else Debug.DrawRay(player.gameObject.transform.position, rotatedDirection * poleDistance, Color.red);
    }

    /*
    - player builds up speed
    - player presses ability button again to initiate pole vault movement
    - the player maintains the velocity that they entered with it
    */
    public override void FixedUpdateState()
    {
        //Debug.Log("Fixed Updating Athlete Ability State");

        if (vaultActive)
        {
            currentAngle += rotationSpeed * Time.fixedDeltaTime;
            Vector3 circularMotion = new(0, Mathf.Cos(currentAngle) * targetDistance, Mathf.Sin(currentAngle) * targetDistance);
            output = player.gameObject.transform.rotation * circularMotion;
            player.UpdateMovementVector(output, true);
        }

        if (currentAngle > 0.85f)
            player.ExitJobState();
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {
        //Debug.Log("Exitted Athlete Ability");

        player.movementSpeed *= 0.5f;
        player.poleVaultBoost = player.GetRigidbodyVelocity().magnitude * output.normalized;
        player.poleVaultBoostDecayRate = targetDistance * 1.1f;
        player.ChangeState("Move");
    }
}
