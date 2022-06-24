using UnityEngine;
using System.Collections;

namespace AnimFollow
{
	public class PlayerMovement_AF : MonoBehaviour
	{
		// Add this script to the master

		public readonly int version = 7; // The version of this script

		Animator anim;			// Reference to the animator component.
		HashIDs_AF hash;			// Reference to the HashIDs.

		public float animatorSpeed = 1.3f; // Read by RagdollControl
		public float speedDampTime = .1f;	// The damping for the speed parameter
		public float speed = 0f;
		float mouseInput;
		public float mouseSensitivityX = 100f;
		public bool inhibitMove = false; // Set from RagdollControl
		[HideInInspector] public Vector3 glideFree = Vector3.zero; // Set from RagdollControl
		Vector3 glideFree2 = Vector3.zero;
		[HideInInspector] public bool inhibitRun = false; // Set from RagdollControl

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		void Awake ()
		{
			// Setting up the references.
			if (!(anim = GetComponent<Animator>()))
			{
				Debug.LogWarning("Missing Animator on " + this.name);
				inhibitMove = true;
			}
			if (!(hash = GetComponent<HashIDs_AF>()))
			{
				Debug.LogWarning("Missing Script: HashIDs on " + this.name);
				inhibitMove = true;
			}
			if (anim.avatar)
				if (!anim.avatar.isValid)
					Debug.LogWarning("Animator avatar is not valid");
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		
		void OnAnimatorMove ()
		{
			glideFree2 = Vector3.Lerp (glideFree2, glideFree, .05f);
			transform.position += anim.deltaPosition + glideFree2;
		}
		
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		
		void FixedUpdate ()
		{
			if (inhibitMove)
				return;

			//transform.Rotate(0f, Input.GetAxis("Mouse X") * mouseSensitivityX * Time.fixedDeltaTime, 0f);

			MovementManagement(speed);
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public void MovementManagement (float vertical)
		{
			anim.SetFloat(hash.speedFloat, vertical, speedDampTime, Time.fixedDeltaTime);
		}
	}
}
