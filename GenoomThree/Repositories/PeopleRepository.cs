using GenoomThree.Models;
using GenoomTree.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenoomTree.Repositories
{
    public class PeopleRepository : IPeopleRepository
    {
        private readonly GenoomContext db;


        #region public methods
        public PeopleRepository(GenoomContext context)
        {
            db = context;
        }
        public People Get (int? id)
        {
            People people = db.People.SingleOrDefault (c => c.PersonId == id);
            return people;
        }

        public IEnumerable<People> GetPeople ()
        {
            return db.People.ToList();
        }

        public IEnumerable<People> GetFamily (int? id)
        {
            People person = Get(id);

            List<People> parents = GetParents(id).ToList();

            List<People> siblings = GetSiblings(id).ToList();

            List<People> childrend = GetChildrens(id).ToList();

            List<People> partners = GetPartners(id).ToList();

            List<People> family = new List<People>();

            family.AddRange(parents);
            family.AddRange(siblings);
            family.AddRange(childrend);
            family.AddRange(partners);

            return family;
        }

        public bool PersonHaveParents (int id)
        {
            var parents = db.People.Where(a => a.Parent1Id == null && a.Parent2Id == null && a.PersonId == id).SingleOrDefault ();
            return (parents == null);
        }

        public int CreateEmptyParent1 (int id)
        {
            People parent1 = new People();
            db.People.Add(parent1);
             db.SaveChanges();
            UpdateParent1(id, parent1.PersonId);
            return parent1.PersonId;

        }

        public int CreateEmptyParent2(int id)
        {
            People parent2 = new People();
            db.People.Add(parent2);
            db.SaveChanges();
            UpdateParent2(id, parent2.PersonId);
            return parent2.PersonId;
        }

        public People PersonPartner (int Id)
        {
            People partner = GetPartners(Id).FirstOrDefault();

            if (partner == null)
            {
                return null;
            }          

            return partner;            
        }

        public int CreateChild (int PersonId , int PartnerId)
        {
            People child = new People();
            child.Parent1Id = PersonId;
            child.Parent2Id = PartnerId;
            db.People.Add(child);
            db.SaveChanges();
            return child.PersonId;
        }

        public void Add (People person)
        {
            db.People.Add(person);
            db.SaveChanges();
        }

        public void Delete (int? id)
        {
            People person = Get(id);
            db.People.Remove(person);
            db.SaveChanges();
        }

        public void Update(People person)
        {
            db.Entry(person).State = Microsoft.EntityFrameworkCore.EntityState.Modified ;
            db.SaveChanges();
        }


        #endregion

        #region Private methods
        private void UpdateParent1 (int id, int Parent1Id)
        {
            People person = Get(id);
            person.Parent1Id = Parent1Id;
            db.Entry(person).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            db.SaveChanges();
        }
        private void UpdateParent2(int id, int Parent2Id)
        {
            People person = Get(id);
            person.Parent2Id = Parent2Id;
            db.Entry(person).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            db.SaveChanges();
        }

        private IEnumerable<People> GetParents (int? id)
        {
            People people = Get(id);

            People parent1 = db.People.Where(a => a.PersonId == people.Parent1Id).SingleOrDefault();

            People parent2 = db.People.Where(a => a.PersonId == people.Parent2Id).SingleOrDefault();

            List <People> result = new List<People> () ;
            result.Add(parent1);
            result.Add(parent2);

            return result; 

        }

        private IEnumerable<People> GetSiblings (int? id)
        {
            People people = Get(id);

            return db.People.Where( a => a.Parent1Id == people.Parent1Id  && a.Parent1Id != people.PersonId || a.Parent2Id == people.Parent2Id && a.Parent1Id != people.PersonId);

        }

        private IEnumerable<People> GetChildrens (int? id)
        {

            People person = Get(id);

            return db.People.Where(a => a.Parent1Id == person.PersonId || a.Parent2Id == person.PersonId);       

        }

        private IEnumerable<People> GetPartners(int? id)
        {

            People parent = Get(id);

            List<People> childrens = GetChildrens(id).ToList();

            List<People> partners = new List<People>();

            foreach (People child in childrens )
            {
                bool Exist = false;
                foreach (People paren in partners )
                {
                    if ((paren.PersonId == child.Parent1Id ) || (paren.PersonId == child.Parent2Id))
                    {
                        Exist = true;
                    }
                }

                if (Exist)
                    continue;
                    if (child.Parent1Id != null && child.Parent1Id != parent.PersonId )
                {
                    partners.Add(Get(child.Parent1Id));
                }

                
                if (child.Parent2Id != null && child.Parent2Id != parent.PersonId)
                {
                    partners.Add(Get(child.Parent2Id));
                }
            }

            return partners;

        }

        #endregion

    }

}
