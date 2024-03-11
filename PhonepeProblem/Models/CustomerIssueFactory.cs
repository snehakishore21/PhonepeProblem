using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhonepeProblem.Models
{
    public class CustomerIssueFactory
    {
        private const string PAYMENT = "Payment Related";
        private const string MUTUAL_FUND = "Mutual Fund Related";
        private const string GOLD = "Gold Related";
        private const string INSURANCE = "Insurance Related";
        private const string InvalidIssueErrorMsg = "Invalid Issue Type";

        public ICustomerIssue GetCustomerIssue(string transactionId, string issueType, string subject, string description, string email)
        {
            switch (issueType)
            {
                case PAYMENT:
                    return new PaymentIssue(transactionId, subject, description, email);
                case MUTUAL_FUND:
                    return new MutualFundIssue(transactionId, subject, description, email);
                case GOLD:
                    return new GoldIssue(transactionId, subject, description, email);
                case INSURANCE:
                    return new InsuranceIssue(transactionId, subject, description, email);
                default:    
                    throw new Exception(InvalidIssueErrorMsg);
            }
        }
    }
}
