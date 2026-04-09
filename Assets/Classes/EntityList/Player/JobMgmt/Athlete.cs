using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Athlete : JobState
{
    public Athlete(Player player): base(player) { }

    private const float speedIncRate = 50f;

    private bool vaultActive = false;

    private float defaultSpeed = 0f;
    private float vaultCircularSpeed = 0f;
    private float currentAngle = 0f;
    private float targetDistance = 0f;
    private Vector3 targetPosition = Vector3.zero;
    private Quaternion targetRotation = Quaternion.identity;

    private Vector3 output = Vector3.zero;


    // TODO: implement me
    public override void EnterState(Dictionary<string, object> args = null)
    {
        //Debug.Log("Activated Athlete Ability");
        currentAngle = 0f;

        defaultSpeed = player.movementSpeed;
        vaultActive = false;

        if (args != null)
        {
            if (args.ContainsKey("poleDistance"))
                targetDistance = (float) args["poleDistance"];
            if (args.ContainsKey("polePosition"))
                targetPosition = (Vector3) args["polePosition"];
        }

        targetRotation = Quaternion.LookRotation(targetPosition - player.transform.position);

        player.ChangeState("NoState");
    }

    public override void UpdateState()
    {
        //Debug.Log("Updating Athlete Ability State");

        if (!vaultActive && player.initiatePullJump)
        {
            vaultActive = true;
            vaultCircularSpeed = player.movementSpeed * 0.125f;
        }

        if (vaultActive && player.HasJumped() && player.movementSpeed > 1.1f * defaultSpeed)
            player.ExitJobState();
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
            output = player.transform.rotation * circularMotion;
            player.UpdateMovementVector(output, true);
        }
        else
        {
            player.movementSpeed = Mathf.MoveTowards(player.movementSpeed, defaultSpeed * 3f, Time.fixedDeltaTime * speedIncRate);
            player.UpdateMovementVector(player.movementSpeed * (targetRotation * Vector3.forward));
        }

        if (currentAngle > Math.PI * 0.5f)
        {
            // Debug.Log($"height: {player.gameObject.transform.position.y}, speed: {player.movementSpeed / defaultSpeed * 100f}%, distance: {targetDistance}");
            player.ExitJobState();
        }
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {
        //Debug.Log("Exitted Athlete Ability");
        player.poleVaultBoost = player.movementSpeed * output.normalized;
        player.movementSpeed = defaultSpeed;
        player.poleVaultBoostDecayRate = targetDistance;
        player.spot = null;
        player.initiatePullJump = false;
        player.ChangeState("Move");
    }
}
