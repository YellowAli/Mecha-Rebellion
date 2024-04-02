// Designed by KINEMATION, 2023

using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace KINEMATION.KAnimationCore.Runtime.Core
{
    public struct KTwoBoneIkData
    {
        public KTransform Root;
        public KTransform Mid;
        public KTransform Tip;
        public KTransform Target;
        public KTransform Hint;

        public float PosWeight;
        public float RotWeight;
        public float HintWeight;

        public bool HasValidHint;
    }
    
    public class KTwoBoneIK
    {
        public static void Solve(ref KTwoBoneIkData ikData)
        {
            Vector3 aPosition = ikData.Root.position;
            Vector3 bPosition = ikData.Mid.position;
            Vector3 cPosition = ikData.Tip.position;
            
            Vector3 tPosition = Vector3.Lerp(cPosition, ikData.Target.position, ikData.PosWeight);
            Quaternion tRotation = Quaternion.Lerp(ikData.Tip.rotation, ikData.Target.rotation, ikData.RotWeight);
            bool hasHint = ikData.HasValidHint && ikData.HintWeight > 0f;

            Vector3 ab = bPosition - aPosition;
            Vector3 bc = cPosition - bPosition;
            Vector3 ac = cPosition - aPosition;
            Vector3 at = tPosition - aPosition;

            float abLen = ab.magnitude;
            float bcLen = bc.magnitude;
            float acLen = ac.magnitude;
            float atLen = at.magnitude;

            float oldAbcAngle = KMath.TriangleAngle(acLen, abLen, bcLen);
            float newAbcAngle = KMath.TriangleAngle(atLen, abLen, bcLen);

            // Bend normal strategy is to take whatever has been provided in the animation
            // stream to minimize configuration changes, however if this is collinear
            // try computing a bend normal given the desired target position.
            // If this also fails, try resolving axis using hint if provided.
            Vector3 axis = Vector3.Cross(ab, bc);
            if (axis.sqrMagnitude < KMath.SqrEpsilon)
            {
                axis = hasHint ? Vector3.Cross(ikData.Hint.position - aPosition, bc) : Vector3.zero;

                if (axis.sqrMagnitude < KMath.SqrEpsilon)
                    axis = Vector3.Cross(at, bc);

                if (axis.sqrMagnitude < KMath.SqrEpsilon)
                    axis = Vector3.up;
            }

            axis = Vector3.Normalize(axis);

            float a = 0.5f * (oldAbcAngle - newAbcAngle);
            float sin = Mathf.Sin(a);
            float cos = Mathf.Cos(a);
            Quaternion deltaR = new Quaternion(axis.x * sin, axis.y * sin, axis.z * sin, cos);

            KTransform localTip = ikData.Mid.GetRelativeTransform(ikData.Tip, false);
            ikData.Mid.rotation = deltaR * ikData.Mid.rotation;
            
            // Update child transform.
            ikData.Tip = ikData.Mid.GetWorldTransform(localTip, false);
            
            cPosition = ikData.Tip.position;
            ac = cPosition - aPosition;

            KTransform localMid = ikData.Root.GetRelativeTransform(ikData.Mid, false);
            localTip = ikData.Mid.GetRelativeTransform(ikData.Tip, false);
            ikData.Root.rotation = KMath.FromToRotation(ac, at) * ikData.Root.rotation;

            // Update child transforms.
            ikData.Mid = ikData.Root.GetWorldTransform(localMid, false);
            ikData.Tip = ikData.Mid.GetWorldTransform(localTip, false);

            if (hasHint)
            {
                float acSqrMag = ac.sqrMagnitude;
                if (acSqrMag > 0f)
                {
                    bPosition = ikData.Mid.position;
                    cPosition = ikData.Tip.position;
                    ab = bPosition - aPosition;
                    ac = cPosition - aPosition;

                    Vector3 acNorm = ac / Mathf.Sqrt(acSqrMag);
                    Vector3 ah = ikData.Hint.position - aPosition;
                    Vector3 abProj = ab - acNorm * Vector3.Dot(ab, acNorm);
                    Vector3 ahProj = ah - acNorm * Vector3.Dot(ah, acNorm);

                    float maxReach = abLen + bcLen;
                    if (abProj.sqrMagnitude > (maxReach * maxReach * 0.001f) && ahProj.sqrMagnitude > 0f)
                    {
                        Quaternion hintR = KMath.FromToRotation(abProj, ahProj);
                        hintR.x *= ikData.HintWeight;
                        hintR.y *= ikData.HintWeight;
                        hintR.z *= ikData.HintWeight;
                        hintR = KMath.NormalizeSafe(hintR);
                        ikData.Root.rotation = hintR * ikData.Root.rotation;
                        
                        ikData.Mid = ikData.Root.GetWorldTransform(localMid, false);
                        ikData.Tip = ikData.Mid.GetWorldTransform(localTip, false);
                    }
                }
            }
            
            ikData.Tip.rotation = tRotation;
        }
    }

    public struct KTwoBoneIKJob : IJobParallelFor
    {
        public NativeArray<KTwoBoneIkData> TwoBoneIkJobData;

        public void Execute(int index)
        {
            var twoBoneIkData = TwoBoneIkJobData[index];
            KTwoBoneIK.Solve(ref twoBoneIkData);
            TwoBoneIkJobData[index] = twoBoneIkData;
        }
    }
}