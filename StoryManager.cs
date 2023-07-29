using System.Collections.Generic;
using UnityEngine;

public class StoryManager
{
    private StoryBlock currentStoryBlock;
    private List<StoryBlock> storyHistory;
    private string currentStoryText;

    public StoryManager(StoryBlock initialStoryBlock)
    {
        currentStoryBlock = initialStoryBlock;
        storyHistory = new List<StoryBlock>();
        currentStoryText = initialStoryBlock.GetStoryText();
    }

    public string GetCurrentStoryText()
    {
        return currentStoryText;
    }

    public void SetCurrentStoryText(string text)
    {
        currentStoryText = text;
    }

    public int GetNextStoryBlockCount()
    {
        return currentStoryBlock.GetNextStoryBlock().Length;
    }

    public void MoveToNextStoryBlock(int index)
    {
        StoryBlock[] nextBlocks = currentStoryBlock.GetNextStoryBlock();

        if (index < 0 || index >= nextBlocks.Length)
        {
            Debug.LogWarning("Invalid story block index.");
            return;
        }

        storyHistory.Add(currentStoryBlock);
        currentStoryBlock = nextBlocks[index];
        currentStoryText = currentStoryBlock.GetStoryText();
    }
}
