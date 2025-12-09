


# Minimum Viable Product Features

Add custom display name to the capability names

Scrolling on workflows, connectors, connector, models, model, status pages

Model Call
	- Specify model from registry
	- Specify instructions
	- Specify prompt with argument resolution using path resolution for get
	- Specify the output path for the result
	- Output as text or json for structured output
	  Copy the OpenAI / Gemini method of defining the structued output (defined and advanced schema)
	- Show the input and output context

Model Registry
	- Define model name
	- Define authentication, start with just api key, secret name, (for secrets.json) and then other which means you handle it in overrides
	- Add test button to check connectivity

Max nodes executed for a workflow to prevent looping forever
Max execution time for workflow to prevent it running forever
Max number of runs to keep for a workflow to prevent too much data storage


# Integration Overrides

Max values for nodes, execution and runs to keep
	  
Build
    - Different interfaces for specifying the list of custom serializers
	  1 to find them in the assemblies automatically
	  Another that asks you to list them by type

# Process

- GitHub account
  - Manual build (or tag) to create and publish a release
  - Docusaurus setup for generated static pages site (auto build and publish on changes)

- Build first version of documentation
- Publish and announce

# Implementation Details

Leaving unsaved page
	Should ask user if they really want to leave

Database
	Add a version field to all serializable data, like workflow, connector, model, run, trace so easier to upgrade in future
	Add indexes on fields that are part of ordering queries
	Deleting a workflow, deletes the runs and traces for it
	How to let the user connect to different databases instead of sqlite.
	Built example connecting to SQL Server instead


# Futures

Server
	Integrate use of OmniSharp for full intellisense

Get the nodes by scanning for an attribute, the no need to hard code the 
	RunNode(ThreadContext threadContext, NodeEntity node) (NodeExecutionService.cs)
	return nodeType switch (NodeEntityConverter.cs)


