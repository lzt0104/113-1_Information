using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeaDemo.Models;

namespace TeaDemo.DataAccess.Repository.IRepository
{
    public interface ICategoryRepository :
    IRepository<Category>
    {
        void Update(Category obj);
        void Save();
    }
}
