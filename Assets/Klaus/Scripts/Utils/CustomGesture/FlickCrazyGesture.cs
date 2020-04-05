/*
 * @author Luis Vieira / http://va.lent.in/
 */

using System;
using System.Collections.Generic;
using TouchScript.Utils;
using UnityEngine;

namespace TouchScript.Gestures
{
	/// <summary>
	/// Recognizes fast movement before releasing touches. Doesn't care how much time touch points were on surface and how much they moved.
	/// </summary>
	public class FlickCrazyGesture : Gesture
	{
		#region Constants
		protected float magicConstantForBlock = 100.0f;
		/// <summary>
		/// Message name when gesture is recognized
		/// </summary>
		public const string FLICK_MESSAGE = "OnFlick";
		
		/// <summary>
		/// Direction of a flick.
		/// </summary>
		public enum GestureDirection
		{
			/// <summary>
			/// Direction doesn't matter.
			/// </summary>
			Any,
			
			/// <summary>
			/// Only horizontal.
			/// </summary>
			Horizontal,
			
			/// <summary>
			/// Only vertical.
			/// </summary>
			Vertical,
		}
		
		#endregion
		
		#region Events
		
		/// <summary>
		/// Occurs when gesture is recognized.
		/// </summary>
		public event EventHandler<EventArgs> Flicked
		{
			add { flickedInvoker += value; }
			remove { flickedInvoker -= value; }
		}
		
		// iOS Events AOT hack
		private EventHandler<EventArgs> flickedInvoker;
		
		#endregion
		
		#region Public properties
		

		
		/// <summary>
		/// Gets or sets minimum distance in cm to move in <see cref="FlickTime"/> before ending gesture for it to be recognized.
		/// </summary>
		/// <value>Minimum distance in cm to move in <see cref="FlickTime"/> before ending gesture for it to be recognized.</value>
		public float MinDistance
		{
			get { return minDistance; }
			set { minDistance = value; }
		}
		
		/// <summary>
		/// Gets or sets minimum distance in cm touches must move to start recognizing this gesture.
		/// </summary>
		/// <value>Minimum distance in cm touches must move to start recognizing this gesture.</value>
		/// <remarks>Prevents misinterpreting taps.</remarks>
		public float MovementThreshold
		{
			get { return movementThreshold; }
			set { movementThreshold = value; }
		}
		
		/// <summary>
		/// Gets or sets direction to look for.
		/// </summary>
		/// <value>Direction of movement.</value>
		public GestureDirection Direction
		{
			get { return direction; }
			set { direction = value; }
		}
		
		/// <summary>
		/// Gets flick direction (not normalized) when gesture is recognized.
		/// </summary>
		public Vector2 ScreenFlickVector { get; private set; }

		
		#endregion
		
		#region Private variables
		
		[SerializeField]
		private float minDistance = 1f;
		
		[SerializeField]
		private float movementThreshold = .5f;
		
		[SerializeField]
		private GestureDirection direction = GestureDirection.Any;
		
		private bool moving = false;
		private Vector2 movementBuffer = Vector2.zero;
		private bool isActive = false;
		private TimedSequenceCrazy<Vector2> deltaSequence = new TimedSequenceCrazy<Vector2>();
		
		#endregion
		
		#region Unity methods

		/// <inheritdoc />
		protected void LateUpdate()
		{
			if (!isActive) return;
			
			deltaSequence.Add(ScreenPosition - PreviousScreenPosition);

			//TEst
			var deltas = deltaSequence.FindAllElements();
			var totalMovement = Vector2.zero;
			foreach (var delta in deltas) totalMovement += delta;
			
			switch (Direction)
			{
			case GestureDirection.Horizontal:
				totalMovement.y = 0;
				break;
			case GestureDirection.Vertical:
				totalMovement.x = 0;
				break;
			}
			
			if (totalMovement.magnitude < MinDistance * touchManager.DotsPerCentimeter)
			{
				//Aqui mando mensaje q muestre pausa
				SendMessage("Pause",SendMessageOptions.DontRequireReceiver);
			} else
			{
				//Aqui mando mensaje para saber si es izquierda o derecha

				totalMovement.Normalize();
				if(totalMovement.x > 0 || totalMovement.y > 0){
					SendMessage("Right",SendMessageOptions.DontRequireReceiver);

				}else if(totalMovement.x < 0 || totalMovement.y < 0){
					SendMessage("Left",SendMessageOptions.DontRequireReceiver);

				}

			}


		}
		
		#endregion
		
		#region Gesture callbacks
		
		/// <inheritdoc />
		protected override void touchesBegan(IList<TouchPoint> touches)
		{
			base.touchesBegan(touches);
			
			if (activeTouches.Count == touches.Count)
			{
				isActive = true;
			}
		}
		
		/// <inheritdoc />
		protected override void touchesMoved(IList<TouchPoint> touches)
		{
			base.touchesMoved(touches);
			
			if (!moving)
			{
				movementBuffer += ScreenPosition - PreviousScreenPosition;
				var dpiMovementThreshold = MovementThreshold*touchManager.DotsPerCentimeter;
				if (movementBuffer.sqrMagnitude >= dpiMovementThreshold*dpiMovementThreshold)
				{
					moving = true;
				}
			}



		}
		
		/// <inheritdoc />
		protected override void touchesEnded(IList<TouchPoint> touches)
		{
			base.touchesEnded(touches);
			
			if (activeTouches.Count == 0)
			{
				isActive = false;
				
				if (!moving)
				{
					setState(GestureState.Failed);
					return;
				}
				
				deltaSequence.Add(ScreenPosition - PreviousScreenPosition);
				
				var deltas = deltaSequence.FindAllElements();
				var totalMovement = Vector2.zero;
				foreach (var delta in deltas) totalMovement += delta;
				
				switch (Direction)
				{
				case GestureDirection.Horizontal:
					if(totalMovement.y >= magicConstantForBlock || totalMovement.y  <=  -magicConstantForBlock){
						setState(GestureState.Failed);
						return;
					}
					totalMovement.y = 0;
					break;
				case GestureDirection.Vertical:
					if(totalMovement.x >= magicConstantForBlock || totalMovement.x  <=  -magicConstantForBlock){
						setState(GestureState.Failed);
						return;
					}
					totalMovement.x = 0;
					break;
				}
				
				if (totalMovement.magnitude < MinDistance * touchManager.DotsPerCentimeter)
				{
					setState(GestureState.Failed);

				} else
				{
					ScreenFlickVector = totalMovement;
					setState(GestureState.Recognized);
				}
			}
		}
		
		/// <inheritdoc />
		protected override void touchesCancelled(IList<TouchPoint> touches)
		{
			base.touchesCancelled(touches);
			touchesEnded(touches);
		}
		
		/// <inheritdoc />
		protected override void onRecognized()
		{
			base.onRecognized();
			if (flickedInvoker != null) flickedInvoker(this, EventArgs.Empty);
			if (UseSendMessage) SendMessageTarget.SendMessage(FLICK_MESSAGE, this, SendMessageOptions.DontRequireReceiver);
		}
		
		/// <inheritdoc />
		protected override void reset()
		{
			base.reset();
			deltaSequence.Clear();
			isActive = false;
			moving = false;
			movementBuffer = Vector2.zero;
		}
		
		#endregion
	}
}