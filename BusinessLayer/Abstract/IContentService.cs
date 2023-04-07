using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IContentService
    {
        List<Content> GetList();
        List<Content> GetListById(int id);
        void AddContent(Content content);
        Content GetContent(int id);
        void DeleteContent(Content content);
        void UpdateContent(Content content);
    }
}
