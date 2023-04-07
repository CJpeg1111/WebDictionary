using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IImageFileService
    {
        List<ImageFile> GetList();
        void AddImageFile(ImageFile imageFile);
        ImageFile GetImageFile(int id);
        void DeleteImageFile(ImageFile imageFile);
        void UpdateImageFile(ImageFile imageFile);
    }
}
