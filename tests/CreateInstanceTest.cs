#!/usr/bin/env dotnet
#:project ../src/mynt/mynt.csproj

using mynt;

InstanceInfo instanceInfo = new InstanceInfo("Create Instance Test", true);
Instance instance = Instance.Create(in instanceInfo);

instance.Dispose();