using UnityEngine;

public class Enemy_Range_IK : MonoBehaviour
{
    public Enemy_Range enemyRangeAnimatioLogick;
    public Transform target;
    public Transform aimRayTransform;
    public Transform bone;
    public int iterations = 10;
    public float angleLimit = 90.0f;
    public float distanceLimit = 1.5f;
    // Update is called once per frame
    void LateUpdate()
    {
        if (enemyRangeAnimatioLogick.playerInRange) 
        {
            Vector3 targetPosition = getTargetPostion();
            for(int i = 0; i < iterations; i++)
            {
                AimAtTarget(bone,targetPosition);
            }
        }
    }
    public Vector3 getTargetPostion() 
    { 
        Vector3 targetDirection = target.position - aimRayTransform.position;
        Vector3 aimDirecgtion = aimRayTransform.forward;
        float blendout = 0.0f;

        float targetAngle = Vector3.Angle(targetDirection, aimDirecgtion);

        if (targetAngle > angleLimit) 
        {
            blendout += (targetAngle - angleLimit) / 50.0f;
        }

        float targetDistance = targetDirection.magnitude;
        if (targetDistance < distanceLimit) 
        {
            blendout += distanceLimit - targetDistance;
        }

        Vector3 direction = Vector3.Slerp(targetDirection, aimDirecgtion, blendout);
        return aimRayTransform.position + direction;
    }
    void AimAtTarget(Transform bone, Vector3 targetPosition)
    {
        Vector3 aimDirection = aimRayTransform.forward;
        Vector3 targetDirection = targetPosition - aimRayTransform.position;
        Quaternion aimTowards = Quaternion.FromToRotation(aimDirection, targetDirection);
        bone.rotation = aimTowards * bone.rotation;
    }
}