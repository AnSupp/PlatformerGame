using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    private Slider healthBar;

    private void Awake()
    {
        healthBar = GetComponent<Slider>();
    }

    public void SetHealth(int health)
    {
        healthBar.value = health;
    }
}
