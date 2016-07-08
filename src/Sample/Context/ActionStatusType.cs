using System;
using System.Linq;
using System.Collections.Generic;

namespace Sample.Context
{
    //<summary>何らかの行為に関わる処理ステータス概念</summary>
    public enum ActionStatusType
    {
        /** 未処理 */
        Unprocessed,
        /** 処理中 */
        Processing,
        /** 処理済 */
        Processed,
        /** 取消 */
        Cancelled,
        /** エラー */
        Error
    }
    public static class ActionStatusTypes
    {
        //<summary>ステータス一覧</summary>
        public static List<ActionStatusType> All =
            Enum.GetValues(typeof(ActionStatusType)).Cast<ActionStatusType>().ToList();
        //<summary>完了済みのステータス一覧</summary>
        public static List<ActionStatusType> FinishTypes =
            new List<ActionStatusType> { ActionStatusType.Processed, ActionStatusType.Cancelled };
        //<summary>未完了のステータス一覧(処理中は含めない)</summary>
        public static List<ActionStatusType> UnprocessingTypes =
            new List<ActionStatusType> { ActionStatusType.Unprocessed, ActionStatusType.Error };
        //<summary>未完了のステータス一覧(処理中も含める)</summary>
        public static List<ActionStatusType> UnprocessedTypes =
            new List<ActionStatusType> { ActionStatusType.Unprocessed, ActionStatusType.Processing, ActionStatusType.Error };

        public static bool IsFinish(this ActionStatusType statusType)
        {
            return FinishTypes.Contains(statusType);
        }
        public static bool IsUnprocessing(this ActionStatusType statusType)
        {
            return UnprocessingTypes.Contains(statusType);
        }
        public static bool IsUnprocessed(this ActionStatusType statusType)
        {
            return UnprocessedTypes.Contains(statusType);
        }
    }
}
