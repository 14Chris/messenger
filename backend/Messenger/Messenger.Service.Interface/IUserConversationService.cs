﻿using Messenger.Database;
using Messenger.Facade.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Service.Interface
{
    public interface IUserConversationService
    {
        public Task<ResponseObject> UpdateUserConversation(UserConversation user);
        public ResponseObject GetConversationIdUsers(int idConversation);
        public ResponseObject GetConversationUsers(int idConversation);
    }
}
