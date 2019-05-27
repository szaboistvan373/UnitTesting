using System;
using System.IO;

namespace TestNinja.Mocking.HouseKeeper {
    public interface IStatementGenerator {
        string SaveStatement(int housekeeperOid, string housekeeperName, DateTime statementDate);
    }

    public class StatementGenerator : IStatementGenerator {
        public string SaveStatement(int housekeeperOid, string housekeeperName, DateTime statementDate) {
            var report = new HousekeeperStatementReport(housekeeperOid, statementDate);

            if (!report.HasData)
                return string.Empty;

            report.CreateDocument();

            var filename = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                string.Format("Sandpiper Statement {0:yyyy-MM} {1}.pdf", statementDate, housekeeperName));

            report.ExportToPdf(filename);

            return filename;
        }

        public class HousekeeperStatementReport {
            public HousekeeperStatementReport(int housekeeperOid, DateTime statementDate) {
            }

            public bool HasData { get; set; }

            public void CreateDocument() {
            }

            public void ExportToPdf(string filename) {
            }
        }
    }
}