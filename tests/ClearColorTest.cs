#!/usr/bin/env dotnet
#:project mynt.Tests.Base/mynt.Tests.Base.csproj

using mynt.Tests.Base;

using ClearColorTest test = new ClearColorTest();
test.Run();

class ClearColorTest() : TestBase("Clear Color Test")
{
    
}