using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IDraftMessageService
    {
        List<DraftMessage> GetList();
        void AddDraftMessage(DraftMessage draftMessage);
        DraftMessage GetDraftMessage(int id);
        void DeleteDraftMessage(DraftMessage draftMessage);
        void UpdateDraftMessage(DraftMessage draftMessage);
    }
}
