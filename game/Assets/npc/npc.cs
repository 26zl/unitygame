using UnityEngine;
using MoonSharp.Interpreter;
using System.IO;

public class NPC : MonoBehaviour
{
    // --------------- Public ---------------
    public string luaScriptFile = "npc_logic.lua"; // The Lua file inside StreamingAssets folder

    // --------------- Private ---------------
    private Script luaScript;   // Runs the Lua code
    private float speed;        // Speed we read from Lua
    private Animator animator;  // Controls the animations

    // --------------------------------------
    void Start()
    {
        // 1. Load and run the Lua file
        string path = Path.Combine(Application.streamingAssetsPath, luaScriptFile);
        Debug.Log("Loading Lua from: " + path);

        luaScript = new Script();
        string luaCode = File.ReadAllText(path);
        luaScript.DoString(luaCode);

        // 2. Read the speed variable from Lua (e.g. 0 = idle, 2 = walk, 5 = run)
        speed = (float)luaScript.Globals.Get("speed").Number;

        // 3. Get the Animator component on this GameObject
        animator = GetComponent<Animator>();

        // We are not calling the Lua function getTarget,
        // because we don't want the NPC to move left and right for now
    }

    // --------------------------------------
    void Update()
    {
        // Tells the AnimatorController which animation to play.
        // The NPC itself stays in one place.
        animator.SetFloat("speed", speed);
    }
}