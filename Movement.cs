using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 0.3f;
    public float boostSpeed = 0.9f;
    public float boostCost = 10f; // The stamina cost of boosting
    public Character character;

    protected Rigidbody2D rb;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        // Regenerate stamina
        if (rb.velocity.magnitude < 0.01f) // If the character is standing still
        {
            character.CurrentStamina = Mathf.Min(character.CurrentStamina + Time.deltaTime * 0.5f, character.Stamina); // Regenerate stamina faster
        }
        else
        {
            character.CurrentStamina = Mathf.Min(character.CurrentStamina + Time.deltaTime * 0.1f, character.Stamina); // Normal stamina regeneration
        }

        // Limit position
        Vector3 position = transform.position;
        float screenWidth = Camera.main.aspect * Camera.main.orthographicSize;
        float screenHeight = Camera.main.orthographicSize;
        position.x = Mathf.Clamp(position.x, -screenWidth, screenWidth);
        position.y = Mathf.Clamp(position.y, -screenHeight, screenHeight);
        transform.position = position;
    }

    protected void Move(Vector2 direction)
    {
        rb.AddForce(direction * speed);
    }

    protected void Boost(Vector2 direction)
    {
        if (character.CurrentStamina > boostCost)
        {
            rb.AddForce(direction * boostSpeed);
            character.CurrentStamina -= boostCost * Time.deltaTime; // Decrease stamina
        }
    }
}
