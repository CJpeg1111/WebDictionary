using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IWriterService
    {
        List<Writer> GetList();
        void AddWriter(Writer writer);
        Writer GetWriter(int id);
        void DeleteWriter(Writer writer);
        void UpdateWriter(Writer writer);
    }
}
