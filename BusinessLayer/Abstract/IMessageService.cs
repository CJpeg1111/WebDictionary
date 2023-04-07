using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IMessageService
    {
        List<Message> GetListInbox();
        List<Message> GetListSendbox();
        List<Message> GetListInboxRemoved();
        List<Message> GetListSendboxRemoved();
        List<Message> GetListInboxNotRead();
        void AddMessage(Message message);
        Message GetMessage(int id);
        void DeleteMessage(Message message);
        void UpdateMessage(Message message);
    }
}
