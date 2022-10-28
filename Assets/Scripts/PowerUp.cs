using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public PowerUpType powerUpType;
    
}

public enum PowerUpType
{
    None,
    Pushback,
    Rockets,
    Smash
}