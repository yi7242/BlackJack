#nullable enable
using System;
using System.Collections.Generic;
using GameCanvas;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// ゲームクラス。
/// 学生が編集すべきソースコードです。
/// </summary>
public sealed class Blackjack : GameBase
{
    private int money;
    private int turn;
    private List<int> myHand;
    private List<int> opponentHand;
    private bool stillHold;
    bool win;
    /// 初期化処理
    /// </summary>

    public override void InitGame()
    {
        money = 10000;
        turn = 0;
        stillHold = false;
        myHand = new List<int>();
        opponentHand = new List<int>();
        win = false;
    }

    /// <summary>
    /// 動きなどの更新処理
    /// </summary>
    public override void UpdateGame()
    {
        if (stillHold == true)
        {
            if (gc.GetPointerFrameCount(0) == 0)
            {
                stillHold = false;
            }
        }
        if (turn == 0)
        {
            if (gc.GetPointerFrameCount(0) == 1)
            {
                turn = 1;
                stillHold = true;
                for (int i = 0; i < 2; i++)
                {
                    myHand.Add(gc.Random(1,13));
                    opponentHand.Add(gc.Random(1,13));
                }
            }

        }
    }

    /// <summary>
    /// 描画の処理
    /// </summary>
    public override void DrawGame()
    {
        gc.ClearScreen();
        gc.SetColor(0,0,0);
        gc.SetFontSize(50);

        gc.DrawString("Dealer Hand: " ,60,40);
        for (int i = 0; i < opponentHand.Count; i++)
        {
            if (i == 0)
            {
                gc.DrawString(opponentHand[i].ToString(), 60+i*70,100);
            }
            else
            {
                int x = opponentHand[i-1];
                if (x <= 9)
                {
                    gc.DrawString(opponentHand[i].ToString(), 60+i*70,100);
                }
                else
                {
                    gc.DrawString(opponentHand[i].ToString(), 60+i*120,100);
                }
            }
        }
        gc.DrawString("My Hand: ",60,400);

        for (int i = 0; i < myHand.Count; i++)
        {
            if (i == 0)
            {
                gc.DrawString(myHand[i].ToString(), 60+i*70,460);
            }
            else
            {
                int x = myHand[i-1];
                if (x <= 9)
                {
                    gc.DrawString(myHand[i].ToString(), 60+i*70,460);
                }
                else
                {
                    gc.DrawString(myHand[i].ToString(), 60+i*120,460);
                }
            }
        }

    }
}
