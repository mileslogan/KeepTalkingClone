using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhosOnFirstArrays : MonoBehaviour
{
    public string[][] arrays = new string[28][];

    public string[] READY = new string[14];
    public string[] FIRST = new string[14];
    public string[] NO = new string[14];
    public string[] BLANK = new string[14];
    public string[] NOTHING = new string[14];
    public string[] YES = new string[14];
    public string[] WHAT = new string[14];
    public string[] UHHH = new string[14];
    public string[] LEFT = new string[14];
    public string[] RIGHT = new string[14];
    public string[] MIDDLE = new string[14];
    public string[] OKAY = new string[14];
    public string[] WAIT = new string[14];
    public string[] PRESS = new string[14];
    public string[] YOU = new string[14];
    public string[] YOUARE = new string[14];
    public string[] YOUR = new string[14];
    public string[] YOURE = new string[14];
    public string[] UR = new string[14];
    public string[] U = new string[14];
    public string[] UHHUH = new string[14];
    public string[] UHUH = new string[14];
    public string[] WHAT2 = new string[14];
    public string[] DONE = new string[14];
    public string[] NEXT = new string[14];
    public string[] HOLD = new string[14];
    public string[] SURE = new string[14];
    public string[] LIKE = new string[14];

    void Awake()
    {
        arrays[0] = READY;
        arrays[1] = FIRST;
        arrays[2] = NO;
        arrays[3] = BLANK;
        arrays[4] = NOTHING;
        arrays[5] = YES;
        arrays[6] = WHAT;
        arrays[7] = UHHH;
        arrays[8] = LEFT;
        arrays[9] = RIGHT;
        arrays[10] = MIDDLE;
        arrays[11] = OKAY;
        arrays[12] = WAIT;
        arrays[13] = PRESS;
        arrays[14] = YOU;
        arrays[15] = YOUARE;
        arrays[16] = YOUR;
        arrays[17] = YOURE;
        arrays[18] = UR;
        arrays[19] = U;
        arrays[20] = UHHUH;
        arrays[21] = UHUH;
        arrays[22] = WHAT2;
        arrays[23] = DONE;
        arrays[24] = NEXT;
        arrays[25] = HOLD;
        arrays[26] = SURE;
        arrays[27] = LIKE;

    }
}
