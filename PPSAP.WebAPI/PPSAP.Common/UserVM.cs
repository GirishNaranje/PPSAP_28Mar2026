using System;

namespace PPSAP.Common
{
    public class UserVM
    {
        public string UserName { get; set; }
    }

    public class UserJsonVM
    {
        public string MasterCustomerId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PrimaryEmail { get; set; }

        public string CommunicationsEmail { get; set; }

        public DateTimeOffset? ResidencyStart { get; set; }

        public DateTimeOffset? ResidencyEnd { get; set; }

        public string ResidencyProgramName { get; set; }

        public string ResidencyProgramId { get; set; }

        public object ProgramDirectorProgramName { get; set; }

        public object ProgramDirectorProgramId { get; set; }

        public string Email { get; set; }

        public string RenewalMessage { get; set; }

        public string RenewalLink { get; set; }

        public string Password { get; set; }
    }
}
