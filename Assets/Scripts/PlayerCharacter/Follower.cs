using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Flawless.PlayerCharacter
{
    public class Follower : MonoBehaviour
    {
        public PlanetController FollowTarget;

        public float FollowSpeed = 5f;
        public float DistanceThreshold = 0.2f;
        public float LookAtSpeed = 1f;

        public Vector3 PositionOffset;
        public AnimationCurve SpeedCurve;

        public bool IsLookingAt = true;
        public bool IsMovingTo = true;

        // Update is called once per frame
        void Update()
        {
            var offset = Quaternion.LookRotation(FollowTarget.Velocity.normalized, Vector3.up) * PositionOffset;
            // Update Rotation
            if (IsLookingAt)
            {
                var targetRotation =
                    Quaternion.LookRotation((FollowTarget.transform.position + offset - transform.position).normalized,
                        Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * LookAtSpeed);
            }

            // Update Position
            if (IsMovingTo)
            {
                var distance = Vector3.Distance(transform.position, FollowTarget.transform.position + offset);
                if (distance < DistanceThreshold) return;

                transform.position += (FollowTarget.transform.position + offset - transform.position).normalized *
                                      (FollowSpeed * SpeedCurve.Evaluate(distance / 7f) * Time.deltaTime);
            }
        }
    }
}