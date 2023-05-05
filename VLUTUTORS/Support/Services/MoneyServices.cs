using System;
using System.Linq;
using VLUTUTORS.Models;

namespace VLUTUTORS.Support.Services {
    public class MoneyServices 
    {
        public static void AddMoney(int amount, int userId, CP25Team01Context _db, Action<bool> callback = null) 
        {
            Taikhoannguoidung userAccount = _db.Taikhoannguoidungs.Find(userId);

            userAccount.SoDuVi += amount;

            if(userAccount.SoDuVi == null)
            {
                userAccount.SoDuVi = 0;
                userAccount.SoDuVi += amount;
            }

            _db.Update(userAccount);
            _db.SaveChanges();

            callback?.Invoke(true);
        }

        public static void SubtractMoney(int amount, int userId, CP25Team01Context _db, Action<bool> callback = null) 
        {
            Taikhoannguoidung userAccount = _db.Taikhoannguoidungs.Find(userId);
            if (userAccount.SoDuVi < amount)
            {
                callback?.Invoke(false);
                return;
            }
            userAccount.SoDuVi -= amount;

            if (userAccount.SoDuVi == null)
            {
                userAccount.SoDuVi = 0;
                userAccount.SoDuVi += amount;
            }

            _db.Update(userAccount);
            _db.SaveChanges();

            callback?.Invoke(true);
        }

        public static string AutoGenCodeWhenDeposit() 
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();
            string code = new string(Enumerable.Repeat(chars, 6)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            return code;
        }


    }
}
