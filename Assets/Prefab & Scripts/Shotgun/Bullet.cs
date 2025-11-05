using UnityEngine;


namespace UnderGroundPoker.Objects
{
    public class Bullet : MonoBehaviour {
        //총알 속성 : 피해량 (특수 카드 사용 시 증가)
        public bool isStrong = false;
        //총알 속성 : 공포탄 여부
        public bool isFalse = false;

        //TODO : 공포탄 여부에 따라 다르게 적용할 효과나 색상 마테리얼 변수들
        
        public Material tBulletMaterial;
        public Material fBulletMaterial;
        public ParticleSystem tBulletEffect;
        public ParticleSystem fBulletEffect;

        public Material originalMaterial;
        Material currMaterial;
        public ParticleSystem originalParticle;
        ParticleSystem currParticle;

        /*private void Start()
        {
            currMaterial = this.gameObject.GetComponent<Renderer>().material;
            originalMaterial = currMaterial;
            currParticle = this.gameObject.GetComponentInChildren<ParticleSystem>();
            originalParticle = currParticle;
        }

        public void SetBullet()
        {
            if (!isFalse)
            {
                currMaterial = tBulletMaterial;
                this.gameObject.GetComponent<Renderer>().material = currMaterial;
                currParticle = tBulletEffect;
            }
        }*/
        
    }

}
