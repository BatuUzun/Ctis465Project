var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.API_Users>("api-users");

builder.AddProject<Projects.API_Hospital>("api-hospital");

builder.AddProject<Projects.API_Gateway>("api-gateway");

builder.Build().Run();
