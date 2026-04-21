using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zodiac;

public class StateManager : MonoBehaviour
{
    public bool[] gameState;

    /* Game States[at location]:
     * 0 = Game is not playing
     * 1 = Game is starting
     * 2 = Player Draw Phase
     * 3 = Player Main Phase
     * 4 = Nothing is happening (Player)
     * 5 = Player Battle Phase
     * 6 = Player Main Phase 2
     * 7 = Player End Turn
     * 8 = Player drew a card
     * 9 = Player played a rank 0 Summon
     * 10 = Player played a rank 1 Summon
     * 11 = Player played a rank 2 Summon
     * 12 = Player played a normal Sorcery
     * 13 = Player played a permanent Sorcery
     * 14 = Player played an artifact Sorcery
     * 15 = Player played a Hex face-down
     * 16 = Player activated a normal Hex
     * 17 = Player activated a permanent Hex
     * 18 = Player activated an artifact Hex
     * 19 = Player discarded a card
     * 20 = Player controls 1 Summon
     * 21 = Player controls 2 Summons
     * 22 = Player controls 3 Summons
     * 23 = Player controls 4 Summons
     * 24 = Player controls 5 Summons!
     * 25 = Player controls 1 face-up Sorcery
     * 26 = Player controls 2 face-up Sorceries
     * 27 = Player controls 3 face-up Sorceries
     * 28 = Player controls 4 face-up Sorceries
     * 29 = Player controls 5 face-up Sorceries!
     * 30 = Player controls 1 face-up Hex
     * 31 = Player controls 2 face-up Hexes
     * 32 = Player controls 3 face-up Hexes
     * 33 = Player controls 4 face-up Hexes
     * 34 = Player controls 5 face-up Hexes!
     * 35 = Player controls 1 face-down Sorcery or Hex
     * 36 = Player controls 2 face-down Sorceries or Hexes
     * 37 = Player controls 3 face-down Sorceries or Hexes
     * 38 = Player controls 4 face-down Sorceries or Hexes
     * 39 = Player controls 5 face-down Sorceries or Hexes!
     * 40 = Player took damage
     * 41 = Player gained LIFE
     * 42 = Player attacks
     * 43 = Player controls a Defense Mode Summon
     * 44 = Player controls an Attack Mode Summon
     * 45 = 
     * 46 = 
     * 47 = 
     */

    void Awake()
    {
        gameState = new bool[25];
        for (int i = 0; i < gameState.Length; i++)
        {
            gameState[i] = false;
        }
        gameState[0] = true;
    }
}
