var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.EventsApp_ApiService>("apiservice");

builder.AddProject<Projects.EventsApp_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

builder.Build().Run();
