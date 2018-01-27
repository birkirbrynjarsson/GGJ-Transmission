﻿using UnityEngine;
using System.Collections;
using RootMotion.Demos;

namespace RootMotion {

	/// <summary>
	/// 2 player, 3rd person camera controller.
	/// </summary>
	public class Camera2PController : MonoBehaviour {

		// When to update the camera?
		[System.Serializable]
		public enum UpdateMode {
			Update,
			FixedUpdate,
			LateUpdate,
			FixedLateUpdate
		}

		public Transform target; // The target Transform to follow
		public Transform target2; // If 2 players, then this is the target Transform to use for calculating the midpoint to follow with target
		public Transform rotationSpace; // If assigned, will use this Transform's rotation as the rotation space instead of the world space. Useful with spherical planets.
		public UpdateMode updateMode = UpdateMode.LateUpdate; // When to update the camera?
		public bool lockCursor = true; // If true, the mouse will be locked to screen center and hidden

		[Header("Position")]
		public bool smoothFollow; // If > 0, camera will smoothly interpolate towards the target
		public Vector3 offset = new Vector3(0, 1.5f, 0.5f); // The offset from target relative to camera rotation
		public float followSpeed = 10f; // Smooth follow speed

		[Header("Rotation")]
		public float rotationSensitivity = 3.5f; // The sensitivity of rotation
		public float yMinLimit = -20; // Min vertical angle
		public float yMaxLimit = 80; // Max vertical angle
		public bool rotateAlways = true; // Always rotate to mouse?
		public bool rotateOnLeftButton; // Rotate to mouse when left button is pressed?
		public bool rotateOnRightButton; // Rotate to mouse when right button is pressed?
		public bool rotateOnMiddleButton; // Rotate to mouse when middle button is pressed?

		[Header("Distance")]
		public float distance = 10.0f; // The current distance to target
		public float minDistance = 4; // The minimum distance to target
		public float maxDistance = 30; // The maximum distance to target
		public float zoomSpeed = 10f; // The speed of interpolating the distance
		public float zoomSensitivity = 1f; // The sensitivity of mouse zoom

		[Header("Blocking")]
		public LayerMask blockingLayers;
		public float blockingRadius = 1f;
		public float blockingSmoothTime = 0.1f;
		[Range(0f, 1f)] public float blockedOffset = 0.5f;

		public float x { get; private set; } // The current x rotation of the camera
		public float y { get; private set; } // The current y rotation of the camera
		public float distanceTarget { get; private set; } // Get/set distance

		private Vector3 targetDistance, position;
		private Quaternion rotation = Quaternion.identity;
		private Vector3 smoothPosition;
		private Camera cam;
		private bool fixedFrame;
		private float fixedDeltaTime;
		private Quaternion r = Quaternion.identity;
		private Vector3 lastUp;
		private float blockedDistance = 10f, blockedDistanceV;

		// Initiate, set the params to the current transformation of the camera relative to the target
		protected virtual void Awake () {
			Vector3 angles = transform.eulerAngles;
			x = angles.y;
			y = angles.x;

			distanceTarget = distance;
			smoothPosition = transform.position;

			cam = GetComponent<Camera>();

			lastUp = rotationSpace != null? rotationSpace.up: Vector3.up;
		}

		protected virtual void Update() {
			if (updateMode == UpdateMode.Update) UpdateTransform();
		}

		protected virtual void FixedUpdate() {
			fixedFrame = true;
			fixedDeltaTime += Time.deltaTime;
			if (updateMode == UpdateMode.FixedUpdate) UpdateTransform();
		}

		protected virtual void LateUpdate() {
			UpdateInput();

			if (updateMode == UpdateMode.LateUpdate) UpdateTransform();

			if (updateMode == UpdateMode.FixedLateUpdate && fixedFrame) {
				UpdateTransform(fixedDeltaTime);
				fixedDeltaTime = 0f;
				fixedFrame = false;
			}
		}

		// Read the user input
		public void UpdateInput() {
			if (!cam.enabled) return;

			// Cursors
			Cursor.lockState = lockCursor? CursorLockMode.Locked: CursorLockMode.None;
			Cursor.visible = lockCursor? false: true;

			// Should we rotate the camera?
			bool rotate = rotateAlways || (rotateOnLeftButton && Input.GetMouseButton(0)) || (rotateOnRightButton && Input.GetMouseButton(1)) || (rotateOnMiddleButton && Input.GetMouseButton(2));

			// delta rotation
			if (rotate) {
				x += UserControlThirdPerson.player.GetAxis("Look Horizontal") * rotationSensitivity;
				y = ClampAngle(y - UserControlThirdPerson.player.GetAxis("Look Vertical") * rotationSensitivity, yMinLimit, yMaxLimit);
			}

			// Distance
			distanceTarget = Mathf.Clamp(distanceTarget + zoomAdd, minDistance, maxDistance);
		}

		// Update the camera transform
		public void UpdateTransform() {
			UpdateTransform(Time.deltaTime);
		}

		public void UpdateTransform(float deltaTime) {
			if (!cam.enabled) return;

			// Rotation
			rotation = Quaternion.AngleAxis(x, Vector3.up) * Quaternion.AngleAxis(y, Vector3.right);

			if (rotationSpace != null) {
				r = Quaternion.FromToRotation(lastUp, rotationSpace.up) * r;
				rotation = r * rotation;

				lastUp = rotationSpace.up;

			}

			if (target != null) {

				Vector3 newTarget = target.position;

				// Calculate new camera target for 2 player control
				if (target2 != null) {
					newTarget = new Vector3 ((target2.position.x + target.position.x) / 2f, (target2.position.y + target.position.y) / 2f, (target2.position.z + target.position.z) / 2f);
				}

				// Distance
				distance += (distanceTarget - distance) * zoomSpeed * deltaTime;

				// Smooth follow
				if (!smoothFollow) smoothPosition = newTarget;
				else smoothPosition = Vector3.Lerp(smoothPosition, newTarget, deltaTime * followSpeed);

				// Position
				Vector3 t = smoothPosition + rotation * offset;
				Vector3 f = rotation * -Vector3.forward;


				float addDistance = 0;

				// Distance between players if 2 players.
				if (target2 != null) {
					float z = Mathf.Abs (target.position.z - target2.position.z);
					float x = Mathf.Abs (target.position.z - target2.position.z);
					print ("z: " + z + " x: " + x);
					addDistance = z < x ? x : z;
					print ("distance: " + addDistance);
				}

				position = t + f * (distance + addDistance);

				// Translating the camera
				transform.position = position;
			}

			transform.rotation = rotation;
		}

		// Zoom input
		private float zoomAdd {
			get {
				float scrollAxis = Input.GetAxis("Mouse ScrollWheel");
				if (scrollAxis > 0) return -zoomSensitivity;
				if (scrollAxis < 0) return zoomSensitivity;
				return 0;
			}
		}

		// Clamping Euler angles
		private float ClampAngle (float angle, float min, float max) {
			if (angle < -360) angle += 360;
			if (angle > 360) angle -= 360;
			return Mathf.Clamp (angle, min, max);
		}

	}
}
