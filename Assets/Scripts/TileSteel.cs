using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSteel : Tile
{
    [SerializeField] AudioSource hit;

    public override void Hit()
    {
        hit.Play();
    }

    protected override void Die()
    {
        PlayerManager.Instance.AddScore(score);
    }
}
