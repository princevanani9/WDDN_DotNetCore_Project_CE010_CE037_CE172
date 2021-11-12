using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChattingApplication.Models
{
    public class SqlUserRepository:IUserRepository
    {
        private readonly UserDbContext context;
        public SqlUserRepository(UserDbContext context)
        {
            this.context = context;
        }
        User IUserRepository.Add(User user)
        {
            context.Users.Add(user);
            context.SaveChanges();
            return user;
        }
        User IUserRepository.Update(User user)
        {
            context.Users.Update(user);
            context.SaveChanges();
            return user;
        }
        User IUserRepository.hasUser(string Username, string Password)
        {
            return context.Users.FirstOrDefault(u => u.Username == Username && u.Password == Password);
        }
        IEnumerable<User> IUserRepository.GetAllUsers()
        {
            return context.Users;
        }
        User IUserRepository.GetUser(int Id)
        {
            return context.Users.Find(Id);
        }
        User IUserRepository.checkUser(string Username)
        {
            return context.Users.FirstOrDefault(u => u.Username == Username);
        }
        User IUserRepository.GetUsername(String name)
        {
            return context.Users.FirstOrDefault(u => u.Username == name);
        }
        int IUserRepository.GetId(String name)
        {
            return context.Users.FirstOrDefault(u => u.Username == name).Id;
        }
        Chat IUserRepository.AddChat(Chat chat)
        {
            context.Chats.Add(chat);
            context.SaveChanges();
            return chat;
        }
        IEnumerable<Chat> IUserRepository.GetAllChat()
        {
            return context.Chats;
        }


    }

}
