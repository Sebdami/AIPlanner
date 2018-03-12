using System;
using UnityEngine;

public class PickUpMeatAction : GoapAction
{
	private bool hasTool = false;
	private InnComponent targetInn; // where we get the tool from

	public PickUpMeatAction() {
		addPrecondition ("hasMeat", false); // don't get a tool if we already have one
		addEffect ("hasMeat", true); // we now have meat
	}

	
	public override void reset ()
	{
		hasTool = false;
		targetInn = null;
	}
	
	public override bool isDone ()
	{
		return hasTool;
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
			if (inn.NumMeat > 5) {
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
		if (targetInn.NumMeat >= 5) {
			targetInn.NumMeat -= 5;
			hasTool = true;

			// create the tool and add it to the agent

			BackpackComponent backpack = (BackpackComponent)agent.GetComponent(typeof(BackpackComponent));
			backpack.numMeat = 5;

			return true;
		} else {
			// we got there but there was no tool available! Someone got there first. Cannot perform action
			return false;
		}
	}

}


