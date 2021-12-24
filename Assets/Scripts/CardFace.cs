using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CardFace 
{
    
    private static int playerCard1;
    private static int playerCard2;
    private static int playerCard3;
    private static int bankerCard1;
    private static int bankerCard2;
    private static int bankerCard3;
    
    public static void SetPlayerCard1(int n)
    {
        playerCard1 = n;
    }
    public static int GetPlayerCard1()
    {
        return playerCard1;
    }
    public static void SetPlayerCard2(int n)
    {
        playerCard2 = n;
    }
    public static int GetPlayerCard2()
    {
        return playerCard2;
    }
    public static void SetPlayerCard3(int n)
    {
        playerCard3 = n;
    }
    public static int GetPlayerCard3()
    {
        return playerCard3;
    }
    public static void SetBankerCard1(int n)
    {
        bankerCard1 = n;
    }
    public static int GetBankerCard1()
    {
        return bankerCard1;
    }
    public static void SetBankerCard2(int n)
    {
        bankerCard2 = n;
    }
    public static int GetBankerCard2()
    {
        return bankerCard2;
    }
    public static void SetBankerCard3(int n)
    {
        bankerCard3 = n;
    }
    public static int GetBankerCard3()
    {
        return bankerCard3;
    }
}
