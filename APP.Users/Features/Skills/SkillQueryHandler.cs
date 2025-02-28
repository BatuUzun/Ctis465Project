using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APP.Users.Domain;
using CORE.APP.Features;
using MediatR;

namespace APP.Users.Features.Skills
{
    public class SkillQueryRequest : Request, IRequest<IQueryable<SkillQueryResponse>>
    {
    }

    public class SkillQueryResponse : QueryResponse
    {
        public string Name { get; set; }
    }

    public class SkillQueryHandler : UsersDbHandler, IRequestHandler<SkillQueryRequest, IQueryable<SkillQueryResponse>>
    {
        public SkillQueryHandler(UsersDb db) : base(db)
        {
        }

        public Task<IQueryable<SkillQueryResponse>> Handle(SkillQueryRequest request, CancellationToken cancellationToken)
        {
            var query = _db.Skills.OrderBy(s => s.Name).Select(s => new SkillQueryResponse()
            {
                Id = s.Id,
                Name = s.Name
            });
            return Task.FromResult(query);
        }
    }
}
