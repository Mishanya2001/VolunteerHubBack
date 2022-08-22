﻿using Application.UnitOfWorks;
using Domain.Models;
using MediatR;

namespace Application.Posts.Queries
{
    public record GetPostsQuery() : IRequest<IEnumerable<Post>>;

    public class GetPostsHandler : IRequestHandler<GetPostsQuery, IEnumerable<Post>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPostsHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<Post>> Handle(GetPostsQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Posts.Get(commaSeparatedIncludeProperties: "User,Tags");
        }
    }
}
