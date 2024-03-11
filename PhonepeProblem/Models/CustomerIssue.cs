using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhonepeProblem.Models
{
    public class PaymentIssue : ICustomerIssue
    {
        public string TransactionId { get; }
        public string Subject { get; }
        public string Description { get; }
        public string Email { get; }
        public string IssueId { get; }
        public IssueStatus Status { get; private set; }

        public IssueType IssueType { get; }
        public ICustomerAgent AssignedAgent { get; set;  }
        public string Resolution { get; set;  }

        public PaymentIssue(string transactionId, string subject, string description, string email)
        {
            TransactionId = transactionId;
            Subject = subject;
            Description = description;
            Email = email;
            IssueId = "I" + Guid.NewGuid().ToString();
            Status = IssueStatus.OPEN;
            IssueType = IssueType.PAYMENT;
        }

        public void SetStatus(IssueStatus status)
        {
            Status = status;
        }

        public override string ToString()
        {
            return $"{IssueId} {{ {TransactionId}, {Subject}, {Description}, {Email}, {Status} }}";
        }
    }

    public class MutualFundIssue : ICustomerIssue
    {
        public string TransactionId { get; }
        public string Subject { get; }
        public string Description { get; }
        public string Email { get; }
        public string IssueId { get; }
        public IssueStatus Status { get; private set; }

        public IssueType IssueType { get; }
        public ICustomerAgent AssignedAgent { get; set;  }
        public string Resolution { get; set;  }

        public MutualFundIssue(string transactionId, string subject, string description, string email)
        {
            TransactionId = transactionId;
            Subject = subject;
            Description = description;
            Email = email;
            IssueId = "I" + Guid.NewGuid().ToString();
            Status = IssueStatus.OPEN;
            IssueType = IssueType.MUTUAL_FUND;
        }

        public void SetStatus(IssueStatus status)
        {
            Status = status;
        }

        public override string ToString()
        {
            return $"{IssueId} {{ {TransactionId}, {Subject}, {Description}, {Email}, {Status} }}";
        }
    }

    public class GoldIssue : ICustomerIssue
    {
        public string TransactionId { get; }
        public string Subject { get; }
        public string Description { get; }
        public string Email { get; }
        public string IssueId { get; private set; }
        public IssueStatus Status { get; private set; } = IssueStatus.OPEN;

        public IssueType IssueType { get; }
        public ICustomerAgent AssignedAgent { get; set; }
        public string Resolution { get; set;  }

        public GoldIssue(string transactionId, string subject, string description, string email)
        {
            TransactionId = transactionId;
            Subject = subject;
            Description = description;
            Email = email;
            IssueId = "I" + Guid.NewGuid().ToString();
            Status = IssueStatus.OPEN;
            IssueType = IssueType.GOLD;
        }

        public void SetStatus(IssueStatus status)
        {
            Status = status;
        }

        public override string ToString()
        {
            return $"{IssueId} {{ {TransactionId}, {Subject}, {Description}, {Email}, {Status} }}";
        }
    }

    public class InsuranceIssue : ICustomerIssue
    {
        public string TransactionId { get; }
        public string Subject { get; }
        public string Description { get; }
        public string Email { get; }
        public string IssueId { get; }
        public IssueStatus Status { get; private set; }

        public IssueType IssueType { get; }
        public ICustomerAgent AssignedAgent { get; set;  }
        public string Resolution { get; set;  }

        public InsuranceIssue(string transactionId, string subject, string description, string email)
        {
            TransactionId = transactionId;
            Subject = subject;
            Description = description;
            Email = email;
            IssueId = "I" + Guid.NewGuid().ToString();
            Status = IssueStatus.OPEN;
            IssueType = IssueType.INSURANCE;
        }

        public void SetStatus(IssueStatus status)
        {
            Status = status;
        }

        public override string ToString()
        {
            return $"{IssueId} {{ {TransactionId}, {Subject}, {Description}, {Email}, {Status} }}";
        }
    }
}
