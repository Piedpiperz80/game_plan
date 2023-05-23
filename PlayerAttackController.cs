using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    public float speed = 0.3f;
    public float boostSpeed = 0.9f;
    public float boostCost = 10f; // The stamina cost of boosting
    public Player player;


    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player.CurrentStamina = player.Stamina; // Set current stamina to max stamina
    }

    void Update()
    {
        float moveHorizontal = Input.GetKey(KeyCode.Quote) ? 1 : Input.GetKey(KeyCode.L) ? -1 : 0;
        float moveVertical = Input.GetKey(KeyCode.P) ? 1 : Input.GetKey(KeyCode.Semicolon) ? -1 : 0;

        Vector2 movement = new Vector2(moveHorizontal, moveVertical);

        // If the player is holding boost key and has enough stamina, boost
        if (Input.GetKey(KeyCode.RightShift) && player.CurrentStamina > boostCost)
        {
            rb.AddForce(movement * boostSpeed);
            player.CurrentStamina -= boostCost * Time.deltaTime; // Decrease stamina
        }
        else
        {
            rb.AddForce(movement * speed);
        }

        // Regenerate stamina
        if (moveHorizontal == 0 && moveVertical == 0) // If the player is standing still
        {
            player.CurrentStamina = Mathf.Min(player.CurrentStamina + Time.deltaTime * 0.5f, player.Stamina); // Regenerate stamina faster
        }
        else
        {
            player.CurrentStamina = Mathf.Min(player.CurrentStamina + Time.deltaTime * 0.1f, player.Stamina); // Normal stamina regeneration
        }

        // Limit position
        Vector3 position = transform.position;
        float screenWidth = Camera.main.aspect * Camera.main.orthographicSize;
        float screenHeight = Camera.main.orthographicSize;
        position.x = Mathf.Clamp(position.x, -screenWidth, screenWidth);
        position.y = Mathf.Clamp(position.y, -screenHeight, screenHeight);
        transform.position = position;
    }
}
