using UnityEngine;

public class FourBoneIKSolver : MonoBehaviour
{
    public Transform hip;
    public Transform upperLeg;
    public Transform lowerLeg;
    public Transform foot;
    public Transform target;

    void Update()
    {
        SolveIK();
    }

    void SolveIK()
    {
        // Calculate the positions and rotations for each bone
        // This is a simplified example and may need adjustments for your specific use case
        Vector3 targetPosition = target.position;

        // Calculate the distance between each bone
        float upperLegLength = Vector3.Distance(hip.position, upperLeg.position);
        float lowerLegLength = Vector3.Distance(upperLeg.position, lowerLeg.position);
        float footLength = Vector3.Distance(lowerLeg.position, foot.position);

        // Calculate the direction vectors
        Vector3 directionToTarget = (targetPosition - hip.position).normalized;

        // Position upper leg
        upperLeg.position = hip.position + directionToTarget * upperLegLength;

        // Position lower leg
        lowerLeg.position = upperLeg.position + directionToTarget * lowerLegLength;

        // Position foot
        foot.position = lowerLeg.position + directionToTarget * footLength;

        // Adjust rotations
        upperLeg.LookAt(lowerLeg);
        lowerLeg.LookAt(foot);
        foot.LookAt(target);
    }
}
