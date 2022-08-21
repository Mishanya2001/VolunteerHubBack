﻿using Application.UnitOfWorks;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Models;
using MediatR;

namespace Application.Commands.PostConnections
{
    public class CreatePostConnectionCommand : IRequest<PostConnection>
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public int VolunteerPostId { get; set; }
        public int NeedfulPostId { get; set; }
    }
    public class CreatePostHandler : IRequestHandler<CreatePostConnectionCommand, PostConnection>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreatePostHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<PostConnection> Handle(CreatePostConnectionCommand request, CancellationToken cancellationToken)
        {
            var postConnection = new PostConnection()
            {
                Title = request.Title,
                Message = request.Message,
                VolunteerPost = await PostValidation(request.VolunteerPostId, PostType.PROPOSITION),
                NeedfulPost = await PostValidation(request.NeedfulPostId, PostType.REQUEST)
            };

            await _unitOfWork.PostConnections.Insert(postConnection);
            await _unitOfWork.SaveChanges();
            return postConnection;
        }
        private async Task<Post> PostValidation(int id, PostType expectedType)
        {
            var post = await _unitOfWork.Posts.GetById(id);

            if (post == null)
            {
                throw new PostNotFoundException(id.ToString());
            }
            else if (post.PostType != expectedType)
            {
                throw new WrongPostTypeException(id.ToString(), post.PostType.ToString(), expectedType.ToString());
            }

            return post;
        }
    }
}
