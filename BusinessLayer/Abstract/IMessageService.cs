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
        List<Message> GetListInbox(string ReceiverMail);
        List<Message> GetListSendbox(string SenderMail);
        List<Message> GetListInboxRemoved(string ReceiverMail);
        List<Message> GetListSendboxRemoved(string SenderMail);
        List<Message> GetListInboxNotRead(string ReceiverMail);
        void AddMessage(Message message);
        Message GetMessage(int id);
        void DeleteMessage(Message message);
        void UpdateMessage(Message message);
    }
}
