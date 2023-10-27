using UnityEngine;

namespace Common.Scripts
{
    public class LogUtility
    {
        public static void Log(string s)
        {
            Debug.Log("[DebugInfo] : " +s);
        }
        public static void LogError(string s)
        {
            Debug.LogError("[DebugInfo] : " +s);
        }
        public static void ContentManagementLog(string s)
        {
            Debug.Log("[ContentManagement] : " + s);
        }
        public static void ContentManagementError(string s)
        {
            Debug.LogError("[ContentManagement] : " + s);
        }
        public static void ContentDeliveryLog(string s)
        {
            Debug.Log("[ContentDelivery] : " + s);
        }
        public static void ContentDeliveryLogError(string s)
        {
            Debug.LogError("[ContentDelivery] : " + s);
        }
    }
}