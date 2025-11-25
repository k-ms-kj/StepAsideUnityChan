using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    //carPrefabを入れる
    public GameObject carPrefab;
    //coinPrefabを入れる
    public GameObject coinPrefab;
    //conePrefabを入れる
    public GameObject conePrefab;
    //スタート地点
    private int startPos = 80;
    //ゴール地点
    private int goalPos = 360;
    //アイテムを出すx方向の範囲
    private float posRange = 3.4f;
    //アイテムを生成する距離
    [SerializeField]
    private float itemDistance = 40.0f;
    //Unityちゃん
    [SerializeField]
    private GameObject unitychan;
    //Unityちゃんの初期Z座標
    private float unitychanPosZ = 0.0f;
    //アイテム生成のフラグ
    [SerializeField]
    private bool isItem = false;
    //アイテム格納用のリスト
    [SerializeField]
    private List<GameObject> itemList = new List<GameObject>();
    //移動距離計算用の変数
    private float moveDistance = 0.0f;
    private float differenceDistance = 0.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Unityちゃん初期位置を取得
        this.unitychanPosZ = this.unitychan.transform.position.z;
    }

    //Update is called once per frame
    void Update()
    {
        ItemGenerateFromUnitychan();
    }


    //Unityちゃんから一定の距離でアイテムを生成
    /// <summary>
    /// Unityちゃんから40m離れた地点のアイテムを生成
    /// アイテムはZ＝80の地点からZ=320の間に生成
    /// </summary>
    private void ItemGenerateFromUnitychan()
    {
        //毎フレームごとの移動量を計算
        differenceDistance += unitychan.transform.position.z - moveDistance;
        moveDistance = unitychan.transform.position.z;

        if (differenceDistance > 15.0f)//15m進むごとにアイテムフラグTrue
        {
            isItem = true;
            differenceDistance = 0.0f;
        }

        if (startPos - 40 <= UnitychanDistance() && UnitychanDistance() <= goalPos - 40)
        {
            if (isItem)
            {
                //どのアイテムを出すかをランダムに設定
                int num = Random.Range(1, 11);
                if (num <= 2)
                {
                    //コーンをx軸方向に一直線に生成
                    for (float j = -1.0f; j <= 1.0f; j += 0.4f)
                    {
                        GameObject cone = Instantiate(conePrefab);
                        cone.transform.position = new Vector3(4 * j, cone.transform.position.y, UnitychanDistance() + 40);
                        itemList.Add(cone);
                    }
                }
                else
                {
                    //レーンごとにアイテムを生成
                    for (int j = -1; j <= 1; j++)
                    {
                        //アイテムの種類を決める
                        int item = Random.Range(1, 11);
                        //アイテムを置くz座標のオフセットをランダムに設定
                        int offsetZ = Random.Range(-5, 6);
                        //60%コイン配置：30%車配置：10%何もなし
                        if (1 <= item && item <= 6)
                        {
                            //コインを生成
                            GameObject coin = Instantiate(coinPrefab);
                            coin.transform.position = new Vector3(posRange * j, coin.transform.position.y, UnitychanDistance() + 40 + offsetZ);
                            itemList.Add(coin);
                        }
                        else if (7 <= item && item <= 9)
                        {
                            //車を生成
                            GameObject car = Instantiate(carPrefab);
                            car.transform.position = new Vector3(posRange * j, car.transform.position.y, UnitychanDistance() + 40 + offsetZ);
                            itemList.Add(car);
                        }
                    }
                }

                isItem = false;
            }
            //通り過ぎたアイテムを削除
            DeleteItem();
        }
    }
    
    //Unityちゃんが通り過ぎたアイテムを削除する
    private void DeleteItem()
    {
        if (itemList != null)
        {
            List<GameObject> deleteList = new List<GameObject>();//下記エラー回避用にデリート用リスト生成
            foreach (GameObject item in itemList)//この中でitemList.Removeをするとエラーが出たInvalidOperationException: Collection was modified; enumeration operation may not execute. 
            {
                if (item.transform.position.z < unitychan.transform.position.z - 10.0f)
                {
                    deleteList.Add(item);
                }
            }
            foreach (GameObject deleteItem in deleteList)
            {
                itemList.Remove(deleteItem);
                Destroy(deleteItem);
            }
        }
    }

    //Unityちゃんの進んだ距離を計算
    private int UnitychanDistance()
    {
        int distance = 0;
        float posZ;
        posZ = this.unitychan.transform.position.z - this.unitychanPosZ;
        distance = (int)posZ;
        return distance;
    }
}
