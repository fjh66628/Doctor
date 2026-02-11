using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "DialogueData", menuName = "Game Data/Dialogue")]
public class DialogueSQ : ScriptableObject
{
    [SerializeField]List<string> strings;
    public List<string> getDialogue =>strings;
}
