using System;
using System.Linq;
using VLUTUTORS.Models;

namespace VLUTUTORS.Support.Services {
    public class MoneyServices 
    {
        public static void AddMoney(int amount, int userId, CP25Team01Context _db, Action<bool> callback) 
        {
            //Taikhoannguoidung userAccount = _db.Taikhoannguoidungs.Where(a => a.Equals(userId)).FirstOrDefault();

            //userAccount.Xu += amount;
            //_db.Update(userAccount);
            //_db.SaveChanges();

            //callback?.Invoke(true);
        }

        public static void SubtractMoney(int amount, int userId, CP25Team01Context _db, Action<bool> callback) 
        {
            //Taikhoannguoidung userAccount = _db.Taikhoannguoidungs.Where(a => a.Equals(userId)).FirstOrDefault();
            //if(userAccount.Xu < amount)
            //{
            //    callback?.Invoke(false);
            //    return;
            //}
            //userAccount.Xu -= amount;
            //_db.Update(userAccount);
            //_db.SaveChanges();

            //callback?.Invoke(true);
        }
    }
}
