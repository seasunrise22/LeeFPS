using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// �̱������� ����.
// ���� ���� ī��Ʈ
public class GameManager : MonoBehaviour
{
    public static GameManager instance; // �̱����� �Ҵ��� ���� ����
    private int cratesNum; // �ʵ忡 �����Ǿ� �ִ� ������ ������ ī��Ʈ�� ����.
    public Text cratesText; // ���� �ʵ忡 �����Ǿ� �ִ� ������ ���� ǥ���� ����.

    // ���� ���۰� ���ÿ� �̱����� ����
    private void Awake()
    {
        // �̱��� ���� instance�� ��� �ִ°�?
        if(instance == null)
        {
            // instance�� ��� �ִٸ�(null) �װ��� �ڱ� �ڽ��� �Ҵ�
            instance = this;
        }
        else
        {
            // instance�� �̹� �ٸ� GameManager ������Ʈ�� �Ҵ�Ǿ� �ִ� ���
            // ���� �� �� �̻��� GameManager ������Ʈ�� �����Ѵٴ� �ǹ�
            // �̱��� ������Ʈ�� �ϳ��� �����ؾ� �ϹǷ� �ڽ��� ���� ������Ʈ�� �ı�.
            Debug.LogWarning("���� �� �� �̻��� ���� �Ŵ����� �����մϴ�!");
            Destroy(gameObject);
        }
    }

    // ���ڰ� �����Ǹ� AddCrates �Լ��� ȣ���Ͽ� UI�� ���� �����Ǿ� �ִ� ���ڵ��� �� ������ ���Ž�ų ����. 
    public void AddCrates()
    {
        cratesNum++;
        cratesText.text = "���� ���� : " + cratesNum;
    }

    // ���ڰ� �ı��Ǹ� ���� ���� ������ -1�ϰ� UI�� ����.
    public void DestroyCrates()
    {
        cratesNum--;
        cratesText.text = "���� ���� : " + cratesNum;
    }
}
