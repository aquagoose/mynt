#!/usr/bin/env dotnet
#:project ../src/mynt/mynt.csproj

using mynt;

InstanceInfo info = new InstanceInfo("Create Instance Test");
Instance instance = Instance.CreateInstance(in info);
instance.Dispose();