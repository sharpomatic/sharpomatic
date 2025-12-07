


# Minimum Viable Product Features

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

# Integration Overrides

Models
	- Connector building
	  Just override the authentication entry
	  Just override the openaiexecutionsettings
	  Override it all by you returning the actual connector instance
	  
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


# Futures

Server
	Integrate use of OmniSharp for full intellisense


