using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APP.Users.Domain;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace APP.Users.Features.Users
{
    public class UserUpdateRequest : Request, IRequest<CommandResponse>
    {
        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string UserName { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 3)]
        public string Password { get; set; }

        public bool IsActive { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Surname { get; set; }

        public DateTime? RegistrationDate { get; set; }

        public int RoleId { get; set; }

        public List<int> SkillIds { get; set; }
    }

    public class UserUpdateHandler : UsersDbHandler, IRequestHandler<UserUpdateRequest, CommandResponse>
    {
        public UserUpdateHandler(UsersDb db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(UserUpdateRequest request, CancellationToken cancellationToken)
        {
            if (_db.Users.Any(u => u.Id != request.Id && (u.UserName == request.UserName || (u.Name == request.Name && u.Surname == request.Surname))))
                return Error("User with the same user name or full name exists!");
            var user = _db.Users.Include(u => u.UserSkills).SingleOrDefault(u => u.Id == request.Id);
            if (user is null)
                return Error("User not found!");
            _db.UserSkills.RemoveRange(user.UserSkills);
            user.IsActive = request.IsActive;
            user.Name = request.Name;
            user.Password = request.Password;
            user.RoleId = request.RoleId;
            user.Surname = request.Surname;
            user.UserName = request.UserName;
            user.RegistrationDate = request.RegistrationDate;
            user.SkillIds = request.SkillIds;
            _db.Users.Update(user);
            await _db.SaveChangesAsync(cancellationToken);
            return Success("User updated successfully.", user.Id);
        }
    }
}
