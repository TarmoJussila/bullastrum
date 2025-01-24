using System.Diagnostics;
using UnityEngine;

namespace Bullastrum.Utility
{
    public static class Log
    {
        public enum Type
        {
            Default,
            Warning,
            Error
        }

        [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
        public static void Message(string messageTitle)
        {
            Message(messageTitle, string.Empty, ColorUtility.Color.Default, Type.Default);
        }

        [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
        public static void Message(string messageTitle, Object context = null)
        {
            Message(messageTitle, string.Empty, ColorUtility.Color.Default, Type.Default, context);
        }

        [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
        public static void Message(string messageTitle, Type type = Type.Default, Object context = null)
        {
            Message(messageTitle, string.Empty, ColorUtility.Color.Default, type, context);
        }

        [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
        public static void Message(string messageTitle, ColorUtility.Color color = ColorUtility.Color.Default, Type type = Type.Default, Object context = null)
        {
            Message(messageTitle, string.Empty, color, type, context);
        }

        [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
        public static void Message(string messageTitle, string messageDetail, Object context)
        {
            Message(messageTitle, messageDetail, ColorUtility.Color.Default, Type.Default, context);
        }

        [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
        public static void Message(string messageTitle, string messageDetail, Type type, Object context = null)
        {
            Message(messageTitle, messageDetail, ColorUtility.Color.Default, type, context);
        }

        [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
        public static void Message(string messageTitle, string messageDetail = "", ColorUtility.Color color = ColorUtility.Color.Default, Type type = Type.Default, Object context = null)
        {
            string colorCode = ColorUtility.GetHexColor(color);
            string colon = (!string.IsNullOrEmpty(messageDetail)) ? ":" : "";

            switch (type)
            {
                case Type.Default:
                {
                    UnityEngine.Debug.Log("<color=" + colorCode + "><b>" + messageTitle + colon + "</b> " + messageDetail + "</color>", context);
                    break;
                }
                case Type.Warning:
                {
                    UnityEngine.Debug.LogWarning("<color=" + colorCode + "><b>" + messageTitle + colon + "</b> " + messageDetail + "</color>", context);
                    break;
                }
                case Type.Error:
                {
                    UnityEngine.Debug.LogError("<color=" + colorCode + "><b>" + messageTitle + colon + "</b> " + messageDetail + "</color>", context);
                    break;
                }
            }
        }
    }
}