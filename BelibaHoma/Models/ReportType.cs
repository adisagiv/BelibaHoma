using System.ComponentModel;

namespace BelibaHoma.Models
{
    public enum ReportType
    {
        [Description("סטטיסטיקת שעות חניכה")]
        HourStatistics,

        [Description("סטטיסטיקת התרעות")]
        AlertsStatistics,

        [Description("סטטיסטיקת הצטרפות ונשירת חניכים")]
        JoinDropStatistics,

        [Description("סטטיסטיקת שעות חניכה כתלות בותק בתוכנית")]
        InvestedHoursStatistics,
        
        [Description("סטטיסטיקת ממוצעי ציונים לאורך סמסטרים")]
        AvrGradeStatistics,

        [Description("היסטוגרמת שעות חניכה לפי חונכויות")]
        HourHistogram
    }
}