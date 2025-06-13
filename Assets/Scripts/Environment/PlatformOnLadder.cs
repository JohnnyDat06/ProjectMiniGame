using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformOnLadder : MonoBehaviour
{
	[SerializeField] private PlatformEffector2D platformEffector;
	[SerializeField] private float waitTimeBeforeReenable = 0.5f;


	public void DropPlayer()
	{
		if (platformEffector != null)
		{
			platformEffector.rotationalOffset = 180f;
			Invoke(nameof(ResetEffector), waitTimeBeforeReenable);
		}
	}

	private void ResetEffector()
	{
		if (platformEffector != null)
		{
			platformEffector.rotationalOffset = 0f;
		}
	}
}
