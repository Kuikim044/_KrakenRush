using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class CharacterData
{
    public string name;
    public bool characterUnlocked;
    public float actionSpeed;
    public int life;
    public int scoreMultiplier;
    public int price;
}

public class CharacterInfo : MonoBehaviour
{
    public TextAsset jsonFile;
    public CharacterData characterData;

    private void Start()
    {
        if (jsonFile != null)
        {
            string jsonText = jsonFile.text;

            // �ŧ JSON ���ͺ�硵�
            characterData = JsonUtility.FromJson<CharacterData>(jsonText);

            // ��Ҷ֧�����ͺ�硵�
            string name = characterData.name;
            bool characterUnlocked = characterData.characterUnlocked;
            float actionSpeed = characterData.actionSpeed;
            int life = characterData.life;
            int scoreMultiplier = characterData.scoreMultiplier;
            int price = characterData.price;


            // �ʴ��Ť�ҷ����
           

            // �������������ͧ������ͤǺ�������Ф����
        }
        else
        {
            Debug.LogError("Please assign a JSON file in the Inspector.");
        }
    }

    public void SaveCharacterDataToJson(CharacterData data)
    {
        // �ŧ�ͺ�硵� CharacterData �� JSON
        string jsonText = JsonUtility.ToJson(data);

        // �ѹ�֡ JSON ŧ����
        string filePath = Application.dataPath + "/File Data/Main Character.json";
        File.WriteAllText(filePath, jsonText);
    }
}
