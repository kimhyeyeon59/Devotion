using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public string speaker;  // 비어있으면 플레이어 대사
    [TextArea(2, 5)]
    public string content;
    public Sprite portrait;
}
