using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticGrabberCardBehavior : CardBehavior
{
    public override void CheckLegality(Node node)
    {
        if (node.currentEntity.GetComponent<DragMonster>() != null)
        {
            Debug.Log("This monster already able to drag");

            return;
        }

        base.CheckLegality(node);
    }

    public override void CastCard(Node node)
    {
        targetMonster.gameObject.AddComponent<DragMonster>();
    }
}
