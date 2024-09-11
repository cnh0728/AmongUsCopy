using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EPlayerColor
{
    Red, Blue, Green,
    Pink, Orange, Yellow,
    Black, White, Purple,
    Brown, Cyan, Lime,
}

public class PlayerColor
{
    public static Dictionary<EPlayerColor, Color> colors = new Dictionary<EPlayerColor, Color>()
    {
        {EPlayerColor.Red, new Color(1f, 0f, 0f) },
        {EPlayerColor.Blue, new Color(0.1f, 0.1f, 1f) },
        {EPlayerColor.Green, new Color(0f, 0.6f, 0f) },
        {EPlayerColor.Pink, new Color(1f, 0.3f, 0.9f) },
        {EPlayerColor.Orange, new Color(1f, 0.4f, 0f) },
        {EPlayerColor.Yellow, new Color(1f, 0.9f, 0.1f) },
        {EPlayerColor.Black, new Color(0.2f, 0.2f, 0.2f) },
        {EPlayerColor.White, new Color(0.9f, 1f, 1f) },
        {EPlayerColor.Purple, new Color(0.6f, 0f, 0.6f) },
        {EPlayerColor.Brown, new Color(0.7f, 0.2f, 0f) },
        {EPlayerColor.Cyan, new Color(0f, 1f, 1f) },
        {EPlayerColor.Lime, new Color(0.1f, 1f, 0.1f) },
    };

    public static Color Red { get { return colors[EPlayerColor.Red]; } }
    public static Color Blue { get { return colors[EPlayerColor.Blue]; } }
    public static Color Green { get { return colors[EPlayerColor.Green]; } }
    public static Color Pink { get { return colors[EPlayerColor.Pink]; } }
    public static Color Orange { get { return colors[EPlayerColor.Orange]; } }
    public static Color Yellow { get { return colors[EPlayerColor.Yellow]; } }
    public static Color Black { get { return colors[EPlayerColor.Black]; } }
    public static Color White { get { return colors[EPlayerColor.White]; } }
    public static Color Purple { get { return colors[EPlayerColor.Purple]; } }
    public static Color Brown { get { return colors[EPlayerColor.Brown]; } }
    public static Color Cyan { get { return colors[EPlayerColor.Cyan]; } }
    public static Color Lime { get { return colors[EPlayerColor.Lime]; } }

    public static Color GetColor(EPlayerColor color)
    {
        return colors[color];
    }
}
