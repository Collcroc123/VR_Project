//======= Copyright (c) Valve Corporation, All rights reserved. ===============
//
// Purpose: Collider dangling from the player's head
//
//=============================================================================

using UnityEngine;
using System.Collections;

namespace Valve.VR.InteractionSystem
{
	//-------------------------------------------------------------------------
	[RequireComponent( typeof( CapsuleCollider ) )]
	public class BodyCollider : MonoBehaviour
	{
		public Transform head;
		public float yHead;
		private CapsuleCollider capsuleCollider;

		//-------------------------------------------------
		void Awake()
		{
			capsuleCollider = GetComponent<CapsuleCollider>();
		}


		//-------------------------------------------------
		void FixedUpdate()
		{
			yHead = head.transform.localEulerAngles.y;
			float distanceFromFloor = Vector3.Dot( head.localPosition, Vector3.up );
			capsuleCollider.height = Mathf.Max( capsuleCollider.radius, distanceFromFloor );
			transform.localPosition = head.localPosition - 0.5f * distanceFromFloor * Vector3.up;
			transform.rotation = Quaternion.Euler(new Vector3(0, yHead, 0));
			//transform.localRotation = new Quaternion(0,head.transform.localRotation.y,0,0);
		}
	}
}
