﻿namespace Application.Commands.Posts
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(int userId) : base($"User with id = {userId} was not found")
        {
        }
        public UserNotFoundException(string email) : base($"User with email = {email} was not found")
        {
        }
    }
}