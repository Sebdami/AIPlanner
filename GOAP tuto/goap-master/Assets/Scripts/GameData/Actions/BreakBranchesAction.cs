
using System;
using UnityEngine;

public class BreakBranchesAction : GoapAction
{
	private bool finished = false;
	private TreeComponent targetTree; // where we get the logs from
	
	private float startTime = 0;
	public float workDuration = 3.0f; // seconds
	
	public BreakBranchesAction() {
        //addPrecondition ("hasTool", true); // we need a tool to do this
        addPrecondition("hasLogs", false);
        addPrecondition ("hasFirewood", false); // if we have logs we don't want more
        addPrecondition("hasFood", true);
        addEffect ("hasFirewood", true);
	}
	
	
	public override void reset ()
	{
		finished = false;
		targetTree = null;
		startTime = 0;
	}
	
	public override bool isDone ()
	{
		return finished;
	}
	
	public override bool requiresInRange ()
	{
		return true; // yes we need to be near a tree
	}
	
	public override bool checkProceduralPrecondition (GameObject agent)
	{
		// find the nearest tree that we can chop
		TreeComponent[] trees = (TreeComponent[]) UnityEngine.GameObject.FindObjectsOfType ( typeof(TreeComponent) );
		TreeComponent closest = null;
		float closestDist = 0;
		
		foreach (TreeComponent tree in trees) {
			if (closest == null) {
				// first one, so choose it for now
				closest = tree;
				closestDist = (tree.gameObject.transform.position - agent.transform.position).magnitude;
			} else {
				// is this one closer than the last?
				float dist = (tree.gameObject.transform.position - agent.transform.position).magnitude;
				if (dist < closestDist) {
					// we found a closer one, use it
					closest = tree;
					closestDist = dist;
				}
			}
		}
		if (closest == null)
			return false;

		targetTree = closest;
		target = targetTree.gameObject;
		
		return closest != null;
	}
	
	public override bool perform (GameObject agent)
	{
		if (startTime == 0)
			startTime = Time.time;
		
		if (Time.time - startTime > workDuration) {
			// finished chopping
			BackpackComponent backpack = (BackpackComponent)agent.GetComponent(typeof(BackpackComponent));
			backpack.numFirewood += 1;
            backpack.numFood--;
            finished = true;
		}
		return true;
	}
	
}