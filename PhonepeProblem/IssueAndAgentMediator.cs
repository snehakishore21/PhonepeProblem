using PhonepeProblem.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhonepeProblem
{
    public class IssueAndAgentMediator
    {
        private Dictionary<string, ICustomerIssue> issueMap = new Dictionary<string, ICustomerIssue>();
        Dictionary<IssueType, IList<string>> IssueTypeWithIssueMap = new();

        private Dictionary<string, ICustomerAgent> agentMap = new Dictionary<string, ICustomerAgent>();
        Dictionary<IssueType, IList<string>> IssueTypeWithAgentMap = new Dictionary<IssueType, IList<string>>();

        Dictionary<string, IList<string>> AgentIssuesInPipeLineMap = new();

        Dictionary<string, IList<string>> AgentResolvedIssuesMap = new();

        Dictionary<string, ICustomerIssue> IssuesAgentMap = new();

        internal ICustomerAgent AssignIssue(string issueId)
        {
            if (!issueMap.ContainsKey(issueId))
            {
                throw new Exception("Issue not found");
            }
            ICustomerIssue issue = issueMap[issueId];
            IAgentSelectionStrategy agentSelectionStrategy = AssignAgentStrategyFactory.GetStrategyToSelectAgent(issue);
            (ICustomerAgent agentCurr, bool isWaitlisted) = agentSelectionStrategy.GetAgentForIssue(issue, agentMap);
            
            if (isWaitlisted)
            {
                AgentIssuesInPipeLineMap.TryAdd(agentCurr.AgentEmail, new List<string>());
                NotifyWhenIssueAddedInPipeline(agentCurr, issueId);
            }
            else
            {
                AssignIssueToAgent(agentCurr, issue);
                NotifyWhenIssueAssigned(agentCurr, issueId);
            }

            return agentCurr;
        }

        internal IList<string> GetIssuesWorkedOnBy(string agentEmail)
        {
            IList<string> issuesWorkedOn = new List<string>();
            if(AgentResolvedIssuesMap.ContainsKey(agentEmail))
            {
                issuesWorkedOn = AgentResolvedIssuesMap[agentEmail];
            }
            if (agentMap[agentEmail].CurrentIssue != null)
            {
                issuesWorkedOn.Add(agentMap[agentEmail].CurrentIssue.IssueId);
            }
            return issuesWorkedOn;
        }

        internal void NotifyWhenAgentAdded(ICustomerAgent customerAgent)
        {
            if(agentMap.ContainsKey(customerAgent.AgentEmail))
            {
                throw new Exception("Agent already exists");
            }
            agentMap.Add(customerAgent.AgentEmail, customerAgent);

            foreach (var issueType in customerAgent.IssueType)
            {
                if (IssueTypeWithAgentMap.ContainsKey(issueType))
                {
                    IssueTypeWithAgentMap[issueType].Add(customerAgent.AgentEmail);
                }
                else
                {
                    IssueTypeWithAgentMap.Add(issueType, new List<string>() { customerAgent.AgentEmail });
                }
            }   
        }

        internal void NotifyWhenIssueAdded(ICustomerIssue customerIssue)
        {
            if(issueMap.ContainsKey(customerIssue.IssueId))
            {
                throw new Exception("Issue already exists");
            }
            issueMap.Add(customerIssue.IssueId, customerIssue);
            IssueTypeWithIssueMap.TryAdd(customerIssue.IssueType, new List<string>());
            IssueTypeWithIssueMap[customerIssue.IssueType].Add(customerIssue.IssueId);
        }

        internal void NotifyWhenIssueAssigned(ICustomerAgent customerAgentAssigned, string issueId)
        {
            Console.WriteLine($"Issue {issueId} assigned to {customerAgentAssigned.AgentName}");
        }

        internal void NotifyWhenIssueAddedInPipeline(ICustomerAgent customerAgentAssigned, string issueId)
        {
            Console.WriteLine($"Issue {issueId} added to waitlist of Agent {customerAgentAssigned.AgentName}");
        }

        public void UpdateIssue(string issueId, IssueStatus status, string resolution, ICustomerAgent customerAgent)
        {
            if (!IssuesAgentMap.ContainsKey(customerAgent.AgentEmail))
            { throw new Exception($"Issue {issueId} not assigned to the agent {customerAgent.AgentName}"); }

            if (status == IssueStatus.RESOLVED)
            {
                ResolveIssue(issueId, resolution, customerAgent);
                Console.WriteLine($"{issueId} status updated to {status.ToString()}");
                return;
            }

            ICustomerIssue issue = IssuesAgentMap[customerAgent.AgentEmail];
            
            HandleExceptionForEligibilityForStatusUpdate(issue, customerAgent, issueId);

            issue.SetStatus(status);
            issue.SetResolution(resolution);

            Console.WriteLine($"{issueId} status updated to {status.ToString()}");
        }

        private void HandleExceptionForEligibilityForStatusUpdate(ICustomerIssue issue, ICustomerAgent customerAgent, string issueId)
        {
            if (issue.IssueId != issueId)
            {
                if (AgentIssuesInPipeLineMap.ContainsKey(customerAgent.AgentEmail) && AgentIssuesInPipeLineMap[customerAgent.AgentEmail].Contains(issueId))
                {
                    throw new Exception($"Agent {customerAgent.AgentName} has not started working on issue {issueId}.");
                }
                else
                {
                    throw new Exception($"Issue {issueId} not assigned to the agent {customerAgent.AgentName}");
                }
            }
        }

        private void FreeUpCustomerAgent(ICustomerAgent customerAgent)
        {
            IssuesAgentMap.Remove(customerAgent.AgentEmail);
            customerAgent.AgentStatus = AgentStatus.FREE;
            customerAgent.CurrentIssue = null;
        }

        public void ResolveIssue(string issueId, string resolution, ICustomerAgent customerAgent)
        {
            if (!IssuesAgentMap.ContainsKey(customerAgent.AgentEmail))
            { throw new Exception($"Issue {issueId} not assigned to the agent {customerAgent.AgentName}"); }

            ICustomerIssue issue = IssuesAgentMap[customerAgent.AgentEmail];
            HandleExceptionForEligibilityForStatusUpdate(issue, customerAgent, issueId);

            issue.SetStatus(IssueStatus.RESOLVED);
            issue.SetResolution(resolution);
            FreeUpCustomerAgent(customerAgent);
            AgentResolvedIssuesMap.TryAdd(customerAgent.AgentEmail, new List<string>());
            AgentResolvedIssuesMap[customerAgent.AgentEmail].Add(issueId);
            Console.WriteLine($"{issueId} issue marked resolved");
            
            StartWorkingOnNextTask(customerAgent);
        }

        internal void StartWorkingOnNextTask(ICustomerAgent customerAgent)
        {
            if (!IssuesAgentMap.ContainsKey(customerAgent.AgentEmail) || AgentIssuesInPipeLineMap[customerAgent.AgentEmail].Count == 0)
            {
                return;
            }

            string issueId = AgentIssuesInPipeLineMap[customerAgent.AgentEmail][0];
            ICustomerIssue issue = issueMap[issueId];
            AgentIssuesInPipeLineMap[customerAgent.AgentEmail].RemoveAt(0);
            AssignIssueToAgent(customerAgent, issue);
        }

        private void AssignIssueToAgent(ICustomerAgent customerAgent, ICustomerIssue issue)
        {
            IssuesAgentMap.TryAdd(customerAgent.AgentEmail, issue);
            issue.SetStatus(IssueStatus.IN_PROGRESS);
            customerAgent.CurrentIssue = issue;
        }

        internal IList<ICustomerIssue> GetIssues(Dictionary<string, string> parameters)
        {
            return issueMap.Values.Where(item =>
                parameters.All(param =>
                    item.GetType().GetProperty(param.Key)?.GetValue(item, null)?.Equals(param.Value) ?? false
                )
            ).ToList();
        }
    }
}
