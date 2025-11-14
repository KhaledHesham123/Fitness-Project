using IdentityService.Features.Authantication.DTOS;
using IdentityService.Shared.Entities;
using IdentityService.Shared.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Features.Authantication.CQRS.Commends
{
    public record LoginCommend(UserLoginDTO UserLoginDTO):IRequest<UserLoginToReturnDTO>;

    public class LoginCommendHandler : IRequestHandler<LoginCommend, UserLoginToReturnDTO>
    {
        private readonly IRepository<User> repository;

        public LoginCommendHandler(IRepository<User> repository)
        {
            this.repository = repository;
        }

        public async Task<UserLoginToReturnDTO> Handle(LoginCommend request, CancellationToken cancellationToken)
        {
            var user = await repository.GetByCriteriaAsync(x => x.Email == request.UserLoginDTO.Email);
            //var user = await repository.GetQueryableByCriteria(x => x.Email == request.UserLoginDTO.Email)
            //    .Include(u => u.Role).Include(u => u.Claims).FirstOrDefaultAsync();

            var hasher = new PasswordHasher<User>();

            if (user == null)
            {
                return null; //untill we make RequestResponse
            }

            var isvalid = hasher.VerifyHashedPassword(user,user.HashPassword,request.UserLoginDTO.Password);
            if (isvalid == PasswordVerificationResult.Failed)
            {
                return null;

            }

            return new UserLoginToReturnDTO
            {
                //role= user.Role.Name,
                Token= "Some JWT Token"

            };      


        }
    }


}
