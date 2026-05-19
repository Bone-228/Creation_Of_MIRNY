using System;
using UnityEngine;

/*
PSEUDOCODE / PLAN (detailed):

- Purpose:
  Implement a two-bone turret IK:
    - boneYaw: rotates only around its up axis (right/left).
    - bonePitch: rotates only around its right axis (up/down).

- Data:
  public Transform target
  public Transform aimTransform (raycast / aim reference point)
  public Transform boneYaw (first bone, handles yaw)
  public Transform bonePitch (second bone, handles pitch)

- Per frame (LateUpdate):
  1. Validate required references (early exit if null).
  2. Compute world direction from aimTransform.position to target.position:
       dir = (target.position - aimTransform.position)
  3. YAW STEP (rotate boneYaw around its up axis only):
     a. axisYaw = boneYaw.up (world-space axis)
     b. Project both dir and current aim forward onto plane orthogonal to axisYaw:
         targetProj = ProjectOnPlane(dir, axisYaw)
         currentProj = ProjectOnPlane(aimTransform.forward, axisYaw)
     c. If projections are valid (length > epsilon):
         signedYaw = SignedAngle(currentProj, targetProj, axisYaw)
         Apply rotation: boneYaw.rotation = Quaternion.AngleAxis(signedYaw, axisYaw) * boneYaw.rotation
     d. This changes orientation of children (ideally aimTransform is under these bones).
  4. Recompute dir = (target.position - aimTransform.position) because aimTransform.forward may have changed.
  5. PITCH STEP (rotate bonePitch around its right axis only):
     a. axisPitch = bonePitch.right (world-space axis)
     b. Project both dir and current aim forward onto plane orthogonal to axisPitch:
         targetProj = ProjectOnPlane(dir, axisPitch)
         currentProj = ProjectOnPlane(aimTransform.forward, axisPitch)
     c. If projections are valid:
         signedPitch = SignedAngle(currentProj, targetProj, axisPitch)
         Apply rotation: bonePitch.rotation = Quaternion.AngleAxis(signedPitch, axisPitch) * bonePitch.rotation
  6. Done.

- Notes:
  - Use Vector3.ProjectOnPlane to remove component along axis.
  - Use Vector3.SignedAngle to get direction (sign) around axis.
  - Small epsilon checks to avoid division by zero/NaNs.
  - Operate in world space so this works even if bones are not parented exactly as expected,
    but best results occur if aimTransform is a child of the bones hierarchy.
*/

public class TurretIK : MonoBehaviour
{
    public Transform target; // target to aim at
    public Transform aimTransform; // raycast node / aim reference
    public Transform boneYaw; // bone 1 -> only right/left (yaw)
    public Transform bonePitch; // bone 2 -> only up/down (pitch)

    // Optional: clamp angles (degrees). Set to large values to disable clamping.
    public float maxYawDegrees = 180f;
    public float maxPitchDegrees = 90f;

    private const float kEpsilon = 1e-6f;

    private void LateUpdate()
    {
        if (target == null || aimTransform == null || boneYaw == null || bonePitch == null)
            return;

        // Initial direction from the aim origin to the target
        Vector3 dir = target.position - aimTransform.position;
        if (dir.sqrMagnitude < kEpsilon)
            return;

        // --- YAW (right/left) around boneYaw.up ---
        Vector3 axisYaw = boneYaw.up; // world-space axis for yaw
        Vector3 targetProjYaw = Vector3.ProjectOnPlane(dir, axisYaw);
        Vector3 currentProjYaw = Vector3.ProjectOnPlane(aimTransform.forward, axisYaw);

        if (targetProjYaw.sqrMagnitude >= kEpsilon && currentProjYaw.sqrMagnitude >= kEpsilon)
        {
            float signedYaw = Vector3.SignedAngle(currentProjYaw, targetProjYaw, axisYaw);
            // Apply optional clamping
            signedYaw = Mathf.Clamp(signedYaw, -Mathf.Abs(maxYawDegrees), Mathf.Abs(maxYawDegrees));
            // Rotate boneYaw around its up axis in world space
            boneYaw.rotation = Quaternion.AngleAxis(signedYaw, axisYaw) * boneYaw.rotation;
        }

        // Recompute direction because aimTransform.forward may have changed after yaw
        dir = target.position - aimTransform.position;
        if (dir.sqrMagnitude < kEpsilon)
            return;

        // --- PITCH (up/down) around bonePitch.right ---
        Vector3 axisPitch = bonePitch.right; // world-space axis for pitch
        Vector3 targetProjPitch = Vector3.ProjectOnPlane(dir, axisPitch);
        Vector3 currentProjPitch = Vector3.ProjectOnPlane(aimTransform.forward, axisPitch);

        if (targetProjPitch.sqrMagnitude >= kEpsilon && currentProjPitch.sqrMagnitude >= kEpsilon)
        {
            float signedPitch = Vector3.SignedAngle(currentProjPitch, targetProjPitch, axisPitch);
            // Apply optional clamping
            signedPitch = Mathf.Clamp(signedPitch, -Mathf.Abs(maxPitchDegrees), Mathf.Abs(maxPitchDegrees));
            // Rotate bonePitch around its right axis in world space
            bonePitch.rotation = Quaternion.AngleAxis(signedPitch, axisPitch) * bonePitch.rotation;
        }
    }
}
