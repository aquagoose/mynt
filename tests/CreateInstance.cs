#!/usr/bin/env dotnet
#:project ../src/mynt/mynt.csproj

using mynt;

Mynt.MessageLogged += (severity, message, _, _) => Console.WriteLine($"[{severity}] {message}");

InstanceInfo info = new InstanceInfo("Create Instance Test");
Instance instance = Instance.CreateInstance(in info);
instance.Dispose();