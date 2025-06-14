using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[Header("Clone Settings")]
	[SerializeField] private GameObject playerClonePrefab;
	[SerializeField] private Transform cloneSpawnPoint;

	private GameObject spawnedClone;
	private bool hasSpawnedClone = false;

	/// <summary>
	/// Gọi hàm này sau khi đủ 7 bước.
	/// </summary>
	public void SummonPlayerClone(List<string> recordedMoves)
	{
		//Chỉ cho phép sinh clone 1 lần duy nhất
		if (hasSpawnedClone || recordedMoves.Count != 7) return;

		spawnedClone = Instantiate(playerClonePrefab, cloneSpawnPoint.position, Quaternion.identity);
		hasSpawnedClone = true;

		PlayerClone cloneScript = spawnedClone.GetComponent<PlayerClone>();
		if (cloneScript != null)
		{
			cloneScript.SetReplayPath(recordedMoves);
		}
		else
		{
			Debug.LogWarning("⚠️ Clone prefab thiếu script PlayerCloneReplay!");
		}
	}
}
