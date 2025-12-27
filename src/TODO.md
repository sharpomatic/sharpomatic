## Using worklows

New interface for notifying completion (failure) of a run.
- Should return a context with output values for

Add sync and Task methods for running a workflow.

## Override Secrets

Interface allowing the code to modify the parameterValues for a connection / model before use.
This allows code to get API keys or other secrets and provide them.

## LLM's

- Azure OpenAI - models
- Microsoft Foundry - models

## Conversations

- ChatMessage serializable (use AIJsonUtilities.DefaultOptions), would need to update our context serializing to notice and use this for serial/deserial
- Allow ModelCall to specify the path to get current conversation from (with create one if not present), then append call result to this same instance.

## Export and Import to move between dev/staging/prod

- Workflows
- Connections
- Models

 To zip file?
	  
## Process

- GitHub account
  - Manual build (or tag) to create and publish a release
  - Docusaurus setup for generated static pages site (auto build and publish on changes)

- Build first version of documentation

## Implementation Details

Output the editor and engine as nuget packages
GitHub build pipeline to build/package and publish to nuget the two packages

## Security

How does the user add extra security that impacts the controllers that the hosting adds?


# Futures

Usage - token usage counts

Traces - show tool calls, reasoning and other details

Images - input and output of images

ChatKit output integration
User output, as there might be a stream of outputs
User input request, for LLM or any other part of a process

MCP Server

Server
	Integrate use of OmniSharp for full intellisense





