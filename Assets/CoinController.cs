using UnityEngine;

public class CoinController : MonoBehaviour
{
    //回転速度
    [SerializeField]
    private float rotateSpeed = 3.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //回転を開始する角度を設定
        this.transform.Rotate(0.0f,Random.Range(0.0f,360.0f),0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        //回転
        this.transform.Rotate(0.0f,rotateSpeed,0.0f);
    }
}
