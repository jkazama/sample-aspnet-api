using System.Collections.Generic;
using System.Linq;
using Sample.Context.Orm;

namespace Sample.Models.Master
{
    //<summary>
    // 社員に割り当てられた権限を表現します。
    // low: 実際はグループ概念や組織概念も含めていきます。
    //</summary>
    public class StaffAuthority : OrmActiveRecord<StaffAuthority>
    {
        /** ID */
        public long Id { get; set; }
        /** 社員ID */
        public string StaffId { get; set; }
        /** 権限名称(「プリフィックスにROLE_」を付与してください) */
        public string Authority { get; set; }

        //<summary>社員IDに紐付く権限一覧を返します</summary>
        public static List<StaffAuthority> Find(Repository rep, string staffId)
        {
            return rep.Set<StaffAuthority>().Where(m => m.StaffId == staffId).ToList();
        }
    }
}