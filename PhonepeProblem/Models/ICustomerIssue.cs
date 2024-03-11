using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhonepeProblem.Models
{
    public interface ICustomerIssue
    {
        string TransactionId { get; }
        string Subject { get; }
        string Description { get; }
        string Email { get; }
        string IssueId { get; }
        IssueStatus Status { get; }

        IssueType IssueType { get; }

        ICustomerAgent AssignedAgent { get; set; }

        string Resolution { get; }

        void SetResolution(string resolution);
        void SetStatus(IssueStatus status);

        string ToString();
    }
}
