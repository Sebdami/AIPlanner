
using System;
using UnityEngine;

public class ProduceMeatAction : GoapAction
{
	private bool chopped = false;
	private GardenComponent targetGarden; // where we get the logs from
	
	private float startTime = 0;
	public float workDuration = 2; // seconds
	
	public ProduceMeatAction() {
		addPrecondition ("hasTool", true); // we need a tool to do this
		addPrecondition ("hasMeat", false); // if we have meat we don't want more
        addPrecondition("hasFood", true);
        addEffect ("hasMeat", true);
	}
	
	
	public override void reset ()
	{
		chopped = false;
		targetGarden = null;
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
		GardenComponent[] gardens = (GardenComponent[]) UnityEngine.GameObject.FindObjectsOfType ( typeof(GardenComponent) );
		GardenComponent closest = null;
		float closestDist = 0;
		
		foreach (GardenComponent garden in gardens) {
			if (closest == null) {
				// first one, so choose it for now
				closest = garden;
				closestDist = (garden.gameObject.transform.position - agent.transform.position).magnitude;
			} else {
				// is this one closer than the last?
				float dist = (garden.gameObject.transform.position - agent.transform.position).magnitude;
				if (dist < closestDist) {
					// we found a closer one, use it
					closest = garden;
					closestDist = dist;
				}
			}
		}
		if (closest == null)
			return false;

		targetGarden = closest;
		target = targetGarden.gameObject;
		
		return closest != null;
	}
	
	public override bool perform (GameObject agent)
	{
		if (startTime == 0)
			startTime = Time.time;
		
		if (Time.time - startTime > workDuration) {
			// finished chopping chickens
			BackpackComponent backpack = (BackpackComponent)agent.GetComponent(typeof(BackpackComponent));
			backpack.numMeat += 5;
            backpack.numFood--;
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