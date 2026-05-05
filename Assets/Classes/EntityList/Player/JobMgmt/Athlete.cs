using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Athlete : JobState
{
    public enum Mode
    {
        SPOT_SPAWN,
        VAULT_JUMP
    }

    public Athlete(Player player): base(player) { }

    private const float SPEED_INC_RATE = 50f;
    private const float POLE_MAX_DISTANCE = 20f;
    private const float UNGROUNDED_TIME_MAX = 0.35f;
    private const float SPOT_SPAWN_MAX_DISTANCE = 10f;

    private bool vaultActive = false;
    private bool canVault = false;

    public Mode currentMode = Mode.SPOT_SPAWN;

    // Runtime Variables
    private Quaternion targetRotation = Quaternion.identity;
    private Vector3 targetPosition = Vector3.zero;

    private float defaultSpeed = 0f;
    private float vaultCircularSpeed = 0f;
    private float currentAngle = 0f;
    private float targetDistance = 0f;
    private float ungroundedTimer = 0f;
    private Vector3 output = Vector3.zero;
    private RaycastHit[] surfaces;
    
    private GameObject currentSpot;
    private RaycastHit hit;
    

    public override void EnterState(Dictionary<string, object> args = null)
    {
        canVault = false;

        if (player.itemPresent)
        {
            player.itemPresent.Throw(player);
            player.ExitJobState();
            return;
        }
        
        switch (currentMode)
        {
            case Mode.SPOT_SPAWN:
                if (hit.collider != null)
                {
                    if (CheckForSpotPosition(hit))
                    {
                        if (currentSpot == null)
                            SpawnVaultSpot();
                        else
                            RepositionSpot();
                    }
                    else if (hit.collider.gameObject == currentSpot)
                        DespawnSpot();
                }
                player.ExitJobState();

                break;

            case Mode.VAULT_JUMP:
                surfaces = player.ZoomDetection(POLE_MAX_DISTANCE);

                canVault = player.IsGrounded() && CheckCollisionsForVaultSpot();

                if (!canVault)
                {
                    player.ExitJobState();
                    return;
                }
                player.athleteLineRenderer.gameObject.SetActive(false);

                currentAngle = 0f;
                ungroundedTimer = 0f;

                defaultSpeed = player.movementSpeed;
                vaultActive = false;

                targetRotation = Quaternion.LookRotation(targetPosition - player.transform.position);

                player.ChangeState("NoState");

                break;
        }
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

        if (!player.IsGrounded() && !vaultActive)
            ungroundedTimer += Time.fixedDeltaTime;

        if (player.IsGrounded() && ungroundedTimer > 0f)
            ungroundedTimer = 0f;

        if (ungroundedTimer > UNGROUNDED_TIME_MAX)
        {
            player.ExitJobState();
            return;
        }

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
            player.movementSpeed = Mathf.MoveTowards(player.movementSpeed, defaultSpeed * 3f, Time.fixedDeltaTime * SPEED_INC_RATE);
            player.UpdateMovementVector(player.movementSpeed * (targetRotation * Vector3.forward));
        }

        if (currentAngle > math.PI * 0.5f)
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
        player.poleVaultBoostDecayRate = targetDistance;
        player.movementSpeed = defaultSpeed;
        player.initiatePullJump = false;
        player.ChangeState("Move");
    }

    public bool IsSurfacePoleVaultSpot(RaycastHit hit)
    {
        GameObject targetGO = hit.collider.gameObject;

        if (!targetGO.TryGetComponent<PoleVaultSpot>(out _))
            return false;
        
        if (player.transform.position.y > targetGO.transform.position.y - 2f &&
            player.transform.position.y < targetGO.transform.position.y + 2f
        ) {
            targetDistance = hit.distance;
            targetPosition = targetGO.transform.position;
            player.targetVaultSpot = targetGO;
            return true;
        }
        return false;
    }

    bool CheckCollisionsForVaultSpot()
    {
        foreach (RaycastHit surface in surfaces)
        {
            if (IsSurfacePoleVaultSpot(surface))
                return true;
        }
        return false;
    }

    bool CheckForSpotPosition(RaycastHit hit)
    {
        GameObject obj = hit.collider.gameObject;

        float dot = Vector3.Dot(hit.normal, Vector3.up);
        // Debug.Log($"dot: {dot}");

        return obj.layer == 0 && dot > 0.99f;
    }

    void SpawnVaultSpot()
    {
        currentSpot = Object.Instantiate(player.vaultSpotPrefab, targetPosition, Quaternion.identity);
    }

    void DespawnSpot()
    {
        Object.Destroy(currentSpot);
    }

    void RepositionSpot()
    {
        currentSpot.transform.position = targetPosition;
    }

    public void ChangeMode()
    {
        currentMode = (Mode) (((int) currentMode + 1) % 2);

        if (currentMode != Mode.SPOT_SPAWN)
            Object.Destroy(player.CurrentProjectedSpot);
        else if (currentMode != Mode.VAULT_JUMP)
            player.athleteLineRenderer.gameObject.SetActive(false);
    }

    public void PrepAbility()
    {
        switch(currentMode)
        {
            case Mode.SPOT_SPAWN:
                ProjectVaultSpot();
                break;
            case Mode.VAULT_JUMP:
                ProjectVaultStrength();
                break;
        }   
    }

    void ProjectVaultSpot()
    {
        Physics.Raycast(
            player.transform.position, 
            player.cam.transform.forward, 
            out hit,
            SPOT_SPAWN_MAX_DISTANCE,
            player.athleteCastMask
        );

        if (hit.collider != null && CheckForSpotPosition(hit))
        {   
            targetPosition = hit.point;

            if (player.CurrentProjectedSpot)
                player.CurrentProjectedSpot.transform.position = targetPosition;
            else
                player.CurrentProjectedSpot = Object.Instantiate(player.vaultSpotProjection, targetPosition, Quaternion.identity);
        }
        else Object.Destroy(player.CurrentProjectedSpot);
    }

    void ProjectVaultStrength()
    {
        surfaces = player.ZoomDetection(POLE_MAX_DISTANCE);

        foreach (RaycastHit surface in surfaces)
        {
            if (IsSurfacePoleVaultSpot(surface))
            {
                player.athleteLineRenderer.gameObject.SetActive(true);

                Color lineColor = GetVaultStrengthColor();

                player.athleteLineRenderer.UpdateLine(targetPosition, lineColor);
                break;
            }
            else player.athleteLineRenderer.gameObject.SetActive(false);
        }
    }

    Color GetVaultStrengthColor()
    {
        float percentage = Vector3.Distance(player.transform.position, targetPosition - new Vector3(0, 0.5f, 0f)) / POLE_MAX_DISTANCE;

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
