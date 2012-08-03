using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;
using POS.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using POS.Domain.Abstract;
using POS.Domain.Model;
using System.Web.Mvc;

namespace POS.Tests
{
    
    
    /// <summary>
    ///This is a test class for ControllerBase and is intended
    ///to contain all ControllerBase Unit Tests
    ///</summary>
    [TestClass()]
    public class ControllerBaseTest
    {
        /// <summary>
        ///A test for BypassAntiForgeryTokenAttribute Constructor
        ///</summary>
        // TODO: Ensure that the UrlToTest attribute specifies a URL to an ASP.NET page (for example,
        // http://.../Default.aspx). This is necessary for the unit test to be executed on the web server,
        // whether you are testing a page, web service, or a WCF service.
        [TestMethod]
        public void BypassAntiForgeryTokenAttribute()
        {
            var localMock = new Mock<IProductRepository>();
            
            /*ControllerBase.BypassAntiForgeryTokenAttribute target = new ControllerBase.BypassAntiForgeryTokenAttribute();
            Assert.Inconclusive("TODO: Implement code to verify target");
            // This needs to test that if an action has the attribute [BypassAntiForgeryToken] the method 
            // BypassAntiForgeryTokenAttribute within ControllerBase is called once and 
            // UseAntiForgeryTokenOnPostByDefault is not called at all.*/

        }
    }
}
