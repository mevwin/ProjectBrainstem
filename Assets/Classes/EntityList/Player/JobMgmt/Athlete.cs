using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Athlete : JobState
{
    public Athlete(Player player): base(player) { }

    private const float speedIncRate = 50f;
    private const float poleMaxDistance = 20f;

    private bool vaultActive = false;
    private bool canVault = false;

    // Runtime Variables
    private float defaultSpeed = 0f;
    private float vaultCircularSpeed = 0f;
    private float currentAngle = 0f;
    private float targetDistance = 0f;
    private Quaternion targetRotation = Quaternion.identity;
    private Vector3 targetPosition = Vector3.zero;
    private Vector3 output = Vector3.zero;
    private RaycastHit[] surfaces;
    private PoleVaultSpot targetedVaultSpot = null;


    public override void EnterState(Dictionary<string, object> args = null)
    {
        surfaces = player.ZoomDetection(poleMaxDistance);

        canVault = player.IsGrounded() && AthleteCheckCollisionsForVaultSpot();

        if (!canVault)
        {
            player.ExitJobState();
            return;
        }

        currentAngle = 0f;

        defaultSpeed = player.movementSpeed;
        vaultActive = false;

        targetRotation = Quaternion.LookRotation(targetPosition - player.transform.position);

        player.ChangeState("NoState");
    }

    public override void UpdateState()
    {
        if (!canVault) return;

        if (!vaultActive)
        {
            if (player.initiatePullJump)
            {
                vaultActive = true;
                vaultCircularSpeed = player.movementSpeed * 0.125f;
            }
            else if (player.HasJumped() && player.movementSpeed > 1.1f * defaultSpeed)
            {
                player.ExitJobState();
            }
        }
    }

    /*
    - player builds up speed
    - player presses ability button again to initiate pole vault movement
    - the player maintains the velocity that they entered with it
    */
    public override void FixedUpdateState()
    {
        if (!canVault) return;

        if (vaultActive)
        {
            currentAngle += vaultCircularSpeed * Time.fixedDeltaTime;
            Vector3 circularMotion = new(0, 
                                        Mathf.Sin(currentAngle) * targetDistance * 2f,
                                        Mathf.Cos(currentAngle) * targetDistance);
            output = targetRotation * circularMotion;
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
        if (!canVault) return;

        //Debug.Log("Exitted Athlete Ability");
        player.poleVaultBoost = player.movementSpeed * output.normalized;
        player.movementSpeed = defaultSpeed;
        player.spot = null;
        player.initiatePullJump = false;
        player.ChangeState("Move");
    }

    public bool IsSurfacePoleVaultSpot(RaycastHit hit)
    {
        if (hit.collider.gameObject.TryGetComponent(out targetedVaultSpot) &&
            player.transform.position.y > targetedVaultSpot.transform.position.y - 2f &&
            player.transform.position.y < targetedVaultSpot.transform.position.y + 2f)
        {
            targetDistance = hit.distance;
            targetPosition = targetedVaultSpot.transform.position;
            player.spot = targetedVaultSpot;
            return true;
        }
        return false;
    }

    public bool AthleteCheckCollisionsForVaultSpot()
    {
        foreach (RaycastHit surface in surfaces)
        {
            if (IsSurfacePoleVaultSpot(surface))
                return true;    
        }
        return false;
    }

    public void ProjectVaultStrength()
    {
        surfaces = player.ZoomDetection(poleMaxDistance);

        foreach (RaycastHit surface in surfaces)
        {
            if (IsSurfacePoleVaultSpot(surface))
            {
                player.athleteLineRenderer.gameObject.SetActive(true);

                Color lineColor = GetVaultStrengthColor();

                player.athleteLineRenderer.UpdateLine(surface.collider.gameObject.transform.position, lineColor);
                break;
            }
            else player.athleteLineRenderer.gameObject.SetActive(false);
        }
    }

    Color GetVaultStrengthColor()
    {
        float percentage = Vector3.Distance(player.transform.position, targetPosition - new Vector3(0, 0.5f, 0f)) / poleMaxDistance;

        Color output;
        if (percentage > 0.70f)
            output = Color.red;
        else if (percentage > 0.35f)
            output = Color.yellow;
        else
            output = Color.green;

        return output;
    }
}
