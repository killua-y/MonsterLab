using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallCardBehavior : CardBehavior
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void CastCard(Tile _tile, Card _card = null)
    {
        GridManager.Instance.GetNodeForTile(_tile).currentEntity.TakeDamage(card.effectData);
    }
}
