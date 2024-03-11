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

        public ICustomerAgent AddAgent(string agentEmail, string agentName, List<IssueType> issueType)
        {
            ICustomerAgent customerAgent = new CustomerAgent(agentEmail, agentName, issueType);
            issueAndAgentMediator.NotifyWhenAgentAdded(customerAgent);
            Console.WriteLine($"Agent {customerAgent.AgentName} created.");
            return customerAgent;
        }

        public string ViewAgentWorkHistory()
        {
            Dictionary<string, IList<string>> result = GetAgentWorkHistory();
            string history = string.Empty;
            foreach (var agentRes in result)
            {
                history+= $"{agentRes.Key} -> ( {string.Join(',', agentRes.Value)} ),";
            }
            history = history.Trim(',');
            return history;
        }

        public Dictionary<string, IList<string>> GetAgentWorkHistory()
        {
            Dictionary<string, IList<string>> result = new();
            foreach(var agent in agents)
            {
                IList<string> issues = issueAndAgentMediator.GetIssuesWorkedOnBy(agent.AgentEmail);
                result.Add(agent.AgentName, issues);
            }
            return result;
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
