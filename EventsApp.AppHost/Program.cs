var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.EventsApp_ApiService>("apiservice");

builder.AddProject<Projects.EventsApp_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

var username = builder.AddParameter("username", secret: true);
var password = builder.AddParameter("password", secret: true);

var rabbit = builder.AddRabbitMQ("messaging", username, password);

var auth = builder.AddProject<Projects.AuthService>("authservice")
    .WithReference(rabbit);


builder.AddProject<Projects.EventsService>("eventsservice")
    .WithReference(rabbit);

builder.Build().Run();
