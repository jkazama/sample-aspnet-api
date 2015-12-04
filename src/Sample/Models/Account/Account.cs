using System;
using System.Collections.Generic;
using System.Linq;

namespace Sample.Models.Account
{
    //<summary>
    // 口座を表現します。
    // low: サンプル用に必要最低限の項目だけ口座状態を表現します
    //</summary>
    public class Account
    {
        /** 口座ID */
        public string Id { get; set; }
        /** 口座名義 */
        public string Name { get; set; }
        /** メールアドレス */
        public string Mail { get; set; }
        /** 口座状態 */
        public AccountStatusType StatusType { get; set; }

        public static Account GetValid(Repository rep, string id)
        {
            return rep.Get<Account>(m => m.Id == id);
        }

        public static List<Account> FindAll(Repository rep) {
            return rep.FindAll<Account>();
        }

    }

    //<summary>口座状態を表現します</summary>
    public enum AccountStatusType
    {
        /** 通常 */
        Normal,
        /** 退会 */
        Withdrwal
    }
    public static class AccountStatusTypes
    {
        //<summary>口座状態一覧</summary>
        public static List<AccountStatusType> All =
            Enum.GetValues(typeof(AccountStatusType)).Cast<AccountStatusType>().ToList();
        public static bool valid(this AccountStatusType statusType) { return statusType == AccountStatusType.Normal; }
        public static bool invalid(this AccountStatusType statusType) { return !valid(statusType); }
    }
}