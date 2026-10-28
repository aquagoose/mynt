#!/usr/bin/env dotnet
#:project mynt.Tests.Base/mynt.Tests.Base.csproj

using mynt.Tests.Base;

using ClearColorTest test = new ClearColorTest();
test.Run();

class ClearColorTest() : TestBase("Clear Color Test")
{
    protected override void Loop()
    {
        CommandList.Begin();
        CommandList.End();
        
        Device.ExecuteCommandList(CommandList);
    }
}