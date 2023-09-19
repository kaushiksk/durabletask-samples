# durabletask-samples
Code samples to better understand the execution of orchestrations authored using https://github.com/Azure/durabletask/

# Pre-Requisites
You will need a Azure Storage connection string.
By default the app will use the local development server of [Azure Storage Simulator](https://learn.microsoft.com/en-us/azure/storage/common/storage-use-emulator), ensure that it is up and running.
You can also use an actual Azure Storage instance in the cloud. Update the `AzureStorageConnectionString` config in [App.config](DurableTaskSamples/App.config).

# Running the samples
Clone the repo and build directly from Visual Studio after loading the sln file (double click DurableTaskSamples.sln).
```
> cd DurableTaskSamples\DurableTaskSamples
> dotnet run
```