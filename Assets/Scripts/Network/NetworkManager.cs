using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour
{
	[SerializeField] GameObject player;
	public GameObject enemySpawner;
	public GameObject inGameUI;
	public Text statusText;

	// void Start() {
	// 	PhotonNetwork.autoJoinLobby = true;
	// 	PhotonNetwork.ConnectUsingSettings(version);
	// }

	// public virtual void OnJoinedLobby() {
	// 	RoomOptions roomOptions = new RoomOptions() { IsVisible = false, MaxPlayers = 4 };
	// 	PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
	// }

	// public virtual void OnJoinedRoom() {
	void Start()
	{
		// lobbyCam.SetActive(false);
		// lobbyUI.SetActive(false);

		// GameObject playerObj = Instantiate(player, spawnPoint.position, spawnPoint.rotation);
		// GameObject player = PhotonNetwork.Instantiate(playerName, spawnPoint.position, spawnPoint.rotation, 0);
		// players.Add(player);

		// inGameUI.SetActive(true);
		// enemySpawner.SetActive(true);
		// enemySpawner.GetComponent<EnemySpawner>().target = playerObj;
	}

	// void Update() {
	// 	statusText.text = PhotonNetwork.connectionStateDetailed.ToString();
	// }
}