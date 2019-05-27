using System.Collections.Generic;

namespace TestNinja.Mocking.HouseKeeper {
    public interface IHouseKeeperRepository {
        IEnumerable<Housekeeper> GetHousekeepers();
    }

    public class HouseKeeperRepository : IHouseKeeperRepository {
        private readonly UnitOfWork _unitOfWork;

        public HouseKeeperRepository() {
            _unitOfWork = new UnitOfWork();
        }

        public IEnumerable<Housekeeper> GetHousekeepers() {
            return _unitOfWork.Query<Housekeeper>();
        }
    }
}