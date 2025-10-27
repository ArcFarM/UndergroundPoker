using UnityEngine;

namespace SampleTest
{
    public class HangingLampTest : MonoBehaviour
    {
        #region Variables
        public Rigidbody rb;
        public float force = 3f;
        #endregion


        #region Unity Event Method
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddForce(new Vector3(force, 0, 0), ForceMode.Impulse);
            }
        }
        #endregion
    }

}
