using System;
using System.Collections.Generic;
using UnityEngine;

public class Athlete : JobState
{
    public Athlete(Player player): base(player) { }

    private const float poleDistance = 25f;
    private const float speedIncRate = 0.1f;

    private bool vaultActive = false;

    private float defaultSpeed = 0f;
    private float vaultCircularSpeed = 0f;
    private float targetDistance = 0f;
    private float currentAngle = 0f;

    private Vector3 output = Vector3.zero;


    // TODO: implement me
    public override void EnterState(Dictionary<string, object> args = null)
    {
        //Debug.Log("Activated Athlete Ability");
        currentAngle = 0f;

        defaultSpeed = player.movementSpeed;
        vaultActive = false;
    }

    public override void UpdateState()
    {
        //Debug.Log("Updating Athlete Ability State");

        // Update Pole Raycast
        Quaternion rotation = Quaternion.Euler(2.5f, 0, 0);
        Vector3 direction = rotation * Vector3.forward;
        Vector3 rotatedDirection = player.gameObject.transform.rotation * direction;

        if (player.HasJumped() && !vaultActive &&
            player.movementSpeed > 2f * defaultSpeed &&
            Physics.Raycast(player.gameObject.transform.position, rotatedDirection, out RaycastHit hit) &&
            hit.distance <= poleDistance && hit.distance > poleDistance * 0.5f
        ) { 
            vaultActive = true;
            vaultCircularSpeed = player.movementSpeed * 0.125f;
            targetDistance = hit.distance;
            player.ChangeState("NoState");
        }
        else if (!vaultActive && player.IsAbilityPressed() && player.movementSpeed > 1.1f * defaultSpeed)
        {
            player.ExitJobState();
        }
        else if (!vaultActive)
        {
            Debug.DrawRay(player.gameObject.transform.position, rotatedDirection * poleDistance, Color.red);

            if (player.IsMoving())
            {
                if (player.movementSpeed <= defaultSpeed * 4f)
                    player.movementSpeed += speedIncRate;
            }
            else player.movementSpeed = defaultSpeed;
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

        if (vaultActive)
        {
            currentAngle += vaultCircularSpeed * Time.fixedDeltaTime;
            Vector3 circularMotion = new(0, 
                                        Mathf.Sin(currentAngle) * targetDistance * 2f,
                                        Mathf.Cos(currentAngle) * targetDistance);
            output = player.gameObject.transform.rotation * circularMotion;
            // output *= player.movementSpeed/defaultSpeed;
            player.UpdateMovementVector(output, true);
        }

        if (currentAngle > Math.PI * 0.5f)
        {
            player.ExitJobState();
            Debug.Log(player.gameObject.transform.position + $" {player.movementSpeed/defaultSpeed}");
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
