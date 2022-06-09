using Microsoft.Azure.Management.Synapse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Analytics.Synapse.Artifacts;
using Azure.Analytics.Synapse.Artifacts.Models;

namespace FlowGate.Services
{
    public class SynapseTriggerServiceOptions
    {
        public string WorkspaceName { get; set; } = "";
        public Azure.Core.TokenCredential? Credential { get; set;}
        public string SubscriptionId { get; set; } = "";
    }

    public class SynapseTriggerService
    {

        const string SynapseworkspaceDevSuffix = "dev.azuresynapse.net";
        private string _subscriptionId;

        private PipelineClient _pipelineClient;

        public SynapseTriggerService(SynapseTriggerServiceOptions options)
        {
            _subscriptionId = options.SubscriptionId;

            _pipelineClient = new PipelineClient(new Uri($"https://{options.WorkspaceName}.{SynapseworkspaceDevSuffix}"), options.Credential);
        }

        public async Task RunPipeline(PipelineResource pipeline)
        {
            await _pipelineClient.CreatePipelineRunAsync(pipeline.Name);
        }

        public async Task<PipelineResource> GetPipelineAsync(string name)
        {
            return (await _pipelineClient.GetPipelineAsync(name)).Value;
        }
        public async Task<List<PipelineResource>> FindPipelinesInWorkspace()
        {
            List<PipelineResource> pipelines = new List<PipelineResource>();

            await foreach (var pipeline in _pipelineClient.GetPipelinesByWorkspaceAsync())
            {
                pipelines.Add(pipeline);
            }
            return pipelines;
        }
    }
}
