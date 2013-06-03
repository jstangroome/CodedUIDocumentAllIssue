using System.Drawing.Imaging;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UITesting.HtmlControls;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodedUIDocumentAll
{
    [CodedUITest]
    public class CodedUITest1
    {
        [TestMethod]
        [DeploymentItem("HTMLPage1.html")]
        public void CodedUITestMethod1()
        {
            var testPage = Path.Combine(TestContext.DeploymentDirectory, "HTMLPage1.html");
            var window = BrowserWindow.Launch(testPage);
            
            // run script before Coded UI interaction to show document.all.item(...) returns a single object
            window.ExecuteScript("foo();");

            window.CaptureImage().Save(Path.Combine(TestContext.DeploymentDirectory, "00-before.png"), ImageFormat.Png);
            TestContext.AddResultFile("00-before.png");

            // Use Coded UI to Find a control but not interact with it
            var radioButton = new HtmlRadioButton(window);
            radioButton.SearchProperties.Add(HtmlControl.PropertyNames.Id, "a");
            radioButton.Find();

            // run script after Coded UI interaction to show document.all.item(...) now returns an array of length 1
            window.ExecuteScript("foo();");

            window.CaptureImage().Save(Path.Combine(TestContext.DeploymentDirectory, "01-after.png"), ImageFormat.Png);
            TestContext.AddResultFile("01-after.png");

            var p = new HtmlCustom(window);
            p.SearchProperties.Add(HtmlControl.PropertyNames.Id, "p");
            StringAssert.DoesNotMatch(p.InnerText, new Regex(@"\.checked : undefined"));
        }

        public TestContext TestContext { get; set; }
    }
}
