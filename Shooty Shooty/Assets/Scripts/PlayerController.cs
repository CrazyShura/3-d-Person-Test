using UnityEngine;

public class PlayerController : MonoBehaviour
{
	#region Fields
	[SerializeField]
	float speed = .1f;
	[SerializeField]
	float drawSpeed = 3f;
	[SerializeField]
	Joystick moveControl;
	[SerializeField]
	Joystick lookControl;
	[SerializeField]
	Animator animator;
	CharacterController controller;
	float gravPull;
	LayerMask groundMask;
	#endregion

	#region Properties

	#endregion

	#region Methods
	private void Awake()
	{
		controller = GetComponent<CharacterController>();
		groundMask = LayerMask.GetMask("Ground");
	}
	private void FixedUpdate()
	{
		Vector3 _move = new Vector3(moveControl.Direction.x, 0, moveControl.Direction.y);
		Vector3 _look = new Vector3(lookControl.Direction.x, 0, lookControl.Direction.y);

		if (_look.magnitude > 0)
		{
			transform.forward = _look;
			float _temp = animator.GetLayerWeight(1) + drawSpeed * Time.fixedDeltaTime;
			_temp = Mathf.Clamp01(_temp);
			animator.SetLayerWeight(1, _temp);
		}
		else
		{
			float _temp = animator.GetLayerWeight(1) - drawSpeed * Time.fixedDeltaTime;
			_temp = Mathf.Clamp01(_temp);
			animator.SetLayerWeight(1, _temp);
		}
		if (_move.magnitude > 0)
		{
			if (_look.magnitude == 0)
			{
				transform.forward = _move;
				animator.SetFloat("_x", 0);
				animator.SetFloat("_y", 1);
			}
			else
			{
				Vector3 _temp = (Quaternion.AngleAxis(Vector3.SignedAngle(_move, _look, Vector3.down), Vector3.up) * Vector3.forward).normalized;
				animator.SetFloat("_x", _temp.x);
				animator.SetFloat("_y", _temp.z);
			}
		}
		else
		{
			animator.SetFloat("_x", 0);
			animator.SetFloat("_y", 0);
		}
		//gravity
		if (Physics.OverlapSphere(transform.position - new Vector3(0, controller.height / 2, 0), .1f ,groundMask).Length > 0)
		{
		Debug.Log("ping");
			if (gravPull > 0)
			{
				gravPull = 0;
			}
		}
		else
		{
			gravPull += 10 * Time.deltaTime;
		}
		controller.Move(_move * speed + Vector3.down * gravPull);
	}
	#endregion
}

