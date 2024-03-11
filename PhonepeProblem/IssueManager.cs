using PhonepeProblem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhonepeProblem
{
    public class IssueManager
    {
        private static IssueManager issueManager;

        private IssueAndAgentMediator issueAndAgentMediator;

        private IssueManager(IssueAndAgentMediator issueAndAgentMediator)
        {
            this.issueAndAgentMediator = issueAndAgentMediator;
        }

        public static IssueManager GetInstance(IssueAndAgentMediator issueAndAgentMediator)
        {
            if (issueManager == null)
            {
                issueManager = new IssueManager(issueAndAgentMediator);
            }
            return issueManager;
        }

        public ICustomerIssue CreateIssue(string transactionId, string issueType, string subject, string description, string email)
        {
            CustomerIssueFactory customerIssueFactory = new CustomerIssueFactory();
            ICustomerIssue customerIssue = customerIssueFactory.GetCustomerIssue(transactionId, issueType, subject, description, email);
            issueAndAgentMediator.NotifyWhenIssueAdded(customerIssue);
            Console.WriteLine($"Issue {customerIssue.IssueId} created against transaction \"{transactionId}\".");
            return customerIssue;
        }

        public ICustomerAgent AssignIssue(string issueId)
        {
            ICustomerAgent agent = issueAndAgentMediator.AssignIssue(issueId);
            return agent;
        }

        public string  GetIssues(Dictionary<string, string> parameters)
        {
            var issues = issueAndAgentMediator.GetIssues(parameters);
            string issuesStrings = string.Empty;
            foreach (ICustomerIssue customerIssue in issues)
            {
                issuesStrings+=customerIssue.ToString()+"\n";
            }
            return issuesStrings;

        }
    }
}
