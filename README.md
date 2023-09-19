# durabletask-samples
Code samples to better understand the execution of orchestrations authored using https://github.com/Azure/durabletask/

# Pre-Requisites
You will need a Azure Storage connection string.
By default the app will use the local development server of [Azure Storage Simulator](https://learn.microsoft.com/en-us/azure/storage/common/storage-use-emulator), ensure that it is up and running.
You can also use an actual Azure Storage instance in the cloud. Update the `AzureStorageConnectionString` config in [App.config](./App.config).

# Running the samples
If you're running the samples from Visual Studio, make sure that you set both `DurableTaskClient` and `DurableTaskWorker` as startup projects. To do this, in the Solution Explorer of VS, Right-Click on the solution > Properties > Startup Projects > Multiple Startup Projects.
You can then run the samples by pressing F5.

From the command line, navigate to the root folder with the sln, and run `dotnet build`.

# Simulating disaster scenarios
To simulate a disaster scenario for the worker, and understand the execution flow when a worker recovers, it is recommended to run the `DurableTaskClient` and `DurableTaskWorker` from two separate terminals. <br />

You can set `DisableOrchestrationVerboseLogs` to `true` and `LogDtfCoreEventTraces` to `false` in [App.config](./App.config) for this scenario. <br /> <br />

In Terminal 1, start the client:
```
> cd DurableTaskClient
> dotnet run
```
This will display a list of samples to choose from. <br />
You can select from one of the longer running orchestrations like `InlineForLoopTestingOrchestration`, `FixedPollingWithInlineRetriesOrchestration` or `UnboundedPollingWithInlineRetriesOrchestration`. <br /><br />

In Terminal 2, start the worker:
```
> cd DurableTaskWorker
> dotnet run
```

<br />

> **Note:** Running `dotnet run disableVerboseLogs` has the same effect as setting `DisableOrchestrationVerboseLogs` to `true`

Execution will begin in the worker after a few seconds, and you will start seeing logs from your selected orchestration and it's child activities. <br />
Mid-way between the execution, for example, when `InlineForLoopTestingOrchestration` is executing for i = 2, you can kill the worker process in Terminal 2 by issuing a Ctrl-C. <br />
This would simulate a real world disaster scenario where a machine node would go down. <br /><br />

Now bring back the worker again by executing `dotnet run` in the `DurableTaskWorker` folder again like before. <br /><br />

You will notice that the worker will resume where it left off. In the case of `InlineForLoopTestingOrchestration`, if you killed the process when it was executing i = 2, the execution would resume from i = 2!

# Digging deeper
To understand the execution flow better, start by setting the `DisableOrchestrationVerboseLogs` flag in [App.config](./App.config) to false before running the worker. This is the default configuration as well.
This will give you clear idea of the orchestation code execution from beginning to end.

Once this is clear, set `LogDtfCoreEventTraces` to `true`. This will capture all events from the DTF Core library and display the various orchestration control events from the framework.

> **Note:** After every change to App.Config, please make sure you build the sln again by running `dotnet build` from the root folder.