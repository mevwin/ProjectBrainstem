using System;
using System.Collections.Generic;
using UnityEngine;

public class Athlete : JobState
{
    public Athlete(Player player): base(player) { }

    private const float poleDistance = 20f;
    private const float speedIncRate = 0.5f;

    private bool vaultActive = false;

    private float defaultSpeed = 0f;
    private float vaultCircularSpeed = 0f;
    private float targetDistance = 0f;
    private float currentAngle = 0f;

    private Vector3 output = Vector3.zero;
    private Quaternion targetRotation = Quaternion.identity;
    Vector3 rotatedDirection = Vector3.zero;


    // TODO: implement me
    public override void EnterState(Dictionary<string, object> args = null)
    {
        //Debug.Log("Activated Athlete Ability");
        currentAngle = 0f;

        targetRotation = player.gameObject.transform.rotation;
        defaultSpeed = player.movementSpeed;
        vaultActive = false;
        player.ChangeState("NoState");
    }

    public override void UpdateState()
    {
        //Debug.Log("Updating Athlete Ability State");

        if (!vaultActive && player.movementSpeed >= 1.5f * defaultSpeed &&
            Physics.Raycast(player.gameObject.transform.position, rotatedDirection, out RaycastHit hit) &&
            hit.distance <= poleDistance && hit.distance > poleDistance * 0.5f
        ) { 
            vaultActive = true;
            vaultCircularSpeed = player.movementSpeed * 0.125f;
            targetDistance = hit.distance;
        }
        else if (!vaultActive && player.HasJumped() && player.movementSpeed > 1.1f * defaultSpeed)
        {
            player.ExitJobState();
        }
        else if (!vaultActive)
        {
            Debug.DrawRay(player.gameObject.transform.position, rotatedDirection * poleDistance, Color.red);
        }
    }

    /*
    - player builds up speed
    - player presses ability button again to initiate pole vault movement
    - the player maintains the velocity that they entered with it
    */
    public override void FixedUpdateState()
    {
        //Debug.Log("Fixed Updating Athlete Ability State");

        // Update Pole Raycast
        Vector3 angleOffset = Quaternion.Euler(2.5f, 0, 0) * Vector3.forward;
        rotatedDirection = targetRotation * angleOffset;

        if (vaultActive)
        {
            currentAngle += vaultCircularSpeed * Time.fixedDeltaTime;
            Vector3 circularMotion = new(0, 
                                        Mathf.Sin(currentAngle) * targetDistance * 1.4f,
                                        Mathf.Cos(currentAngle) * targetDistance);
            output = player.gameObject.transform.rotation * circularMotion;
            // output *= player.movementSpeed/defaultSpeed;
            player.UpdateMovementVector(output, true);
        }
        else
        {
            player.movementSpeed += speedIncRate;
            player.UpdateMovementVector(player.movementSpeed * (targetRotation * Vector3.forward));
        }

        if (currentAngle > Math.PI * 0.5f)
        {
            player.ExitJobState();
            // Debug.Log(player.gameObject.transform.position + $" {player.movementSpeed/defaultSpeed}");
        }
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {
        //Debug.Log("Exitted Athlete Ability");
        player.poleVaultBoost = player.movementSpeed * output.normalized;
        player.movementSpeed = defaultSpeed;
        player.poleVaultBoostDecayRate = targetDistance;
        player.ChangeState("Move");
    }
}
