using Xunit.Abstractions;

namespace ClipSharp.Core.Test;

public class CommonTest
{
    private ITestOutputHelper outputHelper;

    public CommonTest(ITestOutputHelper outputHelper)
    {
        this.outputHelper = outputHelper;
    }

    [Fact]
    public void PathTest()
    {
        this.outputHelper.WriteLine($"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}");
        // this.outputHelper.WriteLine(App.ClipSharpFolder);
    }
}