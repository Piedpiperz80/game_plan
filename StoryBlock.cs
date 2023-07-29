using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Story Block")]
public class StoryBlock : ScriptableObject
{
    [TextArea(10, 10)] [SerializeField] string storyText;
    [SerializeField] StoryBlock[] nextStoryBlock;

    public string GetStoryText()
    {
        return storyText;
    }

    public StoryBlock[] GetNextStoryBlock()
    {
        return nextStoryBlock;
    }
}
