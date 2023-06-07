using EntityLayer;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IHeadingService
    {
        List<Heading> GetList();
        List<Heading> GetListActive();
        List<Heading> GetListByWriter(int id);
        List<Heading> GetListByCategory(int id);
        List<Heading> GetListAgenda();
        void AddHeading(Heading heading);
        Heading GetHeading(int id);
        void DeleteHeading(Heading heading);
        void UpdateHeading(Heading heading);
        List<CategoryHeadingChart> ListCategoryHeading();
        
    }
}
