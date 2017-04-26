using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Web.Mvc;
using MedicalAppointmentScheduler.Core.Data;
using MedicalAppointmentScheduler.Core.Business;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using Moq;
using EntityFramework.MoqHelper;

namespace MedicalAppointmentScheduler.Tests.BusinessLayer
{
    [TestClass]
    public class ConditionManagerTest
    {
        private ConditionsManager conditionsManager;
        private Mock<MedicalSchedulerDBEntities> mockContext;
        List<UserDetails> userDetailsData;
        List<Patient> patientData;
        List<Core.Data.Type> typeData;
        List<Patient_Conditions> patient_ConditionData;
        List<Condition> conditionData;

        [TestInitialize]
        public void setUpData()
        {
            
            userDetailsData = new List<UserDetails>();
            var mockUserDetailsSet = EntityFrameworkMoqHelper.CreateMockForDbSet<UserDetails>()
                                      .SetupForQueryOn(userDetailsData)
                                       .WithAdd(userDetailsData)
                                       .WithRemove(userDetailsData)
                                       .WithFind(userDetailsData, "ID");
            mockUserDetailsSet.Setup(m => m.Include(It.IsAny<string>())).Returns(mockUserDetailsSet.Object);

            patient_ConditionData = new List<Patient_Conditions>();
            var mockpatientConditionSet = EntityFrameworkMoqHelper.CreateMockForDbSet<Patient_Conditions>()
                                      .SetupForQueryOn(patient_ConditionData)
                                       .WithAdd(patient_ConditionData)
                                       .WithRemove(patient_ConditionData);

            conditionData = new List<Condition>();
            var mockConditionSet = EntityFrameworkMoqHelper.CreateMockForDbSet<Condition>()
                                      .SetupForQueryOn(conditionData)
                                       .WithAdd(conditionData)
                                       .WithRemove(conditionData);

            patientData = new List<Patient>();
            var mockPatientSet = EntityFrameworkMoqHelper.CreateMockForDbSet<Patient>()
                                     .SetupForQueryOn(patientData)
                                      .WithAdd(patientData)
                                      .WithRemove(patientData)
                                      .WithFind(patientData, "PatientID");

            typeData = new List<Core.Data.Type>();
            var mockTypeSet = EntityFrameworkMoqHelper.CreateMockForDbSet<Core.Data.Type>()
                                     .SetupForQueryOn(typeData)
                                      .WithAdd(typeData)
                                      .WithRemove(typeData)
                                      .WithFind(typeData, "ID");


            mockContext = new Mock<MedicalSchedulerDBEntities>();
            mockContext.Setup(m => m.UserDetails).Returns(mockUserDetailsSet.Object);
            mockContext.Setup(m => m.Patients).Returns(mockPatientSet.Object);
            mockContext.Setup(m => m.Types).Returns(mockTypeSet.Object);
            mockContext.Setup(m => m.Conditions).Returns(mockConditionSet.Object);
            mockContext.Setup(m => m.Patient_Conditions).Returns(mockpatientConditionSet.Object);

            conditionsManager = new ConditionsManager(mockContext.Object);
        }

        [TestMethod]
        public void TestGetDetails()
        {
            //Arrange
            Assert.AreEqual(0, mockContext.Object.Patients.Count());

           
            Patient patient = new Patient() { PatientID=1, Gender="M", HeightFeet=6, HeightInches=2,Weight=140 };

            patientData.Add(patient);

            Assert.AreEqual(1, mockContext.Object.Patients.Count());


            //Act
            Patient testPatient = conditionsManager.GetDetails(1);

            //Assert
            Assert.AreEqual(1, testPatient.PatientID);
            Assert.AreEqual("M", testPatient.Gender);
            Assert.AreEqual(2, testPatient.HeightInches);
            Assert.AreEqual(140, testPatient.Weight);

        }

        [TestMethod]
        public void TestGetTypes()
        {
            //Arrange
            Assert.AreEqual(0, mockContext.Object.Types.Count());
            
            Core.Data.Type type = new Core.Data.Type() {ID = 1, Name="Test Type" };
            typeData.Add(type);

            Assert.AreEqual(1, mockContext.Object.Types.Count());
            
            //Act
            string testTypeName = conditionsManager.GetTypes(1);

            //Assert
            Assert.AreEqual("Test Type", testTypeName);         

        }

        [TestMethod]

        public void TestGetConditions()
        {
            //Arrange
            Patient_Conditions patient_Condition1 = new Patient_Conditions(){ ID= 1, PatientID = 1, TypeID = 1 , ConditionID = 1};
            patient_ConditionData.Add(patient_Condition1);
            Patient_Conditions patient_Condition2 = new Patient_Conditions() { ID = 2, PatientID = 1, TypeID = 1, ConditionID = 2 };
            patient_ConditionData.Add(patient_Condition2);

            Condition condition1 = new Condition() {ID=1, TypeID=1,Name= "ASTHMA" };
            conditionData.Add(condition1);
            Condition condition2 = new Condition() { ID = 2, TypeID = 1, Name = "HEART PROBLEM" };
            conditionData.Add(condition2);
            Condition condition3 = new Condition() { ID = 3, TypeID = 2, Name = "HEARING LOSS" };
            conditionData.Add(condition3);

            List<string> conditions = conditionsManager.GetConditions(1, 1);

            Assert.AreEqual(2, conditions.Count);

        }

        [TestMethod]

        public void TestGetNullConditions()
        {
            //Arrange
            Patient_Conditions patient_Condition1 = new Patient_Conditions() { ID = 1, PatientID = 1, TypeID = 3, ConditionID = 1 };
            patient_ConditionData.Add(patient_Condition1);
            Patient_Conditions patient_Condition2 = new Patient_Conditions() { ID = 2, PatientID = 2, TypeID = 1, ConditionID = 2 };
            patient_ConditionData.Add(patient_Condition2);

            Condition condition1 = new Condition() { ID = 1, TypeID = 1, Name = "ASTHMA" };
            conditionData.Add(condition1);
            Condition condition2 = new Condition() { ID = 2, TypeID = 1, Name = "HEART PROBLEM" };
            conditionData.Add(condition2);
            Condition condition3 = new Condition() { ID = 3, TypeID = 2, Name = "HEARING LOSS" };
            conditionData.Add(condition3);

            List<string> conditions = conditionsManager.GetConditions(1, 1);

            Assert.AreEqual(null, conditions[0]);

        }
    }
}
