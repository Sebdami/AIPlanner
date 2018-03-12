
using System;
using UnityEngine;

public class FryFoodAction : GoapAction
{
	private bool chopped = false;
	private InnComponent targetInn; // where we get the logs from
	
	private float startTime = 0;
	public float workDuration = 2; // seconds
	
	public FryFoodAction() {
		addPrecondition ("hasTool", true); // we need a tool to do this
		addPrecondition ("hasFood", false); // if we have logs we don't want more
        addPrecondition("hasMeat", true);
        addPrecondition("hasFirewood", true);
        addEffect ("hasFood", true);
	}
	
	
	public override void reset ()
	{
		chopped = false;
		targetInn = null;
		startTime = 0;
	}
	
	public override bool isDone ()
	{
		return chopped;
	}
	
	public override bool requiresInRange ()
	{
		return true; // yes we need to be near a tree
	}
	
	public override bool checkProceduralPrecondition (GameObject agent)
	{
		// find the nearest tree that we can chop
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
		if (startTime == 0)
			startTime = Time.time;
		
		if (Time.time - startTime > workDuration) {
			// finished cooking
			BackpackComponent backpack = (BackpackComponent)agent.GetComponent(typeof(BackpackComponent));
			backpack.numFood += 5;
            backpack.numMeat = 0;
            backpack.numFirewood = 0;
			chopped = true;
			ToolComponent tool = backpack.tool.GetComponent(typeof(ToolComponent)) as ToolComponent;
			tool.use(0.34f);
			if (tool.destroyed()) {
				Destroy(backpack.tool);
				backpack.tool = null;
			}
		}
		return true;
	}
	
}