using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 6f;

/*    public float animSpeed = 1.5f;				// アニメーション再生速度設定

    private Animator anim;                          // キャラにアタッチされるアニメーターへの参照
    private AnimatorStateInfo currentBaseState;         // base layerで使われる、アニメーターの現在の状態の参照

	// アニメーター各ステートへの参照
	static int idleState = Animator.StringToHash("Base Layer.Idle");
	static int locoState = Animator.StringToHash("Base Layer.Locomotion");
	static int jumpState = Animator.StringToHash("Base Layer.Jump");
	static int restState = Animator.StringToHash("Base Layer.Rest");
*/

	private bool waitForClose = false;

	private bool waitForCloseMessage = false;
	public Rigidbody localPlayerRb;

	public void FixedUpdate()
    {
		SendInputToServer();
/*		float lastSyncDifference = Time.time - GameManager.players[Client.instance.myId].lastPositionUpdate;
		Debug.Log(lastSyncDifference);
		if (lastSyncDifference > 0.001f)
		{
			localPlayerRb.isKinematic = false;
			localPlayerRb.useGravity = true;
			localPlayerRb.MovePosition(localPlayerRb.position + GameManager.players[Client.instance.myId].lastVelocityValue * lastSyncDifference);
			localPlayerRb.isKinematic = true;
			localPlayerRb.useGravity = false;
		}*/
	}

    public void Start()
    {
		//anim = GetComponent<Animator>();
	}

    public void SendInputToServer()
    {
        bool[] _inputs = new bool[]
        {
            Input.GetKey(KeyCode.Space),
			Input.GetKey(KeyCode.E)
        };

        Transform cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
		float horizontal = Input.GetAxisRaw("Horizontal");
		float vertical = Input.GetAxisRaw("Vertical");

		// Changing the cinecine camera max speed
		GameManager.players[Client.instance.myId].thirdPersonCamera.m_XAxis.m_MaxSpeed = GameManager.players[Client.instance.myId].sensitivityX * 30;
		GameManager.players[Client.instance.myId].thirdPersonCamera.m_YAxis.m_MaxSpeed = GameManager.players[Client.instance.myId].sensitivityY;

		if (Input.GetKey(KeyCode.Escape) && !waitForClose && !UIManager.instance.messageActive)
        {
			UIManager.instance.toggleSettingsActive();
			StartCoroutine(LateCall());
		}


		if (Input.GetKey(KeyCode.Return) && !waitForCloseMessage)
		{

			StartCoroutine(LateCallMessage());
			if (!UIManager.instance.messageActive)
            {
				UIManager.instance.toggleMessageActive();
			}
		}

        if (UIManager.instance.messageActive)
        {
			if (Input.GetKey(KeyCode.Escape))
            {
				UIManager.instance.toggleMessageActive();
				StartCoroutine(LateCall());
			}
        }

		Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (UIManager.instance.settingsActive || UIManager.instance.messageActive)
        {
			Cursor.lockState = CursorLockMode.None;
		} else
        {
			Cursor.lockState = CursorLockMode.Locked;

			ClientSend.PlayerMovement(direction, cam.eulerAngles.y, _inputs);
		}
	}

	IEnumerator LateCall()
	{
		waitForClose = true;
		yield return new WaitForSeconds(0.5f);
		waitForClose = false;
	}

	IEnumerator LateCallMessage()
	{
		waitForCloseMessage = true;
		yield return new WaitForSeconds(0.5f);
		waitForCloseMessage = false;
	}

}
