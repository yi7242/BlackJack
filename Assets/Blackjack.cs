#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
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
    int win;
    private string[] Cards = {"A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K"};
    private int lastFrameHold;
    private int opponentTimer;
    /// 初期化処理
    /// </summary>

    public override void InitGame()
    {
        stillHold = false;
        Reset();
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
                lastFrameHold = 0;
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

        if (turn == 1 && !stillHold)
        {
            if (gc.GetPointerFrameCount(0) == 0)
            {
                if (lastFrameHold >= 1)
                {
                    myHand.Add(gc.Random(1,13));
                    if (Score(myHand) > 21)
                    {
                        win = 0;
                        turn = 3;
                    }
                    else if (Score(myHand) == 21)
                    {
                        if (Score(opponentHand) == 21)
                        {
                            win = 2;
                        }
                        turn = 3;
                    }
                }
            }

            if (gc.GetPointerFrameCount(0) == 60)
            {
                stillHold = true;
                turn = 2;
                checkWin();
            }
        }

        if (turn == 2)
        {
            opponentTimer--;
            if (opponentTimer == 0)
            {
                opponentTimer = 30;
                opponentHand.Add(gc.Random(1,13));
                checkWin();
            }
        }

        if (turn == 3 && !stillHold)
        {
            if (gc.GetPointerFrameCount(0) > 0)
            {
                stillHold = true;
                Reset();
                turn = 1;
                for (int i = 0; i < 2; i++)
                {
                    myHand.Add(gc.Random(1,13));
                    opponentHand.Add(gc.Random(1,13));
                }
            }
        }
        if (Score(myHand) == 21)
        {
            if (Score(opponentHand) == 21)
            {
                win = 2;
            }
            turn = 3;
        }
        lastFrameHold = gc.GetPointerFrameCount(0);
        Debug.Log(lastFrameHold);
    }

    /// <summary>
    /// 描画の処理
    /// </summary>
    public override void DrawGame()
    {
        gc.ClearScreen();
        gc.SetColor(0,0,0);
        gc.SetFontSize(50);
        if (turn == 0)
        {
            gc.DrawString("タップで開始", 60,50);
        }
        if (turn == 1)
        {
            gc.DrawString("タップでHit, 長押しでStand", 60,50);
        }

        if (turn == 2)
        {
            gc.DrawString("相手のターン", 60,50);
        }

        if (turn == 3)
        {
            gc.DrawString("あなた:" +Score(myHand)+ "  相手:" + Score(opponentHand), 60,50);
            if (win == 1)
            {
                gc.DrawString("あなたの勝ち！", 60,120);
            }
            else if (win == 0)
            {
                gc.DrawString("あなたの負け！", 60,120);
            }
            else
            {
                gc.DrawString("引き分け！", 60,120);
            }
            gc.DrawString("タップして再びプレイ", 60,220);
        }

        gc.DrawString("相手の手: " ,60,390);
        if (turn == 1)
        {
            gc.DrawString(Cards[opponentHand[0]-1], 60,480);
            if (opponentHand[0] == 10)
            {
                gc.DrawString("?", 180,480);
            }
            else
            {
                gc.DrawString("?", 130,480);
            }
        }
        else
        {
            for (int i = 0; i < opponentHand.Count; i++)
            {
                if (i == 0)
                {
                    gc.DrawString(Cards[opponentHand[i]-1].ToString(), 60+i*70,480);
                }
                else
                {
                    int x = opponentHand[i-1];
                    if (x != 10)
                    {
                        gc.DrawString(Cards[opponentHand[i]-1].ToString(), 60+i*70,480);
                    }
                    else
                    {
                        gc.DrawString(Cards[opponentHand[i]-1].ToString(), 60+i*120,480);
                    }
                }
                gc.DrawString("(" + Score(opponentHand) + ")", 60,570);
            }
        }
        gc.DrawString("自分の手: ",60,750);

        for (int i = 0; i < myHand.Count; i++)
        {
            if (i == 0)
            {
                gc.DrawString(Cards[myHand[i]-1].ToString(), 60+i*70,840);
            }
            else
            {
                int x = myHand[i-1];
                if (x != 10)
                {
                    gc.DrawString(Cards[myHand[i]-1].ToString(), 60+i*70,840);
                }
                else
                {
                    gc.DrawString(Cards[myHand[i]-1].ToString(), 60+i*120,840);
                }
            }
            gc.DrawString("(" + Score(myHand) + ")", 60,930);
        }

    }

    void checkWin()
    {
        if (Score(opponentHand) == 21)
        {
            win = 0;
            turn = 3;
        }
        else if (Score(opponentHand) > 21)
        {
            turn = 3;
        }
        else if (Score(opponentHand) > 17)
        {
            if (Score(opponentHand) > Score(myHand))
            {
                win = 0;
            }
            else if (Score(opponentHand) == Score(myHand))
            {
                win = 2;
            }
            turn = 3;
        }
    }

    int Score(List<int> hands)
    {
        int score = 0;
        foreach (var card in hands)
        {
            if (card >= 10)
            {
                score += 10;
            }
            else
            {
                score += card;
            }
        }

        return score;
    }

    private void Reset()
    {
        money = 10000;
        turn = 0;
        myHand = new List<int>();
        opponentHand = new List<int>();
        win = 1;
        lastFrameHold = 0;
        opponentTimer = 30;
    }
}
