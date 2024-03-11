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

        private Dictionary<string, ICustomerIssue> issueMap = new Dictionary<string, ICustomerIssue>();
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
            Console.WriteLine($"Issue {customerIssue.IssueId} created against transaction \"{transactionId}\".");
            issueAndAgentMediator.NotifyWhenIssueAdded(customerIssue);
            issueMap.TryAdd(customerIssue.IssueId, customerIssue);
            return customerIssue;
        }

        public void AssignIssue( string issueId)
        {
            if(!issueMap.ContainsKey(issueId))
            {
                throw new Exception("Issue not found");
            }
            ICustomerAgent customerAgentAssigned = issueAndAgentMediator.GetAgentForIssue(issueMap[issueId]);

            issueMap[issueId].AssignedAgent = customerAgentAssigned;
            issueMap[issueId].SetStatus(customerAgentAssigned.AgentStatus == AgentStatus.FREE ? IssueStatus.IN_PROGRESS: IssueStatus.OPEN);
            customerAgentAssigned.AgentStatus = AgentStatus.WORKING;
            customerAgentAssigned.CurrentIssue = issueMap[issueId];
            Console.WriteLine($"Issue {issueId} assigned to agent {customerAgentAssigned.AgentName}");
            issueAndAgentMediator.NotifyWhenIssueAssigned(customerAgentAssigned, issueId);
        }

        public IEnumerable<ICustomerIssue> GetIssues(Dictionary<string, string> parameters)
        {
            return issueMap.Values.Where(item =>
                parameters.All(param =>
                    item.GetType().GetProperty(param.Key)?.GetValue(item, null)?.Equals(param.Value) ?? false
                )
            );
        }
    }
}
