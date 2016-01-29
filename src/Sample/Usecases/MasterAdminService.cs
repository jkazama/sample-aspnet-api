using Sample.Models;
using Sample.Models.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Usecases
{
    //<summary>
    // サービスマスタドメインに対する社内ユースケース処理。
    //</summary>
    public class MasterAdminService
    {
        private Repository _rep;
        public MasterAdminService(Repository rep)
        {
            _rep = rep;
        }

        //<summary>社員を取得します。</summary>
        public Staff GetStaff(string id)
        {
            return _rep.Tx(() => Staff.Get(_rep, id));
        }

        //<summary>社員権限を取得します。</summary>
        public List<StaffAuthority> FindStaffAuthority(string staffId)
        {
            return _rep.Tx(() => StaffAuthority.Find(_rep, staffId));
        }

        //<summary>休日情報を登録す</summary>
        public void RegisterHoliday(RegHoliday p)
        {
            _rep.Tx(() => Holiday.Register(_rep, p));
        }

    }
}
