using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum Rarity
{
    SS,
    S,
    A,
    C,
    N
}
public enum RewardItem
{
    ScoreBooster,
    Skipper,
    None
}
public enum RewardGacha
{
    Coin,
    McToken,
    Star,
    None
}

[System.Serializable]
public class GachaItem
{
    public string Name;
    public int itemID;
    public int amount;
    public Sprite itemImage;
    public Rarity rarity;
    

    public RewardItem rewardItem;
    public RewardGacha reward;
    public float probability;
    public float probabilityClass;

    public void SetProbability(float totalProbability)
    {
        if (totalProbability == 0f)
        {
            probability = 0f;
        }
        else
        {
            probability = probabilityClass / totalProbability;
        }
    }

}

public class GachaSystem : MonoBehaviour
{
    public List<GachaItem> gachaItems; // รายการไอเท็มใน Gacha
    public List<GachaItem> inventory; // คลังไอเท็ม

    public GachaScriptable itemList;

    public Image imageItem;
    public TextMeshProUGUI nameItem;
    public GameObject openDisPlayPanel;

    private void Start()
    {
        InitializeGachaProbabilities();
        gachaItems = new List<GachaItem>(itemList.itemData);
    }

    private void InitializeGachaProbabilities()
    {
        Dictionary<Rarity, float> totalProbabilityByRarity = new Dictionary<Rarity, float>();

        // คำนวณโอกาสรวมของแต่ละคลาส
        foreach (GachaItem item in gachaItems)
        {
            if (!totalProbabilityByRarity.ContainsKey(item.rarity))
            {
                totalProbabilityByRarity[item.rarity] = 0f;
            }
            totalProbabilityByRarity[item.rarity] += item.probabilityClass;
        }

        // แบ่งความน่าจะเป็นของแต่ละไอเท็ม
        foreach (GachaItem item in gachaItems)
        {
            float totalProbability = totalProbabilityByRarity[item.rarity];
            item.SetProbability(totalProbability);
        }
    }

    public void PerformGacha()
    {
        float randomValue = Random.Range(0f, 100f);

        // ตรวจสอบความน่าจะเป็นของแต่ละไอเท็ม
        float cumulativeProbability = 0f;

        foreach (GachaItem item in gachaItems)
        {
            cumulativeProbability += item.probability;
            if (randomValue <= cumulativeProbability)
            {
                Debug.Log("You got " + item.Name + " (Rarity: " + item.rarity + ")");
                inventory.Add(item);
                DisplayInventory(item.itemID);
                GiveReward(item.itemID);
                break;
            }
        }
        Debug.Log(randomValue);
    }

    public void DisplayInventory(int ItemId)
    {
        openDisPlayPanel.SetActive(true); 
        // แสดงรายการไอเท็มในคลัง
        foreach (GachaItem item in gachaItems)
        {

            if (item.itemID == ItemId)
            {
                if (item.reward == RewardGacha.Coin)
                {
                    nameItem.text = item.Name;
                    imageItem.sprite = item.itemImage;
                }
                    
                if (item.reward == RewardGacha.McToken)
                {
                    nameItem.text = item.Name;
                    imageItem.sprite = item.itemImage;
                }

                if (item.reward == RewardGacha.Star)
                {
                    nameItem.text = item.Name;
                    imageItem.sprite = item.itemImage;
                }

                if (item.rewardItem == RewardItem.Skipper)
                {
                    nameItem.text = item.Name;
                    imageItem.sprite = item.itemImage;
                }
                if (item.rewardItem == RewardItem.ScoreBooster)
                {
                    nameItem.text = item.Name;
                    imageItem.sprite = item.itemImage;
                }

                break;
            }
        }
    }
    public void TapToCloseDisplayItem()
    {
        openDisPlayPanel.SetActive(false);
    }
    private void GiveReward(int ItemId)
    {
        foreach (GachaItem item in gachaItems)
        {
            if (item.itemID == ItemId)
            {
                if(item.reward == RewardGacha.Coin)
                    Player.player.playerData.coin += item.amount;
                Player.player.playerData.coin = Player.player.playerData.coin;
                if (item.reward == RewardGacha.McToken)
                    Debug.Log("U got McToken" + item.amount);
                if (item.reward == RewardGacha.Star)
                    Player.player.playerData.star += item.amount;
                Player.player.playerData.star = Player.player.playerData.star;

                if (item.rewardItem == RewardItem.Skipper)
                    Debug.Log("U got Skipper" + item.amount);
                if (item.rewardItem == RewardItem.ScoreBooster)
                    Debug.Log("ScoreBooster" + item.amount);

                break;
            }
        }
    }
}