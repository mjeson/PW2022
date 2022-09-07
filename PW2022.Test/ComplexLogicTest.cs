using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PW2022.Logic;

namespace PW2022.Test
{
    [TestClass]
    public class ComplexLogicTest
    {
        [TestMethod]
        public void Execute_Weak_Success()
        {
            var restrictedApiMock = new Mock<IRestrictedApi>();
            restrictedApiMock.Setup(x => x.GetSalt());

            var subject = new ComplexLogic(restrictedApiMock.Object);
            subject.Input = new Input()
            {
                Data = "Test data"
            };

            subject.Execute();

            restrictedApiMock.Verify(x => x.GetSalt(), Times.Once());
            //Verify GetSalt called once
            //Minimal value
        }

        [TestMethod]
        public void Execute_Strong_Success()
        {
            bool getSaltIsCalled = false;
            int salt = 3;

            var restrictedApiMock = new Mock<IRestrictedApi>();
            restrictedApiMock.Setup(x => x.GetSalt())
                .Callback(() =>
                {
                    getSaltIsCalled = true;
                })
                .Returns(salt);

            var subject = new ComplexLogic(restrictedApiMock.Object);
            subject.Input = new Input()
            {
                Data = "Test data"
            };

            subject.Execute();

            var expected = new Output()
            {
                Encoded = "VmtkV2VtUkRRbXRaV0ZKbw==",
                Times = salt,
            };

            JObject expectedJson = JObject.Parse(JsonConvert.SerializeObject(expected));
            JObject actualJson = JObject.Parse(JsonConvert.SerializeObject(subject.Output));
            Assert.IsTrue(JObject.DeepEquals(expectedJson, actualJson));

            Assert.IsTrue(getSaltIsCalled);
            //Verify the actual value
            //Shows reader what a sample Output looks like
            //Troubleshoot when getSaltIsCalled
            //Fail the test when Output properties are changed
        }
    }
}
