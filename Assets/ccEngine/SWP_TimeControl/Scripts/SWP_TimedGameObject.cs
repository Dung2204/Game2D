// --------------ABOUT AND COPYRIGHT----------------------
//  Copyright © 2013 SketchWork Productions Limited
//        support@sketchworkproductions.com
// -------------------------------------------------------

using UnityEngine;
using System.Collections;

/// <summary>
/// This is the second most important script that is used with the Time Controller. Add this script to the objects/prefabs that you want the time control to effect. This has several modes of use:
/// 1) If a TimeController is linked then the speed will automatically be set to the speed of the Time Controller. This is useful for GameObjects that maybe instantiated during the game.
/// 2) If you choose to “Search Objects” the script will look for any other common speed related components on the same GameObject automatically. In this case the “Assigned Objects” array is ignored.
/// 3) If you choose to not “Search Objects” the script will use the “Assigned Objects” array to check what you want to time control. This is also useful for nested things you want controlled, but don’t want this script on every single child GameObject.
/// </summary>
public class SWP_TimedGameObject: SWP_InternalTimedGameObject
{
	/// <summary>
	///If this script is attached to a GameObject with a Mecanim Animator component when false it will use the global Animator speed which effects the entire animation. If set to true it will allow you to create a custom script where the Time Controller will trigger events for you to do any custom blending giving you more advanced control.
	/// </summary>
#if UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5_0
	[SerializeField] public bool UseEventsForMecanim = false;
#endif

	/// <summary>
	///Cached legacy animation component.  (Animation)
	/// </summary>
	Animation aniLegacy;

	/// <summary>
	///Cached mecanim animation component.  (Animator)
	/// </summary>
#if UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5_0
	Animator aniMecanim;
#endif

	/// <summary>
	///Cached Shuriken particle system.  (ParticleSystem)
	/// </summary>
#if UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5_0
	ParticleSystem psShuriken;
#endif

#if UNITY_2018_4_OR_NEWER
	/// <summary>
	///Cached Legacy particle system.  (ParticleEmitter)
	/// </summary>
	ParticleSystem psLegacy;
#else
	/// <summary>
	///Cached Legacy particle system.  (ParticleEmitter)
	/// </summary>
	ParticleEmitter psLegacy;
#endif

	/// <summary>
	///Cached RigidBody.  This is limited in this script and is best suited to custom events.
	/// </summary>
	Rigidbody rbRigidbody;
#if UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5_0
	Rigidbody2D rbRigidbody2D;
#endif
	
	/// <summary>
	///Cached Audio Source.
	/// </summary>
	AudioSource asAudio;

	/// <summary>
	///Last Vector3 saved speed.
	/// </summary>
	Vector3 vecSavedSpeed;
#if UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5_0
	Vector2 vecSavedSpeed2D;
#endif

	/// <summary>
	///Last Vector3 saved Spin.
	/// </summary>
	Vector3 vecSavedSpin;
#if UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5_0
	float vecSavedSpin2D;
#endif

	/// <summary>
	///Awake function is used to cache the components if using SearchObjects so it is only called once.
	/// </summary>
	void Awake()
	{
		if (SearchObjects)
		{
            aniLegacy = this.GetComponent<Animation>();  //gameObject.animation;

#if UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5_0
			if (!UseEventsForMecanim)
				aniMecanim = GetComponent<Animator>();
#endif

#if UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5_0
			psShuriken = gameObject.particleSystem;
            if (psShuriken == null)
            {
                for (int i =0; i < transform.GetChildCount();i++)
                {
                    psShuriken = transform.GetChild(i).gameObject.particleSystem;
                    if (psShuriken != null)
                    {
                        break;
                    }
                }                
            }
#elif UNITY_3_5
			psShuriken = GetComponent<ParticleSystem>();
#endif
#if UNITY_2018_4_OR_NEWER
			psLegacy = this.GetComponent<ParticleSystem>(); //gameObject.particleEmitter;
#else
	psLegacy = this.GetComponent<ParticleEmitter>(); //gameObject.particleEmitter;
#endif

			rbRigidbody = this.GetComponent<Rigidbody>(); //gameObject.rigidbody;
#if UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5_0
			rbRigidbody2D = gameObject.rigidbody2D;
#endif

			asAudio = GetComponent<AudioSource>();
		}
	}

	/// <summary>
	///When not using SeachObjects the caches need to be cleaned.
	/// </summary>
	protected override void ClearAssignedObjects()
	{
		aniLegacy = null;
#if UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5_0
		aniMecanim = null;
#endif
#if UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5_0
		psShuriken = null;
#endif
		psLegacy = null;
		rbRigidbody = null;
		
#if UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5_0
		rbRigidbody2D = null;
#endif
		asAudio = null;
	}

	/// <summary>
	///When not using SeachObject this function loops through the object array and sets the speed for the ohjects.
	/// </summary>
	protected override void SetSpeedLooping(float _fNewSpeed, float _fCurrentSpeedPercent, float _fCurrentSpeedZeroToOne)
	{
		for (int thisCounter = 0; thisCounter < AssignedObjects.Length; thisCounter++)
		{
			if (AssignedObjects[thisCounter].GetType() == typeof(Animation))
				aniLegacy = (Animation)AssignedObjects[thisCounter];
#if UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5_0
			else if (AssignedObjects[thisCounter].GetType() == typeof(Animator))
				aniMecanim = (Animator)AssignedObjects[thisCounter];
#endif
#if UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5_0
			else if (AssignedObjects[thisCounter].GetType() == typeof(ParticleSystem))
				psShuriken = (ParticleSystem)AssignedObjects[thisCounter];
#endif
#if UNITY_2018_4_OR_NEWER
			else if (AssignedObjects[thisCounter].GetType() == typeof(ParticleSystem))
				psLegacy = (ParticleSystem)AssignedObjects[thisCounter];
#else
	else if (AssignedObjects[thisCounter].GetType() == typeof(ParticleEmitter))
				psLegacy = (ParticleEmitter)AssignedObjects[thisCounter];
#endif
			else if (AssignedObjects[thisCounter].GetType() == typeof(Rigidbody))
				rbRigidbody = (Rigidbody)AssignedObjects[thisCounter];
#if UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5_0
			else if (AssignedObjects[thisCounter].GetType() == typeof(Rigidbody2D))
				rbRigidbody2D = (Rigidbody2D)AssignedObjects[thisCounter];
#endif
			else if (AssignedObjects[thisCounter].GetType() == typeof(AudioSource))
				asAudio = (AudioSource)AssignedObjects[thisCounter];
			
			SetSpeedAssigned(_fNewSpeed, _fCurrentSpeedPercent, _fCurrentSpeedZeroToOne);
		}
		
		ClearAssignedObjects();
	}

	/// <summary>
	///This function actually sets the new speed of the objects.
	/// </summary>
	protected override void SetSpeedAssigned(float _fNewSpeed, float _fCurrentSpeedPercent, float _fCurrentSpeedZeroToOne)
	{
		if (aniLegacy != null)
			foreach (AnimationState thisAnimationState in aniLegacy)
				aniLegacy[thisAnimationState.name].speed = _fCurrentSpeedZeroToOne;
				
#if UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5_0
		if (aniMecanim != null)
			aniMecanim.speed = _fCurrentSpeedZeroToOne;
#endif
		
#if UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5_0
		if (psShuriken != null)
			psShuriken.playbackSpeed = _fCurrentSpeedZeroToOne;
#endif
		
		if (psLegacy != null)
		{
#if UNITY_2018_4_OR_NEWER
			if (_fNewSpeed == 0f)
				psLegacy.Stop();
			else
			{
				psLegacy.Play();

				ParticleSystem.MainModule psModule = psLegacy.main;
				psModule.startSpeed = new ParticleSystem.MinMaxCurve(GetNewSpeedFromPercentage(psModule.startSpeed.constantMin), GetNewSpeedFromPercentage(psModule.startSpeed.constantMax));

				psModule.startSize = new ParticleSystem.MinMaxCurve(GetNewSpeedFromPercentage(psModule.startSize.constantMin), GetNewSpeedFromPercentage(psModule.startSize.constantMax));
				psModule.startRotation = new ParticleSystem.MinMaxCurve(GetNewSpeedFromPercentage(psModule.startRotation.constantMin), GetNewSpeedFromPercentage(psModule.startRotation.constantMax));
				ParticleSystem.RotationOverLifetimeModule psModuleRotate = psLegacy.rotationOverLifetime;
				psModuleRotate = new ParticleSystem.RotationOverLifetimeModule
				{
					xMultiplier = GetNewSpeedFromPercentage(psModuleRotate.xMultiplier),
					yMultiplier = GetNewSpeedFromPercentage(psModuleRotate.yMultiplier),
					zMultiplier = GetNewSpeedFromPercentage(psModuleRotate.zMultiplier)
				};
				ParticleSystem.Particle[] parParticles = null;
				psLegacy.GetParticles(parParticles);

				for (int thisCount = 0; thisCount < parParticles.Length; thisCount++)
					parParticles[thisCount].velocity = new Vector3(GetNewSpeedFromPercentage(parParticles[thisCount].velocity.x), GetNewSpeedFromPercentage(parParticles[thisCount].velocity.y), GetNewSpeedFromPercentage(parParticles[thisCount].velocity.z));

				psLegacy.SetParticles(parParticles);
			}
#else
	if (_fNewSpeed == 0f)
				psLegacy.enabled = false;
			else
			{
				psLegacy.enabled = true;
								
				psLegacy.worldVelocity = new Vector3(GetNewSpeedFromPercentage(psLegacy.worldVelocity.x), GetNewSpeedFromPercentage(psLegacy.worldVelocity.y), GetNewSpeedFromPercentage(psLegacy.worldVelocity.z));
				psLegacy.localVelocity = new Vector3(GetNewSpeedFromPercentage(psLegacy.localVelocity.x), GetNewSpeedFromPercentage(psLegacy.localVelocity.y), GetNewSpeedFromPercentage(psLegacy.localVelocity.z));
				psLegacy.rndVelocity = new Vector3(GetNewSpeedFromPercentage(psLegacy.rndVelocity.x), GetNewSpeedFromPercentage(psLegacy.rndVelocity.y), GetNewSpeedFromPercentage(psLegacy.rndVelocity.z));
				psLegacy.emitterVelocityScale = GetNewSpeedFromPercentage(psLegacy.emitterVelocityScale);
				psLegacy.angularVelocity =  GetNewSpeedFromPercentage(psLegacy.angularVelocity);
				psLegacy.rndAngularVelocity =  GetNewSpeedFromPercentage(psLegacy.rndAngularVelocity);
				
				Particle[] parParticles =  psLegacy.particles;
				
				for (int thisCount = 0; thisCount < parParticles.Length; thisCount++)
					parParticles[thisCount].velocity = new Vector3(GetNewSpeedFromPercentage(parParticles[thisCount].velocity.x), GetNewSpeedFromPercentage(parParticles[thisCount].velocity.y), GetNewSpeedFromPercentage(parParticles[thisCount].velocity.z));
				
				psLegacy.particles = parParticles;
			}
#endif
		}

		if (rbRigidbody != null)
		{
			if (_fNewSpeed == 0f)
			{
				vecSavedSpeed = rbRigidbody.velocity;
				vecSavedSpin = rbRigidbody.angularVelocity;
				rbRigidbody.isKinematic = true;
			}
			else if (_fNewSpeed != 100f)
			{
				rbRigidbody.isKinematic = false;
				rbRigidbody.velocity = vecSavedSpeed;
				rbRigidbody.angularVelocity = vecSavedSpin;
			}
			else
			{
				rbRigidbody.isKinematic = false;
				rbRigidbody.velocity = vecSavedSpeed;
				rbRigidbody.angularVelocity = vecSavedSpin;
			}
		}
		
#if UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5_0
		if (rbRigidbody2D != null)
		{
			if (_fNewSpeed == 0f)
			{
				vecSavedSpeed2D = rbRigidbody2D.velocity;
				vecSavedSpin2D = rbRigidbody2D.angularVelocity;
				rbRigidbody2D.isKinematic = true;
			}
			else if (_fNewSpeed != 100f)
			{
				rbRigidbody2D.isKinematic = false;
				rbRigidbody2D.velocity = vecSavedSpeed2D;
				rbRigidbody2D.angularVelocity = vecSavedSpin2D;
			}
			else
			{
				rbRigidbody2D.isKinematic = false;
				rbRigidbody2D.velocity = vecSavedSpeed2D;
				rbRigidbody2D.angularVelocity = vecSavedSpin2D;
			}
		}
#endif
		
		if (asAudio != null)
			asAudio.pitch = Mathf.Clamp(_fCurrentSpeedZeroToOne, 0.15f, 2f); //Clamp this value because a speed of 0 or greater than 2 makes no sense.
	}
}