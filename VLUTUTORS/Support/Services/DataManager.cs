using VLUTUTORS.Models;

namespace VLUTUTORS.Support.Services
{
    public class DataManager 
    {
        private CP25Team01Context _db = new CP25Team01Context();

        private static DataManager _dataManager;

        private DataManager() {}

        public static DataManager Instance()
        {
            if(_dataManager == null)
            {
                _dataManager = new DataManager();
            }

            return _dataManager;
        }

        public CP25Team01Context db() { return _db; }

    }
}
