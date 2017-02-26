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
        public ActionResult parents (int id)
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
        public ActionResult children(int id)
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
                textTree.Append(Environment.NewLine+"Name :" + person.Name );
                Console.Write(person.Name + "   ");
                PreOrder_Rec(person.Parent1Id);
                PreOrder_Rec(person.Parent2Id);
            }
        }


        private StringBuilder textTree;


        //    // POST: People/Create
        //    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //    [HttpPost]
        //    [ValidateAntiForgeryToken]
        //    public async Task<IActionResult> Create([Bind("PersonId,Name,Birthday,Parent1Id,Parent2Id")] People people)
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            _context.Add(people);
        //            await _context.SaveChangesAsync();
        //            return RedirectToAction("Index");
        //        }
        //        return View(people);
        //    }

        //    // GET: People/Edit/5
        //    public async Task<IActionResult> Edit(int? id)
        //    {
        //        if (id == null)
        //        {
        //            return NotFound();
        //        }

        //        var people = await _context.People.SingleOrDefaultAsync(m => m.PersonId == id);
        //        if (people == null)
        //        {
        //            return NotFound();
        //        }
        //        return View(people);
        //    }

        //    // POST: People/Edit/5
        //    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //    [HttpPost]
        //    [ValidateAntiForgeryToken]
        //    public async Task<IActionResult> Edit(int id, [Bind("PersonId,Name,Birthday,Parent1Id,Parent2Id")] People people)
        //    {
        //        if (id != people.PersonId)
        //        {
        //            return NotFound();
        //        }

        //        if (ModelState.IsValid)
        //        {
        //            try
        //            {
        //                _context.Update(people);
        //                await _context.SaveChangesAsync();
        //            }
        //            catch (DbUpdateConcurrencyException)
        //            {
        //                if (!PeopleExists(people.PersonId))
        //                {
        //                    return NotFound();
        //                }
        //                else
        //                {
        //                    throw;
        //                }
        //            }
        //            return RedirectToAction("Index");
        //        }
        //        return View(people);
        //    }

        //    // GET: People/Delete/5
        //    public async Task<IActionResult> Delete(int? id)
        //    {
        //        if (id == null)
        //        {
        //            return NotFound();
        //        }

        //        var people = await _context.People
        //            .SingleOrDefaultAsync(m => m.PersonId == id);
        //        if (people == null)
        //        {
        //            return NotFound();
        //        }

        //        return View(people);
        //    }

        //    // POST: People/Delete/5
        //    [HttpPost, ActionName("Delete")]
        //    [ValidateAntiForgeryToken]
        //    public async Task<IActionResult> DeleteConfirmed(int id)
        //    {
        //        var people = await _context.People.SingleOrDefaultAsync(m => m.PersonId == id);
        //        _context.People.Remove(people);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }

        //    private bool PeopleExists(int id)
        //    {
        //        return _context.People.Any(e => e.PersonId == id);
        //    }
    }
}
