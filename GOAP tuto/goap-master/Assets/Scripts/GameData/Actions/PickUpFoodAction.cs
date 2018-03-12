using System;
using UnityEngine;

public class PickUpFoodAction : GoapAction
{
	private bool hasFood = false;
	private InnComponent targetInn; // where we get the tool from

	public PickUpFoodAction() {
		addPrecondition ("hasFood", false);
		addEffect ("hasFood", true); // we now have a tool
	}

	
	public override void reset ()
	{
		hasFood = false;
		targetInn = null;
	}
	
	public override bool isDone ()
	{
		return hasFood;
	}

	public override bool requiresInRange ()
	{
		return true; // yes we need to be near a supply pile so we can pick up the tool
	}

	public override bool checkProceduralPrecondition (GameObject agent)
	{
        // find the nearest supply pile that has spare tools
        InnComponent[] inns = (InnComponent[]) UnityEngine.GameObject.FindObjectsOfType ( typeof(InnComponent) );
        InnComponent closest = null;
		float closestDist = 0;

		foreach (InnComponent inn in inns) {
			if (inn.NumFood >= 5) {
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
		}
		if (closest == null)
			return false;

		targetInn = closest;
		target = targetInn.gameObject;

		return closest != null;
	}

	public override bool perform (GameObject agent)
	{
		if (targetInn.NumFood >= 5) {
			targetInn.NumFood -= 5;
			hasFood = true;

			// create the tool and add it to the agent

			BackpackComponent backpack = (BackpackComponent)agent.GetComponent(typeof(BackpackComponent));
			backpack.numFood = 5;

			return true;
		} else {
			// we got there but there was no tool available! Someone got there first. Cannot perform action
			return false;
		}
	}

}


