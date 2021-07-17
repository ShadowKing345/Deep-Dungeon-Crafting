using System;

namespace Ui.Notifications
{
    public enum NotificationLevel
    {
        Log,
        Warning,
        Error
    }

    public static class NotificationLevelsExtension
    {
        public static float GetStandardDuration(this NotificationLevel level) => level switch
        {
            NotificationLevel.Log => 5f,
            NotificationLevel.Warning => 10f,
            NotificationLevel.Error => float.MaxValue,
            _ => 0
        };
    }
}