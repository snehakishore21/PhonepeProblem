using PhonepeProblem.Models;
using System;
using System.Transactions;
using static System.Net.Mime.MediaTypeNames;

namespace PhonepeProblem
{
    class Program
    {
        public static void Main(string[] args)
        {
            IssueAndAgentMediator mediator = new IssueAndAgentMediator();
            AgentManager agentManager = AgentManager.GetInstance(mediator);
            IssueManager customerIssueManager = IssueManager.GetInstance(mediator);

            ICustomerIssue issue1 = customerIssueManager.CreateIssue("T1", "Payment Related", "Payment Failed", "My payment failed but money is debited", "testUser1@test.com");
            ICustomerIssue issue2 = customerIssueManager.CreateIssue("T2", "Mutual Fund Related", "Purchase Failed", "Unable to purchase Mutual Fund", "testUser2@test.com");
            ICustomerIssue issue3 = customerIssueManager.CreateIssue("T3", "Payment Related", "Payment Failed", "My payment failed but money is debited", "testUser2@test.com");

            agentManager.AddAgent("agent1@test.com", "Agent 1", new List<IssueType> { IssueType.GOLD, IssueType.PAYMENT });
            agentManager.AddAgent("agent2@test.com", "Agent 2", new List<IssueType> { IssueType.PAYMENT });

            ICustomerAgent agentI1 = customerIssueManager.AssignIssue(issue1.IssueId);

            ICustomerAgent agentI2 = customerIssueManager.AssignIssue(issue2.IssueId);

            ICustomerAgent agentI3 = customerIssueManager.AssignIssue(issue3.IssueId);

            string customerIssues = customerIssueManager.GetIssues(new Dictionary<string, string> { { "email", "testUser2@test.com" } });
            Console.WriteLine(customerIssues);
            string customerIssues2 = customerIssueManager.GetIssues(new Dictionary<string, string> { { "type", "Payment Related" } });
            Console.WriteLine(customerIssues2);

            agentManager.UpdateIssue(issue1.IssueId, IssueStatus.IN_PROGRESS, "Waiting for payment confirmation", agentI3);
            agentManager.ResolveIssue(issue1.IssueId,"PaymentFailed debited amount will get reversed", agentI3);
            
            string history = agentManager.ViewAgentWorkHistory();
            Console.WriteLine(history);
        }
    }
}