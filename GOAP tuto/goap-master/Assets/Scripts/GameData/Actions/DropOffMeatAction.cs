
using System;
using UnityEngine;

public class DropOffMeatAction : GoapAction
{
	private bool droppedOffMeat = false;
	private InnComponent targetInn; // where we drop off the  tools
	
	public DropOffMeatAction() {
		addPrecondition ("hasMeat", true); // can't drop off meat if we don't already have some
		addEffect ("hasMeat", false); // we now have no meat
		addEffect ("collectMeat", true); // we collected meat
	}
	
	
	public override void reset ()
	{
		droppedOffMeat = false;
		targetInn = null;
	}
	
	public override bool isDone ()
	{
		return droppedOffMeat;
	}
	
	public override bool requiresInRange ()
	{
		return true; // yes we need to be near a supply pile so we can drop off the tools
	}
	
	public override bool checkProceduralPrecondition (GameObject agent)
	{
        // find the nearest supply pile that has spare tools
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
        targetInn.NumMeat += backpack.numMeat;
        droppedOffMeat = true;
        backpack.numMeat = 0;

        return true;
	}
}