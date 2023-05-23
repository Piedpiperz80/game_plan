using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDefendAI : Movement
{
    public Transform AttackCircle; // The player's attack circle transform
    public float boostDistance = 2f; // The distance at which the enemy starts to boost
    public float wallAvoidanceDistance = 1.0f; // The distance at which the enemy starts to avoid the walls

    protected override void Start()
    {
        base.Start(); // Call the base class's Start method
    }

    protected override void Update()
    {
        base.Update();

        // Calculate the direction away from the player
        Vector2 directionAwayFromPlayer = (transform.position - AttackCircle.position).normalized;

        // Calculate the distance to the player
        float distanceToPlayer = Vector2.Distance(transform.position, AttackCircle.position);

        // If the enemy is close to the player, boost
        if (distanceToPlayer < boostDistance && character.CurrentStamina > boostCost)
        {
            Boost(directionAwayFromPlayer);
        }
        else
        {
            Move(directionAwayFromPlayer);
        }

        // Avoid walls
        Vector3 position = transform.position;
        float screenWidth = Camera.main.aspect * Camera.main.orthographicSize;
        float screenHeight = Camera.main.orthographicSize;
        if (Mathf.Abs(position.x) > screenWidth - wallAvoidanceDistance)
        {
            float wallAvoidanceForce = (screenWidth - Mathf.Abs(position.x)) / wallAvoidanceDistance;
            rb.AddForce(new Vector2(-Mathf.Sign(position.x) * wallAvoidanceForce, 0));
        }
        if (Mathf.Abs(position.y) > screenHeight - wallAvoidanceDistance)
        {
            float wallAvoidanceForce = (screenHeight - Mathf.Abs(position.y)) / wallAvoidanceDistance;
            rb.AddForce(new Vector2(0, -Mathf.Sign(position.y) * wallAvoidanceForce));
        }
    }
}
