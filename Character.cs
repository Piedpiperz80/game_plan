using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    // Abilities
    public float Agility;
    public float Constitution;
    public float Stamina;
    public float Strength;
    public float Charisma;
    public float Creativity;
    public float Intelligence;
    public float Perception;
    public float Willpower;

    // Current stamina
    public float CurrentStamina;

    // Skills
    public float Lockpicking; // Primary: Agility, Secondary: Intelligence, Perception
    public float Speed; // Primary: Agility, Secondary: Stamina, Constitution

    // Skill Caps
    public float LockpickingCap;
    public float SpeedCap;

    protected virtual void Awake()
    {     
        // Initialize abilities
        Agility = 50;
        Constitution = 50;
        Stamina = 50;
        Strength = 50;
        Charisma = 50;
        Creativity = 50;
        Intelligence = 50;
        Perception = 50;
        Willpower = 50;

        // Initialize skills
        Lockpicking = 0;
        Speed = 0;

        CurrentStamina = Stamina; // Set current stamina to max stamina
    }

    protected virtual void Update()
    {
        // Update skill caps based on abilities
        LockpickingCap = 0.5f * Agility + 0.25f * (Intelligence + Perception);
        SpeedCap = 0.5f * (Agility + Strength);

        // Ensure skills do not exceed their caps
        Lockpicking = Mathf.Min(Lockpicking, LockpickingCap);
        Speed = Mathf.Min(Speed, SpeedCap);
    }
}
