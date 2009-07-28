using BusinessData.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BusinessData.Entities;

namespace BusinessDataTest
{
    
    
    /// <summary>
    ///This is a test class for CalendariosDAOTest and is intended
    ///to contain all CalendariosDAOTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CalendariosDAOTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for GetCalendarioByAnoSemestre
        ///</summary>
        [TestMethod()]
        public void GetCalendarioByAnoSemestreTest()
        {
            CalendariosDAO target = new CalendariosDAO(); // TODO: Initialize to an appropriate value
            int ano = 2008; // TODO: Initialize to an appropriate value
            int semestre = 2; // TODO: Initialize to an appropriate value
            Calendario actual;
            actual = target.GetCalendarioByAnoSemestre(ano, semestre);
            Assert.AreEqual(ano, actual.Ano);
            Assert.AreEqual(semestre, actual.Semestre);
        }
    }
}
