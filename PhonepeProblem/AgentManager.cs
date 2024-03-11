using PhonepeProblem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhonepeProblem
{
    public class AgentManager
    {
        private static AgentManager agentManager;
        private IssueAndAgentMediator issueAndAgentMediator;
        private List<ICustomerAgent> agents = new List<ICustomerAgent>();
        private AgentManager(IssueAndAgentMediator issueAndAgentMediator)
        {
            this.issueAndAgentMediator = issueAndAgentMediator;
        }

        public static AgentManager GetInstance(IssueAndAgentMediator issueAndAgentMediator)
        {
            if (agentManager == null)
            {
                agentManager = new AgentManager(issueAndAgentMediator);
            }
            return agentManager;
        }

        public ICustomerAgent AddAgent(string agentEmail, string agentName, List<string> issueType)
        {
            ICustomerAgent customerAgent = new CustomerAgent(agentEmail, agentName, issueType);
            Console.WriteLine($"Agent {customerAgent.AgentName} created.");
            issueAndAgentMediator.NotifyWhenAgentAdded(customerAgent);
            return customerAgent;
        }

        public Dictionary<string, IList<string>> ViewAgentWorkHistory()
        {
            Dictionary<string, IList<string>> result = new();
            foreach(var agent in agents)
            {
                IList<string> issues = issueAndAgentMediator.GetIssuesResolvedBy(agent.AgentEmail);
                result.Add(agent.AgentName, issues);
            }
            return result;
        }

        public IList<ICustomerIssue> SearchForIssues(string paramKey, string value)
        {
            IList<ICustomerIssue> issues = issueAndAgentMediator.SearchForIssues(paramKey, value);
            return issues;
        }

        public IList<ICustomerIssue> GetListOfIssues(ICustomerAgent agent)
        {
            IList<ICustomerIssue> customerIssues = issueAndAgentMediator.GetListOfIssues(agent.AgentEmail);
            return customerIssues;
        }


        public void UpdateIssue(string issueId, IssueStatus status, string resolution, ICustomerAgent customerAgent)
        {
            issueAndAgentMediator.UpdateIssue(issueId, status, resolution, customerAgent);
            if(status == IssueStatus.RESOLVED)
            {
                customerAgent.AgentStatus  = AgentStatus.FREE;
                issueAndAgentMediator.StartWorkingOnNextTask(customerAgent);
            }
        }

        public void ResolveIssue(string issueId, string resolution, ICustomerAgent customerAgent)
        {
            issueAndAgentMediator.ResolveIssue(issueId, resolution, customerAgent);
            customerAgent.AgentStatus = AgentStatus.FREE;
            issueAndAgentMediator.StartWorkingOnNextTask(customerAgent);
        }
    }
}
