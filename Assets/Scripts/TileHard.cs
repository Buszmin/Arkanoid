using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileHard : Tile
{
    [SerializeField] Renderer ren;
    [SerializeField] Material CrackedMaterial;

    [SerializeField] AudioSource damage;
    public bool cracked;

    public override void Hit()
    {
        if(cracked)
        {
            base.Hit();
        }
        else
        {
            damage.Play();
            ren.material = CrackedMaterial;
            data.type = 2;
            cracked = true;
        }
    }
}
