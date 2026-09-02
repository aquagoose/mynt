#!/usr/bin/env dotnet
#:project ../src/mynt/mynt.csproj

using mynt;

Mynt.MessageLogged += (message, severity, _, _) => Console.WriteLine($"[{severity}] {message}");

InstanceInfo instanceInfo = new InstanceInfo("Create Instance Test", true);
Instance instance = Instance.Create(in instanceInfo);
Console.WriteLine(instance.Backend);

Adapter[] adapters = instance.EnumerateAdapters();
foreach (Adapter adapter in adapters)
    Console.WriteLine(adapter);

instance.Dispose();