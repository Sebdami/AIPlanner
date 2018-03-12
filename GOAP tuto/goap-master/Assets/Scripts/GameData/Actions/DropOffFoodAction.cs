
using System;
using UnityEngine;

public class DropOffFoodAction: GoapAction
{
	private bool droppedOffFood = false;
	private InnComponent targetInn; // where we drop off the food

    public DropOffFoodAction() {
		addPrecondition ("hasFood", true); // can't drop off food if we don't already have some
        addEffect ("hasFood", false); // we now have no food
        addEffect ("collectFood", true); // we collected food
	}
	
	
	public override void reset ()
	{
		droppedOffFood = false;
		targetInn = null;
	}
	
	public override bool isDone ()
	{
		return droppedOffFood;
	}
	
	public override bool requiresInRange ()
	{
		return true; // yes we need to be near an inn so we can drop off the food
    }
	
	public override bool checkProceduralPrecondition (GameObject agent)
	{
        // find the nearest inn
        InnComponent[] inns = (InnComponent[]) UnityEngine.GameObject.FindObjectsOfType ( typeof(InnComponent) );
        InnComponent closest = null;
		float closestDist = 0;
		
		foreach (InnComponent inn in inns) {
			if (closest == null) {
				// first one, so choose it for now
				closest = inn;
				closestDist = (inn.gameObject.transform.position - agent.transform.position).magnitude;
			} else {
				// is this one closer than the last?
				float dist = (inn.gameObject.transform.position - agent.transform.position).magnitude;
				if (dist < closestDist) {
					// we found a closer one, use it
					closest = inn;
					closestDist = dist;
				}
			}
		}
		if (closest == null)
			return false;

		targetInn = closest;
		target = targetInn.gameObject;
		
		return closest != null;
	}
	
	public override bool perform (GameObject agent)
	{
		BackpackComponent backpack = (BackpackComponent)agent.GetComponent(typeof(BackpackComponent));
		targetInn.NumFood += backpack.numFood;
		droppedOffFood = true;
		backpack.numFood = 0;
		
		return true;
	}
}
