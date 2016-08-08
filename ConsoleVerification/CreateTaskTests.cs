using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ForgetTheMilk.Controllers;
using NUnit.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConsoleVerification
{
    //Para versões mais antigas do NUnit
    //[TestFixture]
    public class CreateTaskTests : AssertionHelper
    {
        [Test]
        public void DescriptionAndNoDueDate()
        {
            var input = "Pickup the groceries";

            var task = new Task(input, default(DateTime));

            //var descriptionShouldBe = input;
            //DateTime? dueDateShouldBe = null;
            //var success = descriptionShouldBe == task.Description && dueDateShouldBe == task.DueDate;
            //var failureMessage = "Description: " + task.Description + "should be " + descriptionShouldBe
            //              + Environment.NewLine
            //              + "Due Date: " + task.DueDate + " should be " + dueDateShouldBe;
            //Assert.That(success, failureMessage);

            //Assert.AreEqual(input, task.Description);
            //Assert.AreEqual(null, task.DueDate);

            Expect(task.Description, Is.EqualTo(input));
            Expect(task.DueDate, Is.EqualTo(null));
        }

        [Test]
        [TestCase("Pickup the groceries aug 5 - as of 2015-08-31")]
        [TestCase("Pickup the groceries jul 5 - as of 2015-08-31")]
        public void TestAugDueDateDoesWrapYear(string input)
        {
            var today = new DateTime(2016, 8, 31);

            var task = new Task(input, today);
            
            Expect(task.DueDate.Value.Year, Is.EqualTo(2017));
        }

        [Test]
        public void TestAugDueDateDoesNotWrapYear()
        {
            var input = "Pickup the groceries aug 5 - as of 2016-08-04";
            var today = new DateTime(2016, 8, 4);

            var task = new Task(input, today);
            
            Expect(task.DueDate, Is.EqualTo(new DateTime(2016, 8, 5)));
        }

        [Test]
        [TestCase("Groceries jan 5", 1)]
        [TestCase("Groceries feb 5", 2)]
        [TestCase("Groceries mar 5", 3)]
        [TestCase("Groceries apr 5", 4)]
        [TestCase("Groceries may 5", 5)]
        [TestCase("Groceries jun 5", 6)]
        [TestCase("Groceries jul 5", 7)]
        [TestCase("Groceries aug 5", 8)]
        [TestCase("Groceries sep 5", 9)]
        [TestCase("Groceries oct 5", 10)]
        [TestCase("Groceries nov 5", 11)]
        [TestCase("Groceries dec 5", 12)]
        public void DueDate(string input, int expectedMonth)
        {
            var task = new Task(input, default(DateTime));

            Expect(task.DueDate, Is.Not.Null);
            Expect(task.DueDate.Value.Month, Is.EqualTo(expectedMonth));
        }

        [Test]
        public void TwoDigitDay_ParseBothDigits()
        {
            var input = "groceries apr 10";
            var task = new Task(input, default(DateTime));
            Expect(task.DueDate.Value.Day, Is.EqualTo(10));
        }

        [Test]
        public void DayIsPastTheLastDayOfTheMonth_DoesNotParseDueDate()
        {
            var input = "groceries apr 44";
            var task = new Task(input, default(DateTime));
            Expect(task.DueDate, Is.Null);
        }

        [Test]
        public void AddFeb29TaskInMarchOfYearBeforeLeapYear()
        {
            var input = "groceries feb 29";
            var today = new DateTime(2015, 3, 1);
            var task = new Task(input, today);
            Expect(task.DueDate.Value, Is.EqualTo(new DateTime(2016,2,29)));
        }

    }

    public class TaskLinkTests : AssertionHelper
    {
        class IgnoreLinkValidator : ILinkValidator
        {
            public void Validate(string Link)
            {
                
            }
        }

        [Test]
        public void CreateTask_DescriptionWithALink_SetLink()
        {
            var input = "test http://www.google.com";

            var task = new Task(input, default(DateTime), new LinkValidator());

            Expect(task.Link, Is.EqualTo("http://www.google.com"));
        }

        [Test]
        public void Validate_InvalidUrl_ThrowsException()
        {
            var input = "http://www.doesnotexistdotcom.com";
            
            Expect(() => new Task(input, default(DateTime), new LinkValidator()),
                Throws.Exception.With.Message.EqualTo("Invalid Link " + input));
        }

    }
}
