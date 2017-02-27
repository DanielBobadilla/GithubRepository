using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using GenoomTree.Repositories;
using GenoomTree.Models;
using GenoomTree.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTestPeopleController
    {
        [TestMethod]
        public void ViewNameDetails()
        {
            
            //Arrengue
            var repository = new Mock<IPeopleRepository>();

            var people = new People { PersonId = 1, Name = "Orvile Simpson" , Parent1Id =  null, Parent2Id = null , Birthday = DateTime.MinValue };
            repository.Setup(x => x.Get(1)).Returns(people);
            PeopleController controller = new PeopleController (repository.Object);

            // Act
            ViewResult result = controller.Details(1) as ViewResult;

            // Assert
            Assert.AreEqual("Details", result.ViewName);

        }

        [TestMethod]
        public void ViewNameFamily()
        {
            //Arrengue
            var repository = new Mock<IPeopleRepository>();

            var person1 = new People { PersonId = 1, Name = "Orvile Simpson", Parent1Id = null, Parent2Id = null, Birthday = DateTime.MinValue };
            var person2 = new People { PersonId = 2, Name = "Second", Parent1Id = 1, Parent2Id = null, Birthday = DateTime.MinValue };

            List<People> persons = new List<People>();
            persons.Add(person1);
            persons.Add(person2);

            repository.Setup(x => x.GetFamily(1)).Returns(persons);
            PeopleController controller = new PeopleController(repository.Object);

            // Act
            ViewResult result = controller.Family (2) as ViewResult;

            // Assert
            Assert.AreEqual("Family", result.ViewName);


        }
        [TestMethod]
        public void ViewNameTree ()
        {
            //Arrengue
            var repository = new Mock<IPeopleRepository>();

            var people = new People { PersonId = 1, Name = "Orvile Simpson", Parent1Id = null, Parent2Id = null, Birthday = DateTime.MinValue };
            repository.Setup(x => x.Get(1)).Returns(people);
            PeopleController controller = new PeopleController(repository.Object);


            // Act
            ViewResult result = controller.tree(1) as ViewResult;

            // Assert
            Assert.AreEqual("tree", result.ViewName);
        }

        [TestMethod]
        public void ViewNameChildren()
        {
            //Arrengue
            var repository = new Mock<IPeopleRepository>();

            var people = new People { PersonId = 1, Name = "Orvile Simpson", Parent1Id = null, Parent2Id = null, Birthday = DateTime.MinValue };
            repository.Setup(x => x.Get(1)).Returns(people);
            PeopleController controller = new PeopleController(repository.Object);


            // Act
            ViewResult result = controller.children(1) as ViewResult;

            // Assert
            Assert.AreEqual("children", result.ViewName);
        }

        [TestMethod]
        public void ViewNameParents()
        {
            //Arrengue
            var repository = new Mock<IPeopleRepository>();

            var people = new People { PersonId = 1, Name = "Orvile Simpson", Parent1Id = null, Parent2Id = null, Birthday = DateTime.MinValue };
            repository.Setup(x => x.Get(1)).Returns(people);
            PeopleController controller = new PeopleController(repository.Object);


            // Act
            ViewResult result = controller.parents(1) as ViewResult;

            // Assert
            Assert.AreEqual("parents", result.ViewName);
        }
        [TestMethod]
        public void PersonHaveParents()
        {
            //Arrengue
            var repository = new Mock<IPeopleRepository>();

            var person1 = new People { PersonId = 1, Name = "Orvile Simpson", Parent1Id = null, Parent2Id = null, Birthday = DateTime.MinValue };
            var person2 = new People { PersonId = 2, Name = "Second", Parent1Id = 1, Parent2Id = null, Birthday = DateTime.MinValue };

            repository.Setup(x => x.PersonHaveParents(2)).Returns(true);

            PeopleController controller = new PeopleController(repository.Object);

            // Act
            ViewResult result = controller.parents(2) as ViewResult;

            // Assert
            Assert.AreEqual(null, result);

        }


    }
}
