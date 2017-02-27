using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GenoomThree.Models;
using GenoomTree.Models;
using GenoomTree.Repositories;
using System.Text;

namespace GenoomTree.Controllers
{
    public class PeopleController : Controller
    {
        readonly IPeopleRepository repository;

        public PeopleController(IPeopleRepository repository)
        {
            this.repository = repository;    
        }

        // GET: People
        public ActionResult Index()
        {
            return View( repository.GetPeople() );
        }

        // GET: People/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var people = repository.Get(id);

            if (people == null)
            {
                return NotFound();
            }

            return View("Details", people);
        }

        // GET: People/Family/5
        public ActionResult Family(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var family = repository.GetFamily(id);

            if (family == null)
            {
                return NotFound();
            }
            ViewBag.Family = family;

            return View("Family", family);
        }

        // GET: People/Create
        public IActionResult Create()
        {
            return View();
        }


        // GET: People/Details/5
        public ActionResult parents(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var people = repository.Get(id);

            if (people == null)
            {
                return NotFound();
            }

            return View("parents", people);
        }
        
        // POST: People/parents 
        // GenerateEmptyParents
        [HttpPost]
        [ActionName("parents")]
        public ActionResult parentsPost (int id)
        {
            List<int> results = new List<int>();

            if (ModelState.IsValid)
            {
                if (repository.PersonHaveParents(id))
                {
                    return NotFound();
                }

                results.Add(repository.CreateEmptyParent1(id));
                results.Add(repository.CreateEmptyParent2(id));
            }
            
            return RedirectToAction("Index");
        }


        // GET: People/Children/5
        public ActionResult children(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var people = repository.Get(id);

            if (people == null)
            {
                return NotFound();
            }

          
            return View("children", people);
        }

        // POST: People/Children
        
        [HttpPost]
        [ActionName("children")]
        public ActionResult childrenPost(int id)
        {

            if (ModelState.IsValid)
            {
                People partner = repository.PersonPartner(id); 

                if (partner == null)
                {
                    return NotFound();
                }

                repository.CreateChild(id, partner.PersonId);

            }

            return RedirectToAction("Index");
        }

        // GET: People/tree/5
        public ActionResult tree (int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var people = repository.Get(id);

            if (people == null)
            {
                return NotFound();
            }

            textTree = new StringBuilder();
            PreOrder_Rec(people.PersonId);
            ViewBag.tree = textTree.ToString();

            return View("tree", people);
        }

        private void PreOrder_Rec(int? Id)
        {
            if (Id != null)
            {
                People person = repository.Get(Id);

                StringBuilder spaces = new StringBuilder();
                for (int i =0; i < Line ; i++)
                {
                    spaces.Append(tab);
                }

                textTree.Append(spaces.ToString ()+"{Name :" + person.Name + " , <br />");
                if ((person.Parent1Id != null) && (person.Parent2Id != null))
                {
                    textTree.Append(spaces.ToString() + tab + "Parents : [ <br />");
                    Line++;
                }
                Line++;
                int tempLine = Line;
                StringBuilder tempSpaces = new StringBuilder();

                for (int j = 0; j < tempLine; j++)
                {
                    tempSpaces.Append(tab);
                }
                PreOrder_Rec(person.Parent1Id);
                PreOrder_Rec(person.Parent2Id);
                textTree.Append(tempSpaces.ToString() + " ]}  <br />");
            }
        }

        private int Line = 0 ; 
        private StringBuilder textTree;
        const string tab = "&nbsp;&nbsp;&nbsp;&nbsp;";


        // POST: People/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("PersonId,Name,Birthday,Parent1Id,Parent2Id")] People people)
        {
            if (ModelState.IsValid)
            {
                repository.Add(people);
                return RedirectToAction("Index");
            }
            return View(people);
        }

        // GET: People/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var people = repository.Get (id) ;
            if (people == null)
            {
                return NotFound();
            }
            return View(people);
        }

        // POST: People/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PersonId,Name,Birthday,Parent1Id,Parent2Id")] People people)
        {
            if (id != people.PersonId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                repository.Update(people);
                return RedirectToAction("Index");
            }
            return View(people);
        }

        // GET: People/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var people = repository.Get(id);
            if (people == null)
            {
                return NotFound();
            }

            return View(people);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            repository.Delete (id);
            return RedirectToAction("Index");
        }

        //private bool PeopleExists(int id)
        //{
        //    return _context.People.Any(e => e.PersonId == id);
        //}
    }
}
