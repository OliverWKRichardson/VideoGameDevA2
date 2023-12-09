using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    // Current NPCDialogue script
    NPCDialogue npcDialogue;

    // Player control script
    FirstPersonAIO player;

    [SerializeField] Inventory groundInventory;

    // Prefabs for all items
    [SerializeField] GameObject basicAmmo;
    [SerializeField] GameObject betterAmmo;
    [SerializeField] GameObject katana;
    [SerializeField] GameObject basicGun;
    [SerializeField] GameObject betterGun;
    [SerializeField] GameObject basicGunPart;
    [SerializeField] GameObject betterGunPart;

    [SerializeField] InventoryController inventoryController;

    // Text Mesh Pro which holds the NPCs Dialogue
    [SerializeField] TextMeshProUGUI dialogueText;

    // 4 Available buttons for user response with function programming
    [SerializeField] Button Option1Button;
    [SerializeField] TextMeshProUGUI Option1Text;
    [SerializeField] NPCResponse Option1Response;
    [SerializeField] Button Option2Button;
    [SerializeField] TextMeshProUGUI Option2Text;
    [SerializeField] NPCResponse Option2Response;
    [SerializeField] Button Option3Button;
    [SerializeField] TextMeshProUGUI Option3Text;
    [SerializeField] NPCResponse Option3Response;
    [SerializeField] Button Option4Button;
    [SerializeField] TextMeshProUGUI Option4Text;
    [SerializeField] NPCResponse Option4Response;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonAIO>();

        gameObject.SetActive(false);
    }

    // NPC calls this with it's dialogue file
    // name is the name of the NPC
    // lines is the set of dialogue and responses programmed
    // states is the set of states to make unlockable and lockable dialogue
    public void startDialogue(string name, NPCDialogue dialogue)
    {
        // Activate the canvas
        gameObject.SetActive(true);

        player.ControllerPause();

        // Set current dialogue
        npcDialogue = dialogue;

        // Start dialogue from beginning
        LoadLine(name, dialogue.lines, 0);
    }

    public void closeDialogue()
    {
        npcDialogue = null;
        hideButtons();
        player.ControllerPause();
        gameObject.SetActive(false);
    }

    // Hide the buttons and delete text
    private void hideButtons()
    {
        Option1Button.gameObject.SetActive(false);
        Option2Button.gameObject.SetActive(false);
        Option3Button.gameObject.SetActive(false);
        Option4Button.gameObject.SetActive(false);

        Option1Text.SetText("");
        Option2Text.SetText("");
        Option3Text.SetText("");
        Option4Text.SetText("");
    }

    // Load a line of the dialogue
    private void LoadLine(string name, string[] lines, int lineNo)
    {
        hideButtons();

        // FIND THE DIALOGUE FROM THE NPC
        // If the line given is not an NPCDialogue line
        if (lines[lineNo].IndexOf("NPCDialogue") == -1)
        {
            closeDialogue();
            Debug.Log("Error: LoadLine given incorrect line number");
        } else
        {
            // Get the text attached to NPCDialogue line
            string text = GetTextBetweenChars('"', 0, lines[lineNo]);
            dialogueText.SetText(name + ": " + text);
        }

        bool nextLine = false;
        int yLevel = 2;
        // GETS THE PLAYER RESPONSES
        // Check if there is a next line
        if (lineNo + 1 < lines.Length) {
            // If there is a next line, generate a response and check next line
            nextLine = GenerateResponse(lines[lineNo + 1], Option1Response, Option1Button, Option1Text, yLevel);
            if (Option1Text.text != "") yLevel -= 45;
        }
        if (lineNo + 2 < lines.Length && nextLine) {
            nextLine = GenerateResponse(lines[lineNo + 2], Option2Response, Option2Button, Option2Text, yLevel);
            if (Option2Text.text != "") yLevel -= 45;
        }
        if (lineNo + 3 < lines.Length && nextLine)
        {
            nextLine = GenerateResponse(lines[lineNo + 3], Option3Response, Option3Button, Option3Text, yLevel);
            if (Option1Text.text != "") yLevel -= 45;
        }
        if (lineNo + 4 < lines.Length && nextLine)
        {
            GenerateResponse(lines[lineNo + 4], Option4Response, Option4Button, Option4Text, yLevel);
        }
    }

    // Get the text between the next set of characters starting at initialIndex
    private string GetTextBetweenChars(char c, int initialIndex, string line)
    {
        int startIndex = line.IndexOf(c, initialIndex) + 1;
        int endIndex = line.IndexOf(c, startIndex);
        if (startIndex == -1 || endIndex == -1) return "";
        return line.Substring(startIndex, endIndex - startIndex);
    }

    // Generate a response which can be used to function the buttons
    private bool GenerateResponse(string line, NPCResponse response, Button button, TextMeshProUGUI text, int yLevel)
    {
        // If the line is not a response, return false
        if (line.IndexOf("NPCResponse") == -1) return false;

        // If the line requires a state, check the state to see if it is met
        if (line.IndexOf("NPCState") != -1)
        {
            bool[] states = npcDialogue.states;
            string npcState = GetTextBetweenChars('"', line.IndexOf("NPCState"), line);
            string[] statecombo = npcState.Split(',');
            // If the state is NOT met, return true
            if ((statecombo[1] == "false" && states[int.Parse(statecombo[0])]) || (statecombo[1] == "true" && !states[int.Parse(statecombo[0])]))
            {
                return true;
            }
        }

        // If there is a case to be met
        if (line.IndexOf("NPCCase") != -1)
        {
            string npcCase = GetTextBetweenChars('"', line.IndexOf("NPCCase"), line);
            string[] casecombo = npcCase.Split(',');
            // If the case is "has" item
            if (casecombo[0] == "has")
            {
                // Look for the items
                Queue<InventoryItem> items = null;
                switch (casecombo[1])
                {
                    case "basicAmmo":
                        items = inventoryController.FindMaterials(InventoryItem.ItemName.BasicAmmo, int.Parse(casecombo[2]));
                        break;
                    case "betterAmmo":
                        items = inventoryController.FindMaterials(InventoryItem.ItemName.BetterAmmo, int.Parse(casecombo[2]));
                        break;
                    case "katana":
                        items = inventoryController.FindMaterials(InventoryItem.ItemName.Katana, int.Parse(casecombo[2]));
                        break;
                    case "basicGun":
                        items = inventoryController.FindMaterials(InventoryItem.ItemName.BasicGun, int.Parse(casecombo[2]));
                        break;
                    case "betterGun":
                        items = inventoryController.FindMaterials(InventoryItem.ItemName.BetterGun, int.Parse(casecombo[2]));
                        break;
                    case "basicGunPart":
                        items = inventoryController.FindMaterials(InventoryItem.ItemName.BasicGunPart, int.Parse(casecombo[2]));
                        break;
                    case "betterGunPart":
                        items = inventoryController.FindMaterials(InventoryItem.ItemName.BetterGunPart, int.Parse(casecombo[2]));
                        break;
                }
                // Queue is null if the items cannot be found, therefore return true
                if (items == null) return true;
            }
        }

        // Set the button text to the response
        text.SetText(GetTextBetweenChars('"', 0, line));
        button.gameObject.SetActive(true);
        button.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, yLevel, 0);

        // Reset the response values
        response.ResetValues();
        
        // Edit the response values based on the line
        if (line.IndexOf("NPCPath") != -1)
        {
            response.NPCPath = int.Parse(GetTextBetweenChars('"', line.IndexOf("NPCPath"), line));
        } else
        {
            response.NPCPath = 0;
        }

        if (line.IndexOf("NPCSetState") != -1)
        {
            response.NPCSetState = GetTextBetweenChars('"', line.IndexOf("NPCSetState"), line);
        }

        if (line.IndexOf("NPCReward") != -1)
        {
            response.NPCReward = GetTextBetweenChars('"', line.IndexOf("NPCReward"), line);
        }

        if (line.IndexOf("NPCTake") != -1)
        {
            response.NPCTake = GetTextBetweenChars('"', line.IndexOf("NPCTake"), line);
        }
        return true;
    }

    private void ActivateResponse(NPCResponse response)
    {
        // If change a state
        if (response.NPCSetState != null)
        {
            // Get state index and bool value
            string[] statecombo = response.NPCSetState.Split(',');
            // Get copy of states
            bool[] newStates = npcDialogue.states;
            // Change the indexed state to true/false
            newStates[int.Parse(statecombo[0])] = statecombo[1] == "true";
            npcDialogue.states = newStates;
        }

        // If take items
        if (response.NPCTake != null)
        {
            // Find which item and how much
            string[] takecombo = response.NPCTake.Split(',');
            switch (takecombo[0])
            {
                case "basicAmmo":
                    inventoryController.UseMaterials(InventoryItem.ItemName.BasicAmmo, int.Parse(takecombo[1]));
                    break;
                case "betterAmmo":
                    inventoryController.UseMaterials(InventoryItem.ItemName.BetterAmmo, int.Parse(takecombo[1]));
                    break;
                case "katana":
                    inventoryController.UseMaterials(InventoryItem.ItemName.Katana, int.Parse(takecombo[1]));
                    break;
                case "basicGun":
                    inventoryController.UseMaterials(InventoryItem.ItemName.BasicGun, int.Parse(takecombo[1]));
                    break;
                case "betterGun":
                    inventoryController.UseMaterials(InventoryItem.ItemName.BetterGun, int.Parse(takecombo[1]));
                    break;
                case "basicGunPart":
                    inventoryController.UseMaterials(InventoryItem.ItemName.BasicGunPart, int.Parse(takecombo[1]));
                    break;
                case "betterGunPart":
                    inventoryController.UseMaterials(InventoryItem.ItemName.BetterGunPart, int.Parse(takecombo[1]));
                    break;
            }
        }

        if (response.NPCReward != null)
        {
            string[] rewardcombo = response.NPCReward.Split(',');

            closeDialogue();
            player.ToggleInventory();

            switch (rewardcombo[0])
            {
                case "basicAmmo":
                    groundInventory.SpawnItem(basicAmmo, 0, 0, int.Parse(rewardcombo[1]));
                    break;
                case "betterAmmo":
                    groundInventory.SpawnItem(betterAmmo, 0, 0, int.Parse(rewardcombo[1]));
                    break;
                case "katana":
                    groundInventory.SpawnItem(katana, 0, 0, int.Parse(rewardcombo[1]));
                    break;
                case "basicGun":
                    groundInventory.SpawnItem(basicGun, 0, 0, int.Parse(rewardcombo[1]));
                    break;
                case "betterGun":
                    groundInventory.SpawnItem(betterGun, 0, 0, int.Parse(rewardcombo[1]));
                    break;
                case "basicGunPart":
                    groundInventory.SpawnItem(basicGunPart, 0, 0, int.Parse(rewardcombo[1]));
                    break;
                case "betterGunPart":
                    groundInventory.SpawnItem(betterGunPart, 0, 0, int.Parse(rewardcombo[1]));
                    break;
            }
            return;
        }

        if (response.NPCPath != 0)
        {
            LoadLine(npcDialogue.NpcName, npcDialogue.lines, response.NPCPath - 1);
        } else
        {
            closeDialogue();
        }
    }

    public void Response1()
    {
        ActivateResponse(Option1Response);
    }

    public void Response2()
    {
        ActivateResponse(Option2Response);
    }

    public void Response3()
    {
        ActivateResponse(Option3Response);
    }

    public void Response4()
    {
        ActivateResponse(Option4Response);
    }
}
