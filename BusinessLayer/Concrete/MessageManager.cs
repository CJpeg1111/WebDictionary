using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class MessageManager : IMessageService
    {
        IMessageDal _messageDal;
        public MessageManager(IMessageDal messageDal)
        {
            _messageDal = messageDal;
        }

        public void AddMessage(Message message)
        {
            _messageDal.Insert(message);
        }

        public void DeleteMessage(Message message)
        {
            _messageDal.Delete(message);
        }

        public List<Message> GetListInbox()
        {
            return _messageDal.List(x => x.ReceiverMail == "admin@gmail.com" && x.Remove == false);
        }

        public List<Message> GetListInboxNotRead()
        {
            return _messageDal.List(x => x.ReceiverMail == "admin@gmail.com" && x.Remove == false && x.IsRead == false);
        }

        public List<Message> GetListInboxRemoved()
        {
            return _messageDal.List(x => x.ReceiverMail == "admin@gmail.com" && x.Remove == true);
        }

        public List<Message> GetListSendbox()
        {
            return _messageDal.List(x => x.SenderMail == "admin@gmail.com" && x.Remove == false);
        }

        public List<Message> GetListSendboxRemoved()
        {
            return _messageDal.List(x => x.SenderMail == "admin@gmail.com" && x.Remove == true);
        }

        public Message GetMessage(int id)
        {
            return _messageDal.Get(x => x.MessageId == id);
        }

        public void UpdateMessage(Message message)
        {
            _messageDal.Update(message);
        }
    }
}
