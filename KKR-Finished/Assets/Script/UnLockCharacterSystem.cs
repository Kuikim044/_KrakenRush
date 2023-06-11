using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnLockCharacterSystem : MonoBehaviour
{
    public PlayerData playerData;
    public CharacterInfo characterInfo;
    public void OnCharacterPurchase(int characterIndex)
    {
        if (characterInfo.characterData.characterUnlocked)
            return;

        if (characterInfo.characterData.price > Player.player.coin)
        {
            Debug.Log("not enough money");
            return;
        }


        // ���¡�� UnlockCharacter � PlayerData ���ͻŴ��ͤ����Ф÷��Ѫ�շ���˹�
        playerData.UnlockCharacter(characterIndex);
        playerData.AddCharacter(characterInfo.characterData.name, characterInfo.characterData.characterUnlocked = true);
        playerData.coin -= characterInfo.characterData.price;

        characterInfo.characterData.characterUnlocked = true; // ����¹ʶҹлŴ��͡
        characterInfo.SaveCharacterDataToJson(characterInfo.characterData);


        // ������������ͧ�����ѧ�ҡ�Ŵ��ͤ����Ф� ���ѻവʶҹТͧ����Ф����
    }
}
