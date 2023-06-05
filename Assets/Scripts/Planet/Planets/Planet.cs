using Flawless.LifeSys;
using UnityEngine;
using UnityEngine.Serialization;

namespace Flawless.Planet
{
    public class Planet : MonoBehaviour
    {
        public float CollideDamage = 200f;
        public float CollideForce = 10f;

        public virtual void CollideAndDamageLife(Rigidbody playerRigidbody, PlayerLifeAmount playerLifeAmount,
            Vector3 normal)
        {
            playerLifeAmount.LifeAmount -= CollideDamage;
            var velocityDir = playerRigidbody.velocity.normalized;
            var boundDirection = velocityDir +
                                 Mathf.Abs(2 * Vector3.Dot(velocityDir, normal)) * normal;
            playerRigidbody.AddForce(boundDirection * CollideForce - playerRigidbody.velocity,
                ForceMode.VelocityChange);
        }
    }
}