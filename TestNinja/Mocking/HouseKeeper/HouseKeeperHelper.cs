using System.Collections.Generic;

namespace TestNinja.Mocking.HouseKeeper {
    using System;

    namespace TestNinja.Mocking {
        public class HousekeeperHelper {
            private readonly IHouseKeeperRepository _houseKeeperRepository;
            private readonly IStatementGenerator _statementGenerator;
            private readonly IEmailSender _emailSender;

            public HousekeeperHelper(IHouseKeeperRepository houseKeeperRepository = null,
                IStatementGenerator statementGenerator = null, IEmailSender emailSender = null) {
                _houseKeeperRepository = houseKeeperRepository ?? new HouseKeeperRepository();
                _statementGenerator = statementGenerator ?? new StatementGenerator();
                _emailSender = emailSender ?? new EmailSender();
            }

            public bool SendStatementEmails(DateTime statementDate) {
                var housekeepers = _houseKeeperRepository.GetHousekeepers();

                foreach (var housekeeper in housekeepers) {
                    if (housekeeper.Email == null)
                        continue;

                    var statementFilename =
                        _statementGenerator.SaveStatement(housekeeper.Oid, housekeeper.FullName, statementDate);

                    if (string.IsNullOrWhiteSpace(statementFilename))
                        continue;

                    var emailAddress = housekeeper.Email;
                    var emailBody = housekeeper.StatementEmailBody;

                    try {
                        _emailSender.EmailFile(emailAddress, emailBody, statementFilename,
                            string.Format("Sandpiper Statement {0:yyyy-MM} {1}", statementDate, housekeeper.FullName));
                    }
                    catch (Exception e) {
                        XtraMessageBox.Show(e.Message, string.Format("Email failure: {0}", emailAddress),
                            MessageBoxButtons.OK);
                    }
                }
                return true;
            }

            public enum MessageBoxButtons {
                OK
            }

            public class XtraMessageBox {
                public static void Show(string s, string housekeeperStatements, MessageBoxButtons ok) {
                }
            }

            public class MainForm {
                public bool HousekeeperStatementsSending { get; set; }
            }

            public class DateForm {
                public DateForm(string statementDate, object endOfLastMonth) {
                }

                public DateTime Date { get; set; }

                public DialogResult ShowDialog() {
                    return DialogResult.Abort;
                }
            }

            public enum DialogResult {
                Abort,
                OK
            }
        }
    }
}