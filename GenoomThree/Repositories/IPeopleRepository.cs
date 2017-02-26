using GenoomTree.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenoomTree.Repositories
{
    public interface IPeopleRepository
    {
        People Get(int? id);
        IEnumerable<People> GetFamily(int? id);
        IEnumerable<People> GetPeople();
        bool PersonHaveParents(int id);
        int CreateEmptyParent1(int id);
        int CreateEmptyParent2(int id);
        People PersonPartner(int Id);
        int CreateChild(int PersonId, int PartnerId);
    }
}
