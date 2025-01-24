using System.Collections.Generic;
using UnityEngine;

namespace Bullastrum.Utility
{
    public static class ColorUtility
    {
        public enum Color
        {
            Default,
            Black,
            Blue,
            Green,
            Orange,
            Purple,
            Red,
            White,
            Yellow
        }

        private static readonly Dictionary<Color, Color32> Colors = new Dictionary<Color, Color32>()
        {
            { Color.Default, new Color32(255, 255, 255, 255) },
            { Color.Black, new Color32(0, 0, 0, 255) },
            { Color.Blue, new Color32(80, 180, 255, 255) },
            { Color.Green, new Color32(105, 255, 90, 255) },
            { Color.Orange, new Color32(255, 128, 0, 255) },
            { Color.Purple, new Color32(255, 150, 255, 255) },
            { Color.Red, new Color32(255, 120, 120, 255) },
            { Color.White, new Color32(255, 255, 255, 255) },
            { Color.Yellow, new Color32(255, 255, 0, 255) }
        };

        public static Color32 GetColor(Color color)
        {
            return Colors[color];
        }

        public static string GetHexColor(Color color)
        {
            return "#" + UnityEngine.ColorUtility.ToHtmlStringRGBA(Colors[color]);
        }
    }
}