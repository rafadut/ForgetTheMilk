using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ForgetTheMilk.Controllers;
using NUnit.Framework;

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
        public void TestMayDueDateDoesWrapYear()
        {
            var input = "Pickup the groceries august 5 - as of 2015-08-31";
            var today = new DateTime(2016, 8, 31);

            var task = new Task(input, today);
            
            Expect(task.DueDate, Is.EqualTo(new DateTime(2017, 8, 5)));
        }

        [Test]
        public void TestMayDueDateDoesNotWrapYear()
        {
            var input = "Pickup the groceries august 5 - as of 2016-08-04";
            var today = new DateTime(2016, 8, 4);

            var task = new Task(input, today);
            
            Expect(task.DueDate, Is.EqualTo(new DateTime(2016, 8, 5)));
        }
    }
}
