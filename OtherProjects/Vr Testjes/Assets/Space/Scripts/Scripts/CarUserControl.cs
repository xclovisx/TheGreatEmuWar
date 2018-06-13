using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Vehicles.Car
{
    [RequireComponent(typeof (CarController))]
    public class CarUserControl : MonoBehaviour
    {
		private GameObject Player;
		private playerControl playerScrpt;
        private CarController m_Car; // the car controller we want to use

		private void Start(){
			Player = GameObject.FindGameObjectWithTag ("Player");
			playerScrpt = Player.GetComponent<playerControl> ();
		}
        private void Awake()
        {
            // get the car controller
            m_Car = GetComponent<CarController>();
        }


        private void FixedUpdate()
        {
			if (playerScrpt.driving && playerScrpt.curCar == this.gameObject) {

				// pass the input to the car!
				float h = CrossPlatformInputManager.GetAxis ("Horizontal");
				float v = CrossPlatformInputManager.GetAxis ("Vertical");
#if !MOBILE_INPUT
				float handbrake = CrossPlatformInputManager.GetAxis ("Jump");
				m_Car.Move (h, v, v, handbrake);
#else
            m_Car.Move(h, v, v, 0f);
#endif
			} else {
				m_Car.Move (0, 0, 0, 1);
			}
        }
    }
}
