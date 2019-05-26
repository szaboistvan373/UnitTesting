using System.Data.Entity;

namespace TestNinja.Mocking {
    public class EmployeeController {
        public IEmployeeStorage EmployeeStorage { private get; set; }

        public EmployeeController() {
            EmployeeStorage = new EmployeeStorage();
        }

        public ActionResult DeleteEmployee(int id) {
            EmployeeStorage.DeleteEmployee(id);

            return RedirectToAction("Employees");
        }

        private ActionResult RedirectToAction(string employees) {
            return new RedirectResult();
        }
    }

    public class ActionResult { }

    public class RedirectResult : ActionResult { }

    public class EmployeeContext {
        public DbSet<Employee> Employees { get; set; }

        public void SaveChanges() {
        }
    }

    public interface IEmployeeStorage {
        void DeleteEmployee(int employeeId);
    }

    public class EmployeeStorage : IEmployeeStorage {
        private EmployeeContext _db;

        public EmployeeStorage() {
            _db = new EmployeeContext();
        }

        public void DeleteEmployee(int employeeId) {
            var employee = _db.Employees.Find(employeeId);
            if (employee == null)
                return;
            _db.Employees.Remove(employee);
            _db.SaveChanges();
        }
    }

    public class Employee {
    }
}