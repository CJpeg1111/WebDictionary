using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class DraftMessageManager : IDraftMessageService
    {
        IDraftMessageDal _draftMessageDal;
        public DraftMessageManager(IDraftMessageDal draftMessageDal)
        {
            _draftMessageDal = draftMessageDal;
        }

        public void AddDraftMessage(DraftMessage draftMessage)
        {
            _draftMessageDal.Insert(draftMessage);
        }

        public void DeleteDraftMessage(DraftMessage draftMessage)
        {
            _draftMessageDal.Delete(draftMessage);
        }

        public DraftMessage GetDraftMessage(int id)
        {
            return _draftMessageDal.Get(x => x.DraftMessageId == id);
        }

        public List<DraftMessage> GetList()
        {
            return _draftMessageDal.List(x => x.DraftSenderMail == "admin@gmail.com");
        }

        public void UpdateDraftMessage(DraftMessage draftMessage)
        {
           _draftMessageDal.Update(draftMessage);
        }
    }
}
