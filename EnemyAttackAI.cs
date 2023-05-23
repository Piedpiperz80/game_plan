using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackAI : Movement
{
    public Transform DefendCircle; // The player's defend circle transform
    public float boostDistance = 2f; // The distance at which the enemy starts to boost

    protected override void Start()
    {
        base.Start(); // Call the base class's Start method
    }

    protected override void Update()
    {
        base.Update();

        // Calculate the direction towards the player
        Vector2 directionToPlayer = (DefendCircle.position - transform.position).normalized;

        // Calculate the distance to the defend circle
        float distanceToDefendCircle = Vector2.Distance(transform.position, DefendCircle.position);

        // If the enemy is close to the player, boost
        if (distanceToDefendCircle < boostDistance && character.CurrentStamina > boostCost)
        {
            Boost(directionToPlayer);
        }
        else
        {
            Move(directionToPlayer);
        }
    }
}
