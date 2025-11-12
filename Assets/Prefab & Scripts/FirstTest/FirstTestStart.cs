using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

//테스트용 라운드 시작
public class FirstTestStart : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(this.gameObject.TryGetComponent<Button>(out Button b))
        {
            b.onClick.AddListener(() =>
            {
                UnderGroundPoker.Manager.GameManager.Instance.RoundManager.RoundStart();
                this.gameObject.SetActive(false);
            });
        }
    }
}
