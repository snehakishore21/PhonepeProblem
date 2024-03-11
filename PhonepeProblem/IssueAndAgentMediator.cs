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
        Dictionary<IssueType, IList<ICustomerAgent>> CustomerAgentIssueMap = new Dictionary<IssueType, IList<ICustomerAgent>>();

        Dictionary<IssueType, IList<ICustomerIssue>> IssueTypeWithIssueMap = new();

        Dictionary<string, IList<ICustomerIssue>> IssuesInPipeLine = new();

        Dictionary<string, IList<string>> ResolvedIssuesAgentMap = new();

        Dictionary<string, IList<ICustomerIssue>> IssuesAgentMap = new();

        internal ICustomerAgent GetAgentForIssue(ICustomerIssue issue)
        {
            IssueType issueType = issue.IssueType;
            if (CustomerAgentIssueMap.ContainsKey(issueType) && CustomerAgentIssueMap[issueType].Count>0)
            {
                foreach (var agent in CustomerAgentIssueMap[issueType])
                {
                    if (agent.AgentStatus == AgentStatus.FREE)
                    {
                        return agent;
                    }
                }
                ICustomerAgent agentCurr = CustomerAgentIssueMap[issueType].First();
                IssuesInPipeLine.TryAdd(agentCurr.AgentEmail, new List<ICustomerIssue>());
                IssuesInPipeLine[agentCurr.AgentEmail].Add(issue);
                return agentCurr;
            }
            else
            {
                throw new Exception("No agent found for the issue type");
            }
        }

        internal IList<string> GetIssuesResolvedBy(string agentEmail)
        {
            if(ResolvedIssuesAgentMap.ContainsKey(agentEmail))
            {
                return ResolvedIssuesAgentMap[agentEmail];
            }
            else
            {
                throw new Exception("No issues resolved by the agent");
            }
        }

        internal void NotifyWhenAgentAdded(ICustomerAgent customerAgent)
        {
            foreach (var issueType in customerAgent.IssueType)
            {
                if (CustomerAgentIssueMap.ContainsKey(issueType))
                {
                    CustomerAgentIssueMap[issueType].Add(customerAgent);
                }
                else
                {
                    CustomerAgentIssueMap.Add(issueType, new List<ICustomerAgent>() { customerAgent });
                }
            }   
        }

        internal void NotifyWhenIssueAdded(ICustomerIssue customerIssue)
        {
            IssueTypeWithIssueMap.TryAdd(customerIssue.IssueType, new List<ICustomerIssue>());
            IssueTypeWithIssueMap[customerIssue.IssueType].Add(customerIssue);
        }

        internal void NotifyWhenIssueAssigned(ICustomerAgent customerAgentAssigned, string issueId)
        {
            Console.WriteLine($"Issue {issueId} assigned to {customerAgentAssigned.AgentName}");
        }

        public IList<ICustomerIssue> SearchForIssues(string paramKey, string value)
        {

        }

        public IList<ICustomerIssue> GetListOfIssues(string agentId)
        {
            if(IssuesAgentMap.ContainsKey(agentId))
            {
                return IssuesAgentMap[agentId];
            }
            else
            {
                throw new Exception("No issues found for the agent");
            }
        }

        public void UpdateIssue(string issueId, IssueStatus status, string resolution, ICustomerAgent customerAgent)
        {
            if(IssuesAgentMap.ContainsKey(customerAgent.AgentEmail))
            {
                var issue = IssuesAgentMap[customerAgent.AgentEmail].Where(x => x.IssueId == issueId).FirstOrDefault();
                if(issue!=null)
                {
                    issue.SetStatus(status);
                    issue.Resolution = resolution;
                    if(status == IssueStatus.RESOLVED)
                    {
                        if(ResolvedIssuesAgentMap.ContainsKey(customerAgent.AgentEmail))
                        {
                            ResolvedIssuesAgentMap[customerAgent.AgentEmail].Add(issueId);
                        }
                        else
                        {
                            ResolvedIssuesAgentMap.Add(customerAgent.AgentEmail, new List<string>() { issueId });
                        }
                    }
                }
                else
                {
                    throw new Exception("Issue not found for the agent");
                }
            }
            else
            {
                throw new Exception("No issues found for the agent");
            }
        }

        public void ResolveIssue(string issueId, string resolution, ICustomerAgent customerAgent)
        {
            if(IssuesAgentMap.ContainsKey(customerAgent.AgentEmail))
            {
                var issue = IssuesAgentMap[customerAgent.AgentEmail].Where(x => x.IssueId == issueId).FirstOrDefault();
                if(issue!=null)
                {
                    issue.SetStatus(IssueStatus.RESOLVED);
                    issue.Resolution = resolution;
                    if(ResolvedIssuesAgentMap.ContainsKey(customerAgent.AgentEmail))
                    {
                        ResolvedIssuesAgentMap[customerAgent.AgentEmail].Add(issueId);
                    }
                    else
                    {
                        ResolvedIssuesAgentMap.Add(customerAgent.AgentEmail, new List<string>() { issueId });
                    }
                }
                else
                {
                    throw new Exception("Issue not found for the agent");
                }
            }
            else
            {
                throw new Exception("No issues found for the agent");
            }
        }

        internal void StartWorkingOnNextTask(ICustomerAgent customerAgent)
        {
            if(IssuesInPipeLine.TryGetValue(customerAgent.AgentEmail, out var issues))
            {
                if(issues.Count>0)
                {
                    var issue = issues.First();
                    issues.Remove(issue);
                    if(IssuesAgentMap.ContainsKey(customerAgent.AgentEmail))
                    {
                        IssuesAgentMap[customerAgent.AgentEmail].Add(issue);
                    }
                    else
                    {
                        IssuesAgentMap.Add(customerAgent.AgentEmail, new List<ICustomerIssue>() { issue });
                    }
                    NotifyWhenIssueAssigned(customerAgent, issue.IssueId);
                }
            }
        }
    }
}
